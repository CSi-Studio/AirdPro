namespace AirdSDK.Bean.Msi
{
    public class MsiInfo
    {
        public string fileOrganisation; //row per file, image per file，spectrum per file
        public IbdInfo ibdInfo;
        public ImageInfo imageInfo;
        public SpectraPosition spectraPosition;
        public SampleStage sampleStage;
        public ScanInfo scanInfo; 
    }
}
