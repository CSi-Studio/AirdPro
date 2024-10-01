using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.ImzMLParser.mzml
{
    public class ReferenceableParamGroupRef : MzMLReference<ReferenceableParamGroup>
    {
        // 序列化版本ID。
        private static readonly long serialVersionUID = 1L;

        // 从ReferenceableParamGroup创建<referenceableParamGroupRef>标签。
        public ReferenceableParamGroupRef(ReferenceableParamGroup refGroup)
            : base(refGroup)
        {
        }

        public override string GetTagName()
        {
            return "referenceableParamGroupRef";
        }
    }
}
