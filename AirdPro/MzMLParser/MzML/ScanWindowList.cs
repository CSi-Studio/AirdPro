namespace AirdPro.csimzMLParser.mzml
{
    public class ScanWindowList(int count) : MzMLContentList<ScanWindow>(count)
    {
        public ScanWindowList(ScanWindowList scanWindowList, ReferenceableParamGroupList rpgList) : this(scanWindowList.Size())
        {
            foreach (ScanWindow scanWindow in scanWindowList)
            {
                this.Add(new ScanWindow(scanWindow, rpgList));
            }
        }

        public void AddScanWindow(ScanWindow scanWindow)
        {
            Add(scanWindow);
        }

        public ScanWindow GetScanWindow(int index)
        {
            return Get(index);
        }

        public override string GetTagName()
        {
            return "scanWindowList";
        }
    }
}
