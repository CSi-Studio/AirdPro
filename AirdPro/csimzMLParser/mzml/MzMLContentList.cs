using AirdPro.csimzMLParser.exceptions;
using HZH_Controls;
using System.Collections;
using System.Collections.Generic;

namespace AirdPro.csimzMLParser.mzml
{
    public abstract class MzMLContentList<T> : MzMLContent, IMzMLTagList<T>, IHasChildren
    where T : IMzMLTag
    {
        private List<T> list = [];

        public MzMLContentList() 
        {

        }

        public MzMLContentList(int count) 
        {
           
        }

        public MzMLContentList(MzMLContentList<T> contentList) : this(contentList.Size())
        {
            foreach (T item in contentList)
            {
                Add(item);
            }
        }

        protected List<T> GetList()
        {
            return list; 
        }

        public virtual void Add(T item)
        {
            if (item is MzMLContent)
            {
                item.SetParent(this);   
            }

            if (list.Count > 1)
            {
                list.Add(item);
            }
            else if (list.Count == 1)
            {
                list = new List<T>(list)
                {
                    item
                };
            }
            else
            {
                list = [item];
            }            
        }

        public virtual T Get(int index)
        {
            return list[index];
        }

        public virtual T Remove(int index)
        {
            if (list == null)
            {
                return default;
            }
            T item = list[index];     
            list.RemoveAt(index);
            return item;
        }

        public virtual bool Remove(T item)
        {
            if (list == null)
            {
                return false;
            } 
            return list.Remove(item);
        }        

        public virtual int IndexOf(T item)
        {
            return list.IndexOf(item);
        }

        public virtual int Size()
        {
            return list.Count;
        }

        public virtual void AddChildrenToCollection(ICollection<IMzMLTag> children)
        {
            foreach (var item in list)
            {
                children.Add(item); 
            }
        }

        public override void AddTagSpecificElementsAtXPathToCollection(ICollection<IMzMLTag> elements, string fullXPath, string currentXPath)
        {
            if (!fullXPath.Equals(currentXPath) && list.IsEmpty())
            {
                throw new UnfollowableXPathException("No " + GetTagName() + "exisits, so cannot go to " + fullXPath, fullXPath, currentXPath);
            }

            if (Size() > 0)
            {
                T firstElement = list[0];
                if (currentXPath.StartsWith("/" + firstElement.GetTagName()))
                {
                    foreach (T item in list)
                    {
                        item.AddElementsAtXPathToCollection(elements, fullXPath, currentXPath);
                    }
                }
            }
        }

        public override string GetXMLAttributeText()
        {
            string attributeText = base.GetXMLAttributeText();
            if (!attributeText.IsEmpty())
            {
                attributeText += " ";
            }
            return attributeText + $"count=\"{Size()}\"";
        }

        public override string ToString()
        {
            return GetTagName();
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }   

        public bool Contains(T item)
        {
            return list.Contains(item);
        }

        public void Clear()
        {
            list.Clear();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}
