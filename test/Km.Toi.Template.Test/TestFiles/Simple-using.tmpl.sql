--i Km.Toi.Template.Test
SELECT * FROM EMP
WHERE 1 = 1
-- if(!string.IsNullOrEmpty(Model.Name)) {
AND EMP.Name LIKE '%' + 'Foo'/* Builder.ToParameter(nameof(Model.Name), Model.Name); */ + '%'
-- }
/* Builder.CustomExtension("Hello"); */