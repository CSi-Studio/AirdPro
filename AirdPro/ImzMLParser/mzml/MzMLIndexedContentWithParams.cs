using pwiz.CLI.msdata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.ImzMLParser.mzml
{
    public abstract class MzMLIndexedContentWithParams : MzMLOrderedContentWithParams, IReferenceableTag
    {
        public string id;
    
        public MzMLIndexedContentWithParams()
        {

        }

        public MzMLIndexedContentWithParams(MzMLIndexedContentWithParams mzMLContent, ReferenceableParamGroupList rpgList) : base(mzMLContent, rpgList)
        {
            
        }

        public virtual string GetID()
        {
            return id;
        }
       
        public virtual void SetID(string id)
        {
            this.id = id;
        }
    }
}
