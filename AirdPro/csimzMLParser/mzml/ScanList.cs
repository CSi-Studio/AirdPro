using AirdPro.csimzMLParser.obo;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AirdPro.csimzMLParser.mzml
{
    public class ScanList : MzMLContentWithParams, IMzMLTagList<Scan>
    {
        private static readonly long serialVersionUID = 1L;

        public static readonly string SPECTRA_COMBINATION_ID = "MS:1000570";
        public static readonly string NO_COMBINATION_ID = "MS:1000795";

        private List<Scan> list = [];

        public ScanList(int count)
        {
            
        }

        public ScanList(ScanList scanList, ReferenceableParamGroupList rpgList, SourceFileList sourceFileList, InstrumentConfigurationList icList)
            : base(scanList, rpgList)
        {
            this.list = new List<Scan> (scanList.Size());
            foreach (Scan scan in scanList)
            {
                this.list.Add(new Scan(scan, rpgList, icList, sourceFileList));
            }
        }

        public void Add(Scan scan)
        {
            scan.SetParent(this);

            if (list.Count > 1)
            {
                list.Add(scan);
            }
            else if (list.Count == 1)
            {
                var tempList = new List<Scan> { list[0] };
                list = tempList;
                list.Add(scan);
            }
            else
            {
                list = new List<Scan> { scan };
            }
        }

        public void AddScan(Scan scan)
        {
            Add(scan);
        }

        public Scan Get(int index)
        {
            return list[index];
        }

        public Scan GetScan(int index)
        {
            return Get(index);
        }

        public int Size()
        {
            return list.Count;
        }

        public override void AddTagSpecificElementsAtXPathToCollection(ICollection<IMzMLTag> elements, string fullXPath, string currentXPath)
        {            
            if (currentXPath.StartsWith("/scan"))
            {
                if (list.Count == 0)
                {
                    throw new InvalidOperationException("No scanList exists, so cannot go to " + fullXPath);
                }

                foreach (var scan in list)
                {
                    scan.AddElementsAtXPathToCollection(elements, fullXPath, currentXPath);
                }
            }
        }

        public override string GetXMLAttributeText()
        {
            return $"count=\"{Size()}\"";
        }

        public IEnumerator<Scan> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string GetTagName()
        {
            return "scanList";
        }

        public override void AddChildrenToCollection(ICollection<IMzMLTag> children)
        {
            base.AddChildrenToCollection(children);

            foreach (var scan in list)
            {
                children.Add(scan);
            }
        }

        public int IndexOf(Scan item)
        {
            return list.IndexOf(item);
        }

        public Scan Remove(int index)
        {
            var item = list[index];
            list.RemoveAt(index);
            return item;
        }

        public bool Remove(Scan item)
        {
            if (list.Remove(item))
            {
                item.SetParent(null);
                return true;
            }
            return false;
        }

        public static ScanList Create()
        {
            var scanList = new ScanList(1);
            scanList.Add(Scan.Create());

            scanList.AddCVParam(new EmptyCVParam(OBO.GetOBO().GetTerm(NO_COMBINATION_ID)));

            return scanList;
        }

        public bool Contains(Scan item)
        {
            return list.Contains(item);
        }

        public void Clear()
        {
            list.Clear();
        }
    }
}
