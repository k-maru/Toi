--i Km.Toi.Template.Test
SELECT * FROM EMP
WHERE 1 = 1
-- if(!string.IsNullOrEmpty(Context.Model.Name)) {
AND EMP.Name LIKE '%' + 'Foo'/* Context.Builder.ToParameter(nameof(Context.Model.Name), Context.Model.Name); */ + '%'
-- }
/* Context.Builder.CustomExtension("Hello"); */