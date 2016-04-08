SELECT * FROM EMP
WHERE 1 = 1
/* if(!string.IsNullOrEmpty(Model.Name) &&
Model.Name == "/*") { // */
*/
AND EMP.Name LIKE '%' + 'Foo'/*
SqlParameter(
nameof(Model.Name), 
Model.Name);
*/ + '%'
-- }
