using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Km.Toi.Definition.Test
{
    public class QueryTextBuilderTest
    {
        [Fact]
        public void テキストを追加できる()
        {
            var tb = new QueryTextBuilder();
            tb.Add("value1,");
            tb.Add("value2,");
            tb.Add("value3,");

            Assert.Equal("value1,value2,value3,", tb.ToString());
        }

        [Fact]
        public void コメントを追加できる()
        {
            var tb = new QueryTextBuilder();
            tb.Add("comment1-line", TextType.LineComment);
            tb.Add("comment2-block", TextType.BlockComment);
            Assert.Equal($"--comment1-line{Environment.NewLine}/*comment2-block*/", tb.ToString());
        }

        [Fact]
        public void TextTypeがnoneの場合は値が追加されない()
        {
            var tb = new QueryTextBuilder();
            tb.Add("value1")
              .Add("value2", TextType.Plain)
              .Add("none", TextType.None)
              .Add("value3", TextType.Plain);

            Assert.Equal("value1value2value3", tb.ToString());
        }

        [Fact]
        public void 追加されていない場合は空文字()
        {
            var tb = new QueryTextBuilder();
            Assert.Empty(tb.ToString());
        }

        [Fact]
        public void AddIfは追加するかどうかを指定できる()
        {
            var tb = new QueryTextBuilder();
            tb.Add("value1,");
            tb.AddIf(false, "value2,");
            tb.Add("value3,");
            tb.AddIf(true, "value4,");
            tb.Add("value5,");

            Assert.Equal("value1,value3,value4,value5,", tb.ToString());
        }

        [Fact]
        public void 直前の設定を削除できる()
        {
            var tb = new QueryTextBuilder();
            tb.Add("value1,");
            tb.RemovePrev();
            tb.Add("value2,");
            tb.Add("value3,");
            tb.RemovePrev();
            tb.Add("value5,");

            Assert.Equal("value2,value5,", tb.ToString());
        }

        [Fact]
        public void 直前の設定を繰り返し削除できる()
        {
            var tb = new QueryTextBuilder();
            tb.Add("value1,");
            tb.Add("value2,");
            tb.Add("value3,");
            tb.RemovePrev();
            tb.RemovePrev();
            tb.Add("value5,");

            Assert.Equal("value1,value5,", tb.ToString());
        }

        [Fact]
        public void 直前の値がない場合は無視される()
        {
            var tb = new QueryTextBuilder();
            tb.RemovePrev();
            tb.Add("value1,");
            tb.RemovePrev();
            tb.RemovePrev();

            Assert.Empty(tb.ToString());
        }

        [Fact]
        public void 直前の値の削除はブロックは無視される()
        {
            var tb = new QueryTextBuilder();
            tb.RemovePrev()
            .Add("value1,")
            .StartBlock("Marker1")
                .Add("v1,")
            .EndBlock()
            .RemovePrev()
            .Add("value2,");

            Assert.Equal("value2,", tb.ToString());
            tb.UseBlock("Marker1");
            Assert.Equal("v1,value2,", tb.ToString());
        }

        [Fact]
        public void 直前の値を変更できる()
        {
            var tb = new QueryTextBuilder();
            tb.Add("value1,")
            .Add("value2,")
            .ReplacePrev("replace2,")
            .Add("value3,")
            .ReplacePrev(pv =>
            {
                Assert.Equal("value3,", pv);
                return "replace3,";
            });

            Assert.Equal("value1,replace2,replace3,", tb.ToString());
        }


        [Fact]
        public void ブロックを設定できる()
        {
            var tb = new QueryTextBuilder();
            tb.Add("value1,");
            tb.Add("value2,");
            tb.StartBlock("Mark1").Add("v-1,").Add("v-2,").EndBlock();
            tb.Add("value3,");

            Assert.Equal("value1,value2,value3,", tb.ToString());
        }

        [Fact]
        public void ブロックはUseすることで有効になる()
        {
            var tb = new QueryTextBuilder();
            tb.Add("value1,");
            tb.Add("value2,");
            tb.StartBlock("Mark1").Add("v-1,").Add("v-2,").EndBlock();
            tb.Add("value3,");

            Assert.Equal("value1,value2,value3,", tb.ToString());
            tb.UseBlock("Mark1");
            Assert.Equal("value1,value2,v-1,v-2,value3,", tb.ToString());
        }

        [Fact]
        public void ブロックはネストすることができる()
        {
            var tb = new QueryTextBuilder();
            tb.Add("value1,")
            .Add("value2,")
            .StartBlock("Mark1")
                .Add("v-1,")
                .StartBlock("Mark2")
                    .Add("v-1-1,")
                    .Add("v-2-1,")
                .EndBlock()
                .Add("v-2,")
            .EndBlock()
            .Add("value3,");

            Assert.Equal("value1,value2,value3,", tb.ToString());
            tb.UseBlock("Mark1");
            Assert.Equal("value1,value2,v-1,v-2,value3,", tb.ToString());
            tb.UseBlock("Mark2");
            Assert.Equal("value1,value2,v-1,v-1-1,v-2-1,v-2,value3,", tb.ToString());

        }

        [Fact]
        public void 孫のブロックが有効になっていても子供が無効の場合は利用されない()
        {
            var tb = new QueryTextBuilder();
            tb.Add("value1,")
            .Add("value2,")
            .StartBlock("Mark1")
                .Add("v-1,")
                .StartBlock("Mark2")
                    .Add("v-1-1,")
                    .Add("v-2-1,")
                .EndBlock()
                .Add("v-2,")
            .EndBlock()
            .Add("value3,");

            Assert.Equal("value1,value2,value3,", tb.ToString());
            tb.UseBlock("Mark2");
            Assert.Equal("value1,value2,value3,", tb.ToString());
            tb.UseBlock("Mark1");
            Assert.Equal("value1,value2,v-1,v-1-1,v-2-1,v-2,value3,", tb.ToString());
        }

        [Fact]
        public void ブロックに同じ名前は設定できない()
        {
            var tb = new QueryTextBuilder();
            tb.Add("value1");
            tb.StartBlock("Block").Add("v-1").EndBlock();
            Assert.Throws<ArgumentException>("name", () => tb.StartBlock("Block"));
        }

        [Fact]
        public void ブロックに親子関係が違っても同じ名前は設定できない()
        {
            var tb = new QueryTextBuilder();

            //子供
            tb.Add("value1");
            tb.StartBlock("Block").Add("v-1");
            Assert.Throws<ArgumentException>("name", () => tb.StartBlock("Block"));

            //孫
            tb = new QueryTextBuilder();
            tb.Add("value1");
            tb.StartBlock("Block").Add("v-1")
                .StartBlock("Block1").Add("v-1-1");
            Assert.Throws<ArgumentException>("name", () => tb.StartBlock("Block"));

            //別階層の孫
            tb = new QueryTextBuilder();
            tb.Add("value1");
            tb.StartBlock("Block1").Add("v-1")
                .StartBlock("Block1-1").Add("v-1-1")
                .EndBlock()
            .EndBlock()
            .StartBlock("Block2").Add("v-1")
                .StartBlock("Block2-1").Add("v-1-1").EndBlock();

            Assert.Throws<ArgumentException>("name", () => tb.StartBlock("Block1-1"));

        }
    }
}
