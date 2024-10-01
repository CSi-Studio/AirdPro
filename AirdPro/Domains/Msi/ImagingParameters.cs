using AirdPro.ImzMLParser.imzml;
using AirdPro.ImzMLParser.mzml;

namespace AirdPro.Domains.Msi
{
    public class ImagingParameters
    { 
        public double MinMZ { get; set; }
        public double MaxMZ { get; set; }
        public double LateralWidth { get; set; }
        public double LateralHeight { get; set; }
        public double PixelWidth { get; set; } = 1;
        public double PixelHeight { get; set; } = 1;
        public int MaxNumberOfPixelX { get; set; }
        public int MaxNumberOfPixelY { get; set; }
        public int MaxNumberOfPixelZ { get; set; } = 1;
        public VerticalStart VStart { get; set; } = VerticalStart.TOP;
        public HorizontalStart HStart { get; set; } = HorizontalStart.LEFT;
        public int SpectraPerPixel { get; set; } = 1;
        public Pattern Pattern { get; set; } = Pattern.FLY_BACK;
        public ScanDirection ScanDirection { get; set; } = ScanDirection.HORIZONTAL;

        public ImagingParameters(ImzML imz)
        {
            MaxNumberOfPixelX = imz.width;
            MaxNumberOfPixelY = imz.height;
            MaxNumberOfPixelZ = imz.depth;
            SpectraPerPixel = imz.GetNumberOfSpectraPerPixel();
            MinMZ = imz.GetMinimumDetectedmz();
            MaxMZ = imz.GetMaximumDetectedmz();

            // Check scan settings first
            ScanSettingsList scanSettingsList = imz.GetScanSettingsList();

            if (scanSettingsList != null)
            {
                foreach (ScanSettings scanSettings in scanSettingsList)
                {
                    CVParam p = scanSettings.GetCVParam(ScanSettings.MAX_DIMENSION_X_ID);
                    if (p != null)
                    {
                        LateralWidth = p.GetValueAsDouble();
                    }
                    p = scanSettings.GetCVParam(ScanSettings.MAX_DIMENSION_Y_ID);
                    if (p != null)
                    {
                        LateralHeight = p.GetValueAsDouble();
                    }

                    // ... (Other settings)
                }

                if (LateralHeight == 0)
                {
                    LateralHeight = MaxNumberOfPixelY * PixelHeight;
                }
                if (LateralWidth == 0)
                {
                    LateralWidth = MaxNumberOfPixelX * PixelWidth;
                }
            }
        }       

    }


}
