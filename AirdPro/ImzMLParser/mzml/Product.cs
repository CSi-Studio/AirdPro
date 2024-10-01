using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.ImzMLParser.mzml
{
    public class Product : MzMLContent, IHasChildren
    {
        private static readonly long serialVersionUID = 1L;

        private IsolationWindow IsolationWindow;

        public Product()
        {

        }

        public Product(Product product, ReferenceableParamGroupList rpgList)
        {
            if (product.IsolationWindow != null)
            {
                IsolationWindow = new IsolationWindow(product.IsolationWindow, rpgList);
            }
        }

        public void SetIsolationWindow(IsolationWindow isolationWindow)
        {
            IsolationWindow.SetParent(this);

            this.IsolationWindow = isolationWindow;
        }

        public IsolationWindow getIsolationWindow()
        {
            return IsolationWindow;
        }

        public override string GetTagName()
        {
            return "product";
        }

        public void AddChildrenToCollection(ICollection<IMzMLTag> children)
        {            
            children.Add(IsolationWindow);
        }
    }
}
