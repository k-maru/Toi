SELECT * FROM EMP
WHERE 1 = 1
--! this is line comment
-- if(!string.IsNullOrEmpty(Model.Name)) {
AND EMP.Name LIKE '%' + '--Foo' + '%'
/*? OR EMP.ShortName LIKE */

-- }
/*!
this is block comment
*/
