using AirdPro.csimzMLParser.imzml;
using AirdPro.csimzMLParser.mzml;
using log4net;
using System;

namespace AirdPro.Domains.Msi
{
    public class ImagingParameters
    {
        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(ImagingParameters));

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
            MaxNumberOfPixelX = imz.GetWidth();
            MaxNumberOfPixelY = imz.GetHeight();
            MaxNumberOfPixelZ = imz.GetDepth();
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

                    p = scanSettings.GetCVParam(ScanSettings.LINE_SCAN_DIRECTION_BOTTOM_UP_ID);
                    if (p != null)
                    {
                        VStart = VerticalStart.BOTTOM;
                    }
                    else
                    {
                        VStart = VerticalStart.TOP;
                    }

                    p = scanSettings.GetCVParam(ScanSettings.LINE_SCAN_DIRECTION_RIGHT_LEFT_ID);
                    if (p != null)
                    {
                        HStart = HorizontalStart.RIGHT;
                    }
                    else
                    {
                        HStart = HorizontalStart.LEFT;
                    }

                    p = scanSettings.GetCVParam(ScanSettings.PIXEL_AREA_ID);
                    if (p != null)
                    {
                        PixelWidth = p.GetValueAsDouble();
                    }
                    PixelHeight = PixelWidth;

                    p = scanSettings.GetCVParam(ScanSettings.SCAN_PATTERN_MEANDERING_ID);
                    if (p != null)
                    {
                        Pattern = Pattern.MEANDER;
                    }
                    p = scanSettings.GetCVParam(ScanSettings.SCAN_PATTERN_FLYBACK_ID);
                    if (p != null)
                    {
                        Pattern = Pattern.FLY_BACK;
                    }
                    p = scanSettings.GetCVParam(ScanSettings.SCAN_PATTERN_RANDOM_ACCESS_ID);
                    if (p != null)
                    {
                        Pattern = Pattern.RANDOM;
                    }

                    p = scanSettings.GetCVParam(ScanSettings.SCAN_TYPE_VERTICAL_ID);
                    if (p != null)
                    {
                        ScanDirection = ScanDirection.VERTICAL;
                    }
                    else
                    {
                        ScanDirection = ScanDirection.HORIZONTAL;
                    }
                }

                double tolerance = 1e-10; // 阈值
                if (Math.Abs(LateralHeight - 0d) < tolerance)
                {
                    LateralHeight = MaxNumberOfPixelY * PixelHeight;
                }
                if (Math.Abs(LateralWidth - 0d) < tolerance)
                {
                    LateralWidth = MaxNumberOfPixelX * PixelWidth;
                }
            }
        }

        public override int GetHashCode()
        {
            const int prime = 31;
            int result = 1;
            result = prime * result + ((HStart == null) ? 0 : HStart.GetHashCode());
            long temp;
            temp = BitConverter.DoubleToInt64Bits(LateralHeight);
            result = prime * result + (int)(temp ^ (temp >>> 32));
            temp = BitConverter.DoubleToInt64Bits(LateralWidth);
            result = prime * result + (int)(temp ^ (temp >>> 32));
            temp = BitConverter.DoubleToInt64Bits(MaxMZ);
            result = prime * result + (int)(temp ^ (temp >>> 32));
            result = prime * result + MaxNumberOfPixelX;
            result = prime * result + MaxNumberOfPixelY;
            result = prime * result + MaxNumberOfPixelZ;
            temp = BitConverter.DoubleToInt64Bits(MinMZ);
            result = prime * result + (int)(temp ^ (temp >>> 32));
            result = prime * result + ((Pattern == null) ? 0 : Pattern.GetHashCode());
            temp = BitConverter.DoubleToInt64Bits(PixelHeight);
            result = prime * result + (int)(temp ^ (temp >>> 32));
            temp = BitConverter.DoubleToInt64Bits(PixelWidth);
            result = prime * result + (int)(temp ^ (temp >>> 32));
            result = prime * result + ((ScanDirection == null) ? 0 : ScanDirection.GetHashCode());
            result = prime * result + SpectraPerPixel;
            result = prime * result + ((VStart == null) ? 0 : VStart.GetHashCode());
            return result;
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null) return false;
            if (GetType() != obj.GetType()) return false;

            ImagingParameters other = (ImagingParameters)obj;

            if (HStart != other.HStart) return false;
            if (BitConverter.DoubleToInt64Bits(LateralHeight) != BitConverter.DoubleToInt64Bits(other.LateralHeight)) return false;
            if (BitConverter.DoubleToInt64Bits(LateralWidth) != BitConverter.DoubleToInt64Bits(other.LateralWidth)) return false;
            if (BitConverter.DoubleToInt64Bits(MaxMZ) != BitConverter.DoubleToInt64Bits(other.MaxMZ)) return false;
            if (MaxNumberOfPixelX != other.MaxNumberOfPixelX) return false;
            if (MaxNumberOfPixelY != other.MaxNumberOfPixelY) return false;
            if (MaxNumberOfPixelZ != other.MaxNumberOfPixelZ) return false;
            if (BitConverter.DoubleToInt64Bits(MinMZ) != BitConverter.DoubleToInt64Bits(other.MinMZ)) return false;
            if (Pattern != other.Pattern) return false;
            if (BitConverter.DoubleToInt64Bits(PixelHeight) != BitConverter.DoubleToInt64Bits(other.PixelHeight)) return false;
            if (BitConverter.DoubleToInt64Bits(PixelWidth) != BitConverter.DoubleToInt64Bits(other.PixelWidth)) return false;
            if (ScanDirection != other.ScanDirection) return false;
            if (SpectraPerPixel != other.SpectraPerPixel) return false;
            if (VStart != other.VStart) return false;

            return true;
        }

    }
}
