SELECT * FROM EMP
WHERE 1 = 1
-- if(!string.IsNullOrEmpty(Context.Model.Name)) {
AND EMP.Name LIKE '%' + 'Foo'/*Context.QueryParameter(nameof(Context.Model.Name), Context.Model.Name);*/ + '%'
-- }
