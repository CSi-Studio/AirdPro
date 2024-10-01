namespace AirdPro.ImzMLParser.mzml
{
    public class ProductList : MzMLContentList<Product>
    {
        private static readonly long serialVersionUID = 1L;

        public ProductList(int count) : base(count)
        {
            
        }

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
