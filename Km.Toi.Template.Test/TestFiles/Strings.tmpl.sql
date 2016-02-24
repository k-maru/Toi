SELECT * FROM EMP
WHERE 1 = 1
-- if(!string.IsNullOrEmpty(Context.Model.Name)) {
AND EMP.Name LIKE '%' + '--Foo' + '%'
--
AND EMP.Address = 'COMMENT /* IN */ AND ''--ESCAPE--''--'
-- }
