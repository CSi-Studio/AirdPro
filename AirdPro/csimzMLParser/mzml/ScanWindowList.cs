namespace AirdPro.csimzMLParser.mzml
{
    public class ScanWindowList : MzMLContentList<ScanWindow>
    {
        private static readonly long serialVersionUID = 1L;

        public ScanWindowList(int count) : base(count)
        {

        }

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
