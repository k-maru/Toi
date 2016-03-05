SELECT * FROM EMP
WHERE 1 = 1
-- if(!string.IsNullOrEmpty(Context.Model.Name)) {
AND EMP.Name LIKE '%' + 'Foo'/* Context.Builder.ToParameter("Name", Context.Model.Name); */ + '%'
-- }
