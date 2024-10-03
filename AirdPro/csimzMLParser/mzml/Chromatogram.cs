using AirdPro.csimzMLParser.exceptions;
using System;
using System.Collections.Generic;

namespace AirdPro.csimzMLParser.mzml
{
    public class Chromatogram : MzMLDataContainer
    {
        private static readonly long serialVersionUID = 1L;

        public static readonly string CHROMATOGRAM_ATTRIBUTE_ID = "MS:1000808";
        public static readonly string CHROMATOGRAM_TYPE_ID = "MS:1000626";

        public Precursor precursor;
        public Product product;
        public Chromatogram(String id, int defaultArrayLength) : base(id, defaultArrayLength)
        {

        }

        public Chromatogram(Chromatogram chromatogram, ReferenceableParamGroupList rpgList, DataProcessingList dpList,
                SourceFileList sourceFileList) : base(chromatogram, rpgList, dpList)
        {
            this.id = chromatogram.id;

            if (chromatogram.precursor != null)
            {
                this.precursor = new Precursor(chromatogram.precursor, rpgList, sourceFileList);
            }
            if (chromatogram.product != null)
            {
                this.product = new Product(chromatogram.product, rpgList);
            }
        }

        public void SetPrecursor(Precursor precursor)
        {
            precursor.SetParent(this);
            this.precursor = precursor;
        }

        public Precursor GetPrecursor()
        {
            return precursor;
        }

        public void SetProduct(Product product)
        {
            product.SetParent(this);
            this.product = product;
        }

        public Product GetProduct()
        {
            return product;
        }

        public override void AddTagSpecificElementsAtXPathToCollection(ICollection<IMzMLTag> elements, String fullXPath, String currentXPath)
        {
            if (currentXPath.StartsWith("/precursor"))
            {
                if (precursor == null)
                {
                    throw new UnfollowableXPathException("No precursor exists, so cannot go to " + fullXPath, fullXPath, currentXPath);
                }

                precursor.AddElementsAtXPathToCollection(elements, fullXPath, currentXPath);

            }
            else if (currentXPath.StartsWith("/product"))
            {
                if (product == null)
                {
                    throw new UnfollowableXPathException("No product exists, so cannot go to " + fullXPath, fullXPath, currentXPath);
                }
                product.AddElementsAtXPathToCollection(elements, fullXPath, currentXPath);
            }
            else if (currentXPath.StartsWith("/binaryDataArrayList"))
            {
                if (binaryDataArrayList == null)
                {
                    throw new UnfollowableXPathException("No binaryDataArrayList exists, so cannot go to " + fullXPath, fullXPath, currentXPath);
                }

                binaryDataArrayList.AddElementsAtXPathToCollection(elements, fullXPath, currentXPath);
            }
        }
        public override string ToString()
        {
            return "chromatogram: id=\"" + id + "\"";
        }
        public override string GetTagName()
        {
            return "chromatogram";
        }

        public override void AddChildrenToCollection(ICollection<IMzMLTag> children)
        {
            base.AddChildrenToCollection(children);

            if (precursor != null)
            {
                children.Add(precursor);
            }
            if (product != null)
            {
                children.Add(product);
            }
            if (binaryDataArrayList != null)
            {
                children.Add(binaryDataArrayList);
            }
        }
    }
}

