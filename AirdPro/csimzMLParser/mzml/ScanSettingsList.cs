namespace AirdPro.csimzMLParser.mzml
{
    public class ScanSettingsList(int count) : MzMLIDContentList<ScanSettings>(count)
    {
        public ScanSettingsList(ScanSettingsList scanSettingsList, ReferenceableParamGroupList rpgList, SourceFileList sourceFileList) : this(scanSettingsList.Size())
        {
            foreach (ScanSettings scanSettings in scanSettingsList)
            {
                Add(new ScanSettings(scanSettings, rpgList, sourceFileList));
            }
        }

        public void AddScanSettings(ScanSettings scanSettings)
        {
            Add(scanSettings);
        }

        public ScanSettings GetScanSettings(int index)
        {
            return Get(index);
        }

        public ScanSettings GetScanSettings(string id)
        {
            return Get(id);
        }

        public override string GetTagName()
        {
            return "scanSettingsList";
        }
    }
}
