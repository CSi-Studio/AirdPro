using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.ImzMLParser.mzml
{
    public class SoftwareList : MzMLIDContentList<Software>
    {
        private static readonly long serialVersionUID = 1L;

        public SoftwareList(int count) : base(count)
        {
        }

        public SoftwareList(SoftwareList softwareList, ReferenceableParamGroupList rpgList) : this(softwareList.Size())
        {   
            foreach (Software software in softwareList)
            {
                Add(new Software(software, rpgList));
            }
        }

        public void AddSoftware(Software software)
        {
            this.Add(software);
        }

        public Software GetSoftware(int index)
        {            
            return Get(index);
        }

        public Software GetSoftware(string id)
        {
            return Get(id);
        }

        public Software RemoveSoftware(int index)
        {            
            return Remove(index);
        }

        public override string GetTagName()
        {
            return "softwareList";
        }

        public static SoftwareList Create()
        {
            SoftwareList softwareList = new(1)
            {
                Software.Create()
            };
            return softwareList;
        }
    }
}
