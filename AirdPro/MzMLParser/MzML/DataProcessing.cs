using AirdPro.csimzMLParser.obo;
using AirdPro.csimzMLParser.util;

namespace AirdPro.csimzMLParser.mzml
{
    public class DataProcessing : MzMLContentList<ProcessingMethod>, IReferenceableTag
    {
        private const long serialVersionUID = 1L;
        protected string id;

        public DataProcessing(string id) : base(0)
        {
            this.id = id;
        }

        public DataProcessing(DataProcessing dp, ReferenceableParamGroupList rpgList, SoftwareList softwareList) : this(dp.id)
        {
            foreach (ProcessingMethod pm in dp)
            {
                Add(new ProcessingMethod(pm, rpgList, softwareList));
            }
        }

        public void AddProcessingMethod(ProcessingMethod preprocessingMethod)
        {
            Add(preprocessingMethod);
        }

        public ProcessingMethod GetProcessingMethod(int index)
        {
            return Get(index);
        }

        public int GetProcessingMethodCount()
        {
            return Size();
        }

        public override string ToString()
        {
            return $"dataProcessing: {id}";
        }

        public virtual string GetID()
        {
            return id;
        }

        public virtual void SetID(string id)
        {
            this.id = id;
        }

        public virtual string ToXMLAttributeText()
        {
            return $"id=\"{XMLHelper.EnsureSafeXML(id)}\"";
        }

        public override string GetTagName()
        {
            return "dataProcessing";
        }

        public static DataProcessing Create()
        {
            return Create(Software.Create());
        }

        public static DataProcessing Create(Software software)
        {
            DataProcessing dp = new("imzML-creation");

            ProcessingMethod pm = new(software);
            dp.Add(pm);

            pm.AddCVParam(new EmptyCVParam(OBO.GetOBO().GetTerm(ProcessingMethod.CONVERSION_TO_MZML_ID)));

            return dp;
        }
    }
}
