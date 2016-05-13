# Toi.Template

## これは何?

SQLのコメントにC#の構文を記述してSQL文とパラメーターの定義を生成するテンプレートエンジンです。

## なぜ作ったのか?

世の中にはEntity FrameworkのようにLINQ構文でRDBアクセスできるものがありますが、以下が不満でした。

- 職人技のようなSQLを書けない
- 本気のチューニング時にはやっぱり生SQLから逃げられない
- INSERT SELECTとかMERGEとか使えない
- その他、SQLだとできるし楽になれるっていうシーンがそれなりにある

マイクロORMではDapperとかが有名ですね。ただ

- SQL文自体は直接指定なため管理方法を考えないといけない。文字列連結は辛すぎる

ということで、SQL文を書きたいけど、どうにかコード内に文字列連結は避けられないか。ということを目的に作りました。
ただし、古き良き、「じゃぁ、SQLはファイルとして外だしだ！」だけだと、条件が増減するときのANDやORでの連結の判定はどうするとか、
パラメーター名はプログラム内とSQLファイルの離れたところに書いてあって気が付いたら不一致してたとか、それなりに厄介な問題が発生します。
そうならないためにSQL文の生成とパラメーターの設定を行うテンプレートエンジンを作りました。

## なにが嬉しいのか?

SQLのコメント形式でC#のコードが書けます。例えば検索条件の付け替えとか自由自在。

```SQL
SELECT * 
FROM USERS 
WHERE 1 = 1
-- if(true) {
AND USERS.NAME = 'Test'
-- } 
```

C#コードがコメント形式なので、そのままSQLとして実行できます。もちろん、ある程度意識する必要はありますが。

パラメーターも渡されたモデルの値をダミーとして設定している値からSQLパラメーター文字に置きなおします。
以下のようなテンプレートとすると

```sql
SELECT * 
FROM USERS 
WHERE 1 = 1
-- if(true) {
AND USERS.NAME = 'Test'/*Builder.ToParameter(nameof(Model.Name), Model.Name);*/
-- } 
```

以下のようなSQLになります。

```sql
SELECT * 
FROM USERS 
WHERE 1 = 1
AND USERS.NAME = @Name
-- } 
```

## どうやって使うのか?

SQL文字列とモデルのインスタンスを渡して実行するだけです。
結果として生成したSQL文字列とDbParameterと形を合わせたパラメーターを返すため後は煮るなり焼くなり好きにしてくださいというスタンスです。

```c#
class User
{
    public string Name { get; set; }
}

var sqlTmpl = @"
SELECT * 
FROM USERS 
WHERE 1 = 1
-- if(true) {
AND USERS.NAME = 'Test'/*Builder.ToParameter(nameof(Model.Name), Model.Name);*/
-- }";

var model = new User() 
{
    Name = "Foo"
};

var engine = new SqlTemplateEngine();
var define = engine.ExecuteAsync(sqlTmpl, model);

var sql = define.SqlText;
var params = define.Parameters;
```

実際のところはこれだけだと全く使えないので、これをもとにSQLを実行する何かと組み合わせて使う形式となります。

## 構文は？

### C#の記述

SQLの行コメントおよびブロックコメントに記述します。

```sql
-- var name = "Foo";
/*
var age = 26;
*/
```

ブロックコメントで記述されたC#で、文字列の中に "/*" が入ってしまう場合、SQLとしてはブロックコメントのネストになってしまいますが、対応しています。
少し、特殊な書き方になってしまいますが、あくまでもSQL文として素で実行できることを優先しました。

```sql
/*
var name = "/*"; // */
*/
```

少しわかりにくいですが、C#中の行の末尾にC#の行コメントを追加して"*/"を追加します。

### SQLのコメント

コメント内をC#のコードとするだけだと、SQL自体のコメントを記述することができなくなってしまうため、コメント構文が定義されています。
SQLの行コメントおよびブロックコメントの先頭に"!"を追加し、空白をあけることで、SQLのコメントだと認識します。

```sql
--! this is line comment
/*!
this is block comment
*/
```

### 文字列としての書き出し

テンプレート上はSQLのコメントとしておきたいが、展開後はSQL文として利用したい文字列などがある場合は、"?"を利用します。

```sql
SELECT * FROM USERS
WHERE USERS.NAME = 'Foo'
--? AND USERS.AGE > 18
```

### Oracleのヒント句

Oracleのヒント句に対応するために"+"も特殊な値として解釈されます。"+"が指定されているブロックコメントはそのまま展開されます。

```sql

/*+ this is oracle hint*/

```

## 機能は？

### モデルの利用

渡されたモデルはModel変数に格納されます。

```sql
-- var name = Model.Name;
/*
foreach(var favorite in Model.Favorites)
{
    // do anything.
}
*/
```

### パラメーターへの変換

SQL文字列およびパラメーターの定義を構築するためのISqlDefinitionBuilderのインスタンスを格納するBuilder変数に
拡張メソッドでToParameter/ToInParameterメソッドが定義されています。

```sql
SELECT * 
FROM USERS 
WHERE 1 = 1
-- if(!string.IsNullOrEmpty(Model.Name)) {
AND USERS.NAME = 'Test'/*Builder.ToParameter(nameof(Model.Name), Model.Name);*/
-- } 
-- if(Model.Favorites.Count > 0){
AND USERS.FAVORITE IN ('CAR', 'PC', 'GAME'/*Builder.ToInParameter("Fav", Model.Favaorites);*/)
-- }
```

以下のようなSQL文字列に展開されます。

```sql
SELECT * 
FROM USERS 
WHERE 1 = 1
AND USERS.NAME = @Name
AND USERS.FAVORITE IN (@Fav1, @Fav2, @Fav3)
```

SQLパラメーターのプレフィックス(上記では@)は、もちろんオプションで切り替えることができます。

### コードブロック

一定の範囲をブロックとして名前を付けて、後から利用するかどうかを設定できます。
例えば、Where句の条件が動的に変わる場合に、その条件に必要なFrom句を追加しなければならない場合などの利用を想定しています。

```sql
SELECT USERS.* FROM USERS
-- Builder.StartBlock("Group");
INNER JOIN GROUPS
ON USERS.GROUPID = GROUPS.GROUPID
-- Builder.EndBlock();
WHERE 1 = 1
AND USERS.NAME LIKE '%FOO%'
-- if(Model.GroupId.HasValue) {
AND GROUPS.ID = 1/*Builder.ToParameter(nameof(Model.GroupId), Model.GroupId);*/
-- Builder.UseBlock("Group");
-- }
```

## 今後のプラン

- SQL文のロード機能
 - ファイルやその他ストレージから
- Entity Frameworkを利用したSQL定義からの実行(作りかけ)
- Dapperを利用したSQL定義からの実行
- アセンブリ参照の指定
- NuGet配布
- APIドキュメント