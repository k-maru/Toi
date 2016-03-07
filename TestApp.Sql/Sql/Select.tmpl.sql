SELECT ProductID, ProductName, QuantityPerUnit
From Products
-- Builder.StartBlock("Categories");
INNER JOIN Categories ON Products.CategoryID = Categories.CategoryID
-- Builder.EndBlock();
-- Builder.StartBlock("Suppliers");
INNER JOIN Suppliers ON Products.SupplierID = Suppliers.SupplierID
-- Builder.EndBlock();
WHERE 1 = 1
-- if(!string.IsNullOrEmpty(Model.CategoryName))
-- {
AND Categories.CategoryName LIKE '%Co%'-- Builder.ToParameter("CategoryName", $"%{Model.CategoryName}%");
-- Builder.UseBlock("Categories");
-- }
-- if(!string.IsNullOrEmpty(Model.CategoryDesc))
-- {
AND Categories.Description LIKE '%d%'-- Builder.ToParameter("CategoryDesc", $"%{Model.CategoryDesc}%");
-- Builder.UseBlock("Categories");
-- }
-- if(!string.IsNullOrEmpty(Model.CompanyName))
-- {
AND Suppliers.CompanyName LIKE '%Ltd%'-- Builder.ToParameter("CategoryDesc", $"%{Model.CompanyName}%");
-- Builder.UseBlock("Suppliers");
-- }
-- if(Model.Countries.Any())
-- {
And Suppliers.Country IN ('UK', 'USA', 'JAPAN' /* Builder.ToInParameter("Country", Model.Countries); */)
-- Builder.UseBlock("Suppliers");
-- }
-- if(Model.FromPrice.HasValue || Model.ToPrice.HasValue) {
AND (
-- if(Model.FromPrice.HasValue) {
    Products.UnitPrice > 10/* Builder.ToParameter("FromPrice", Model.FromPrice.Value); */
-- }
    OR-- Builder.Text.RemovePrevIf(!Model.FromPrice.HasValue || !Model.ToPrice.HasValue);
-- if(Model.ToPrice.HasValue) {
    Products.UnitPrice < 40/* Builder.ToParameter("ToPrice", Model.ToPrice.Value); */
-- }
)
--}
