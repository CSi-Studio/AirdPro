using System.Collections.Generic;

namespace AirdPro.csimzMLParser.mzml
{
    public class Product : MzMLContent, IHasChildren
    {
        private static readonly long serialVersionUID = 1L;

        private IsolationWindow isolationWindow;

        public Product()
        {

        }

        public Product(Product product, ReferenceableParamGroupList rpgList)
        {
            if (product.isolationWindow != null)
            {
                isolationWindow = new IsolationWindow(product.isolationWindow, rpgList);
            }
        }

        public void SetIsolationWindow(IsolationWindow isolationWindow)
        {
            this.isolationWindow.SetParent(this);

            this.isolationWindow = isolationWindow;
        }

        public IsolationWindow getIsolationWindow()
        {
            return isolationWindow;
        }

        public override string GetTagName()
        {
            return "product";
        }

        public void AddChildrenToCollection(ICollection<IMzMLTag> children)
        {            
            children.Add(isolationWindow);
        }
    }
}
