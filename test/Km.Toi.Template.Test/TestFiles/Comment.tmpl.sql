SELECT * FROM EMP
WHERE 1 = 1
--! this is line comment
-- if(!string.IsNullOrEmpty(Model.Name)) {
AND EMP.Name LIKE '%' + '--Foo' + '%'
/*!
this is block comment
*/
AND EMP.Address = 'COMMENT /* IN */ AND ''--ESCAPE--''--'
-- }
