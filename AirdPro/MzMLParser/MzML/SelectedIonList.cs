namespace AirdPro.csimzMLParser.mzml
{
    public class SelectedIonList(int count) : MzMLContentList<SelectedIon>(count)
    {
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
