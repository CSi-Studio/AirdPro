namespace AirdPro.ImzMLParser.mzml
{
    public class DataProcessingList : MzMLIDContentList<DataProcessing>
    {
        private const long serialVersionUID = 1L;

        public DataProcessingList(int count) : base(count)
        {
        }

        public DataProcessingList(DataProcessingList dpList, ReferenceableParamGroupList rpgList, SoftwareList softwareList)
            : base(dpList.Size())
        {
            foreach (DataProcessing dp in dpList)
            {
                this.Add(new DataProcessing(dp, rpgList, softwareList));
            }
        }

        public void AddDataProcessing(DataProcessing dataProcessing)
        {
            Add(dataProcessing);
        }

        public DataProcessing GetDataProcessing(int index)
        {
            return Get(index);
        }

        public DataProcessing GetDataProcessing(string id)
        {
            return Get(id);
        }

        public override string GetTagName()
        {
            return "dataProcessingList";
        }

        public static DataProcessingList Create()
        {
            return Create(Software.Create());
        }

        public static DataProcessingList Create(Software software)
        {
            DataProcessingList dpList = new(1)
            {
                DataProcessing.Create(software)
            };
            return dpList;
        }
    }
}
