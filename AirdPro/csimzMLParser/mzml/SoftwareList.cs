namespace AirdPro.csimzMLParser.mzml
{
    public class SoftwareList(int count) : MzMLIDContentList<Software>(count)
    {
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
