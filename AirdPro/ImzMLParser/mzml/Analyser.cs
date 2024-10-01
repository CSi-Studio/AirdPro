using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.ImzMLParser.mzml
{
    [Serializable]    
    public class Analyser : Component
    {
        /**
         * 序列化版本ID。
         */
        private const long serialVersionUID = 1L;

        /**
         * 访问号: 质谱分析器类型 (MS:1000443)。必须提供（或任何子元素）一次。
         */
        public static readonly string ANALYSER_TYPE_ID = "MS:1000443";

        /**
         * 访问号: 质谱分析器属性 (MS:1000480)。可以提供子元素一次或多次。
         */
        public static readonly string ANALYSER_ATTRIBUTE_ID = "MS:1000480";

        /**
         * 实例化一个新的分析器属性。
         */
        public Analyser()
        {
        }

        /**
         * 复制构造函数，需要新版本的列表以匹配旧的引用。
         * 
         * @param analyser 要复制的旧Analyser
         * @param rpgList 新的ReferenceableParamGroupList
         */
        public Analyser(Analyser analyser, ReferenceableParamGroupList rpgList) : base(analyser, rpgList)
        {
        }

        public override string ToString()
        {
            return "analyser";
        }

        public override string GetTagName()
        {
            return "analyzer";
        }
    }
}
