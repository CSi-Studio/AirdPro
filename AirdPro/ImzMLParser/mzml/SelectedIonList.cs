using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.ImzMLParser.mzml
{
    public class SelectedIonList : MzMLContentList<SelectedIon>
    {
        private static readonly long serialVersionUID = 1L;

        public SelectedIonList(int count) : base(count)
        {
            
        }

        public SelectedIonList(SelectedIonList siList, ReferenceableParamGroupList rpgList) : this(siList.Size())
        {
            foreach (SelectedIon si in siList)
            {
                this.Add(new SelectedIon(si, rpgList));
            }
        }

        public void AddSelectedIon(SelectedIon selectedIon)
        {
            Add(selectedIon);
        }

        public SelectedIon GetSelectedIon(int index)
        {
            return Get(index);
        }

        public  override string GetTagName()
        {
            return "selectedIonList";
        }
    }
}
