SELECT * FROM Emp
-- Context.StartMarker("Com");
INNER JOIN Com
ON Emp.ComId = Com.ComId
-- Context.EndMarker();
WHERE 1 = 1
--! 従業員名
-- if(!string.IsNullOrEmpty(Model.Name)){
AND Emp.EmpName = 'Foo'/* Context.SqlParameter(nameof(Model.Name), Model.Name); */
-- }

--! 部署
-- if(Model.Devisions.ToSafe().Any()){
And Emp.EmpDivision IN (1,2/* Context.SqlInParameter("Division", Model.Divisions); */)
-- }

--! 企業ID
/*
if(Model.ComId.HasValue) {
Context.UseMarked("Com");
*/
AND Com.ComId = 1/* Context.SqlParameter(nameof(Model.ComId), Model.ComId.Value); */
--}

--! 年齢
-- if(Mode.FromAge.HasValue || Model.ToAge.HasValue) {
AND (
	-- if(Model.FromAge.HasValue) {
	Emp.Age > 20/* Context.SqlParameter(nameof(Model.FromAge), Model.FromAge.Value); */
	-- }
	OR-- Context.RemovePrevIf(!Model.FromAge.HasValue || !Model.ToAge.HasValue);
	-- if(Model.ToAge.HasValue) {
	Emp.Age < 40/* Context.SqlParameter(nameof(Model.ToAge), Model.ToAge.Value); */
	-- }
)
--}

--! 住所
-- if(Model.Addresses.ToSafe().Any()) {
AND (
	-- var count = 0;
	-- foreach(var addresss in Model.Addresses) {
	Emp.Address LIKE '%京都%'/* Context.SqlLike("Address" + count.ToString(), address, SqlLike.Both); */
	-- Context.Texts.AddIf(count != 0, " OR ");
	-- } 
	-- if(false) {
	OR Emp.Address LIKE '%大阪%'
	OR Emp.Address LIKE '%東京%'
	-- }
)
-- }