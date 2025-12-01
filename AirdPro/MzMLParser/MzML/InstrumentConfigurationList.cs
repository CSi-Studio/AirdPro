using AirdPro.csimzMLParser.obo;

namespace AirdPro.csimzMLParser.mzml
{
    public class InstrumentConfigurationList : MzMLIDContentList<InstrumentConfiguration>
    {
        private const long serialVersionUID = 1L;

        public InstrumentConfigurationList(int count) : base(count)
        {
        }

        public InstrumentConfigurationList(InstrumentConfigurationList icList, ReferenceableParamGroupList rpgList, ScanSettingsList ssList, SoftwareList softwareList) : this(icList.Size())
        {
            foreach (InstrumentConfiguration ic in icList)
            {
                Add(new InstrumentConfiguration(ic, rpgList, ssList, softwareList));
            }
        }

        public void AddInstrumentConfiguration(InstrumentConfiguration ic)
        {
            Add(ic);
        }

        public InstrumentConfiguration GetInstrumentConfiguration(int index)
        {
            return Get(index);
        }

        public InstrumentConfiguration GetInstrumentConfiguration(string id)
        {
            return Get(id);
        }

        public override string GetTagName()
        {
            return "instrumentConfigurationList";
        }

        public static InstrumentConfigurationList Create()
        {
            InstrumentConfigurationList icList = new(1);

            InstrumentConfiguration ic = new("instrumentConfiguration");
            icList.Add(ic);

            ic.AddCVParam(new EmptyCVParam(OBO.GetOBO().GetTerm(InstrumentConfiguration.INSTRUMENT_MODEL_ID)));

            return icList;
        }
    }
}
