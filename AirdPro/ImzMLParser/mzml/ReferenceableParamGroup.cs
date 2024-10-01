using AirdPro.ImzMLParser.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.ImzMLParser.mzml
{
    [Serializable]
    public class ReferenceableParamGroup : MzMLContentWithParams, IReferenceableTag
    {
        // 序列化版本ID。
        private static readonly long serialVersionUID = 1L;

        // 用于确保生成唯一ID的静态整数。
        private static int idNumber = 0;

        // 参数组的唯一标识符。
        private string id; // 必需的

        // 使用唯一ID创建一个空的ReferenceableParamGroup，ID形式为 'refParam#'，#是每次调用此构造函数时递增的整数值。
        public ReferenceableParamGroup()
        {
            id = "refParam" + idNumber++;
        }

        // 使用指定的唯一ID创建一个空的ReferenceableParamGroup。
        public ReferenceableParamGroup(string id)
        {
            if (id == null)
                throw new ArgumentException("ID cannot be null for ReferenceableParamGroup.");

            this.id = id;
        }

        public ReferenceableParamGroup(ReferenceableParamGroup rpg) : base(rpg, null)
        {
            id = rpg.id;
        }

        public string GetID()
        {
            return id;
        }

        public void SetID(string id)
        {
            this.id = id;
        }

        public override string GetXMLAttributeText()
        {
            return "id=\"" + XMLHelper.EnsureSafeXML(this.GetID()) + "\"";
        }

        public override string ToString()
        {
            return "referenceableParamGroup: " + id;
        }

        public override string GetTagName()
        {
            return "referenceableParamGroup";
        }
    }
}
