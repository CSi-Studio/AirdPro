using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AirdPro.ImzMLParser.mzml
{
    public interface IMzMLTag
    {
        // 获取在MzML文件中出现的标签名称
        string GetTagName();

        // 将匹配指定XPath的所有子MzMLContent（mzML标签）添加到指定的集合中
        void AddElementsAtXPathToCollection(ICollection<IMzMLTag> elements, string xPath);

        // 将匹配指定XPath的所有子MzMLContent（mzML标签）添加到指定的集合中
        // 这个方法的实现可能需要考虑当前节点的上下文
        void AddElementsAtXPathToCollection(ICollection<IMzMLTag> elements, string fullXPath, string currentXPath);

        // 返回描述MzMLTag的XML属性的XML格式文本
        string GetXMLAttributeText();

        // 设置此MzMLContent的父MzMLContent。这个方法目前不执行任何操作
        void SetParent(IMzMLTag parent);

        // 返回此MzMLTag的父MzMLTag，如果是顶级(I)mzML标签则返回null
        IMzMLTag GetParent();

        // 返回当前MzMLTag的XPath
        string GetXPath();
    }
}
