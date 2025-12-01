using AirdPro.csimzMLParser.util;
using System;

namespace AirdPro.csimzMLParser.mzml
{
    [Serializable]
    public abstract class MzMLReference<T> : MzMLContent where T : IMzMLTag, IReferenceableTag
    {
        // 引用的MzML标签
        private readonly T reference;

        // 创建对特定MzML标签的引用
        protected MzMLReference(T reference)
        {
            this.reference = reference;
        }

        // 返回被引用的标签
        public T GetReference()
        {
            return reference;
        }

        public override string GetXMLAttributeText()
        {
            return $"ref=\"{XMLHelper.EnsureSafeXML(reference.GetID())}\"";
        }

        public override string ToString()
        {
            return $"{GetTagName()}: {reference.GetID()}";
        }

    }
}
