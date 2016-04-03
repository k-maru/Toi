SELECT * FROM EMP
WHERE 1 = 1
-- if(!string.IsNullOrEmpty(Model.Name)) {
AND EMP.Name LIKE '%' + 'Foo'/* Builder.ToParameter("Name", Model.Name); */ + '%'
-- }
