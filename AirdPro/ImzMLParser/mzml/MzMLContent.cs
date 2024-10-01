using AirdPro.ImzMLParser.affair;
using AirdPro.ImzMLParser.exceptions;
using AirdPro.ImzMLParser.util;
using HZH_Controls;
using System;
using System.Collections.Generic;

namespace AirdPro.ImzMLParser.mzml
{
    public static class StringExtensions
    {
        public static string ReplaceFirst(this string source, string search, string replacement)
        {
            int pos = source.IndexOf(search);
            if (pos < 0)
            {
                return source;
            }
            return source.Substring(0, pos) + replacement + source.Substring(pos + search.Length);
        }
    }

    [Serializable]  // 标记类为可序列化
    public abstract class MzMLContent : IMzMLTag
    {
        private const long serialVersionUID = 1L;

        public IMzMLTag parent;
        private List<IMzMLContentListener> listeners;

        public void AddListener(IMzMLContentListener listener)
        {
            if (listeners == null) 
            {
                listeners = [];
            }
            listeners.Add(listener);
        }

        public void RemoveListener(IMzMLContentListener listener)
        {
            if (listeners != null)
            {
                listeners.Remove(listener);
            }
        }

        public void RemoveAllListeners()
        {
            if (listeners != null)
            {
                listeners.Clear();
            }
        }

        public List<IMzMLContentListener> GetListeners()
        {
            return listeners;
        }

        public void NotifyListeners(MzMLAffair affair)
        {
            if (listeners != null)
            {
                foreach (IMzMLContentListener listener in listeners)
                {
                    listener.AffairOccured(affair);
                }
            }

            if (affair.NotifyParents() && parent is MzMLContent && ((MzMLContent)parent).HasListeners())
            {
                ((MzMLContent)parent).NotifyListeners(affair);
            }
        }

        public bool HasListeners()
        {
            bool hasListeners = listeners != null && !listeners.IsEmpty();
            if (parent is MzMLContent)
            {
                hasListeners |= ((MzMLContent)parent).HasListeners();
            }
            return hasListeners;
        }

        public void SetParent(IMzMLTag parent)
        {
            this.parent = parent;
        }

        public IMzMLTag GetParent()
        {
            return parent;
        }

        public string GetXPath()
        {
            string xPath = "";
            if (parent != null)
                xPath = parent.GetXPath();

            return xPath + "/" + GetTagName();
        }

        public virtual void AddTagSpecificElementsAtXPathToCollection(ICollection<IMzMLTag> elements, string fullXPath, string currentXPath)
        {
            
        }

        public virtual void AddElementsAtXPathToCollection(ICollection<IMzMLTag> elements, string xPath)
        {
            AddElementsAtXPathToCollection(elements, xPath, xPath);
            throw new NotImplementedException();
        }



        public virtual void AddElementsAtXPathToCollection(ICollection<IMzMLTag> elements, string fullXPath, string currentXPath)
        {
            if (currentXPath.StartsWith("/" + GetTagName()))
            {
                string subXPath = currentXPath.ReplaceFirst("/" + GetTagName(), "");
                if(subXPath.IsEmpty())
                {
                    elements.Add(this);
                    return;
                }

                AddTagSpecificElementsAtXPathToCollection(elements, fullXPath, subXPath);

                if (elements.IsEmpty())
                {
                    throw new InvalidXPathException("Invalid sub-XPath (" + subXPath + ") in XPath " + fullXPath, fullXPath);
                }                    
            }
            else
            {
                throw new Exception("XPath does not start with /" + GetTagName() + " in sub-XPath [" + currentXPath + "] of [" + fullXPath + "]");
            }
        }

        public virtual string GetXMLAttributeText()
        {
            if (this is IReferenceableTag)
            {
                return "id=\"" + XMLHelper.EnsureSafeXML(((IReferenceableTag)this).GetID()) + "\"";
            }

            return "";
        }

        public override string ToString()
        {
            string attributeText = GetXMLAttributeText();
            if (!string.IsNullOrEmpty(attributeText))
            {
                return GetTagName() + ": " + attributeText;
            }      
            return GetTagName();
        }       

        public virtual void EnsureValidReferences()
        {
            
        }

        public virtual string GetTagName()
        {
            throw new NotImplementedException();
        }
    }
}
