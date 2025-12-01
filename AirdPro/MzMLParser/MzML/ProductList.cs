namespace AirdPro.csimzMLParser.mzml
{
    public class ProductList(int count) : MzMLContentList<Product>(count)
    {
        public ProductList(ProductList productList, ReferenceableParamGroupList rpgList) : this(productList.Size())
        {
            foreach (Product product in productList)
            {
                this.Add(new Product(product, rpgList));
            }
        }

        public void AddProduct(Product product)
        {
            Add(product);
        }

        public Product GetProduct(int index)
        {
            return Get(index);
        }

        public override string GetTagName()
        {
            return "productList";
        }

    }
}
