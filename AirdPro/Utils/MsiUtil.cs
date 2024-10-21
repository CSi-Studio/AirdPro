using AirdSDK.Bean;
using AirdSDK.Bean.Msi;
using AirdSDK.Enums;
using AirdSDK.Enums.Msi;
using pwiz.CLI.msdata;
using System;

namespace AirdPro.Utils
{
    public class MsiUtil
    {
        public static MsiInfo GetMsiInfo(MsiConfig msiConfig, MSData msd)
        {
            MsiInfo msiInfo = new();
            //fileOrganisation: row per file, image per file，spectrum per file
            msiInfo.fileOrganisation = GetFileOrganisation(msiConfig);
            //image info
            msiInfo.imageInfo = GetImageInfo(msiConfig);
            //spectrum position
            //msiInfo.spectraPosition = GetSpectraPosition(msd.run.spectrumList);
            //sample stage ???            
            //public ScanInfo scanInfo
            msiInfo.scanInfo = GetScanInfo(msiConfig);

            return msiInfo;
        }

        public static string GetFileOrganisation(MsiConfig msiConfig)
        {
            return msiConfig.fileOrganisation switch
            {
                FileOrganisation.SPECTRUM_PER_FILE => FileOrganisation.SPECTRUM_PER_FILE,
                FileOrganisation.IMAGE_PER_FILE => FileOrganisation.IMAGE_PER_FILE,
                FileOrganisation.ROW_PER_FILE => FileOrganisation.ROW_PER_FILE,
                _ => throw new ArgumentException("Invalid file organisation")
            };
        }

        public static ImageInfo GetImageInfo(MsiConfig msiConfig)
        {
            ImageInfo imageInfo = new ImageInfo();

            imageInfo.maxPixelX = msiConfig.maxPixelX;
            imageInfo.maxPixelY = msiConfig.maxPixelY;
            imageInfo.maxPixelZ = msiConfig.maxPixelZ;       

            return imageInfo;
        }

        public static ScanInfo GetScanInfo(MsiConfig msiConfig)
        {
            ScanInfo scanInfo = new ScanInfo();
            //scanDirection
            switch (msiConfig.scanDirection)
            {
                case ScanDirection.LINESCAN_TOP_DOWN:
                    scanInfo.scanDirection = ScanDirection.LINESCAN_TOP_DOWN;
                    break;
                case ScanDirection.LINESCAN_BOTTOM_UP:
                    scanInfo.scanDirection = ScanDirection.LINESCAN_BOTTOM_UP;
                    break;
                case ScanDirection.LINESCAN_LEFT_RIGHT:
                    scanInfo.scanDirection = ScanDirection.LINESCAN_LEFT_RIGHT;
                    break;
                case ScanDirection.LINESCAN_RIGHT_LEFT:
                    scanInfo.scanDirection = ScanDirection.LINESCAN_RIGHT_LEFT;
                    break;
            }
            //scan pattern
            switch (msiConfig.scanPattern)
            {
                case ScanPattern.MEANDERING:
                    scanInfo.scanPattern = ScanPattern.MEANDERING;
                    break;
                case ScanPattern.FLY_BACK:
                    scanInfo.scanPattern = ScanPattern.FLY_BACK;
                    break;
                case ScanPattern.RANDOM_ACCESS:
                    scanInfo.scanPattern = ScanPattern.RANDOM_ACCESS;
                    break;
            }
            //scan sequence
            switch (msiConfig.scanSequence)
            {
                case ScanSequence.TOP_DOWN:
                    scanInfo.scanSequence = ScanSequence.TOP_DOWN;
                    break;
                case ScanSequence.BOTTOM_UP:
                    scanInfo.scanSequence = ScanSequence.BOTTOM_UP;
                    break;
                case ScanSequence.LEFT_RIGHT:
                    scanInfo.scanSequence = ScanSequence.LEFT_RIGHT;
                    break;
                case ScanSequence.RIGHT_LEFT:
                    scanInfo.scanSequence = ScanSequence.RIGHT_LEFT;
                    break;
            }
            //scanType
            if(msiConfig.scanDirection == ScanDirection.LINESCAN_LEFT_RIGHT || msiConfig.scanDirection == ScanDirection.LINESCAN_RIGHT_LEFT)
            {
                scanInfo.scanType = ScanType.HORIZONTAL;
            }
            else
            {
                scanInfo.scanType = ScanType.VERTICAL;
            }            

            return scanInfo;
        }

        public static SpectraPosition GetSpectraPosition(SpectrumList spectrumList)
        {
            if (spectrumList == null || spectrumList.size() == 0)
            {
                return null;
            }
            int totalSpectrumCount = spectrumList.size();
            int[] x = new int[totalSpectrumCount];
            int[] y = new int[totalSpectrumCount];
            int[] z = new int[totalSpectrumCount];
            for (int i = 0; i < totalSpectrumCount; i++)
            {
               /* x[i] = spectrumList.spectrum(i).GetPixelLocation().GetX();
                y[i] = spectrumList.spectrum(i).GetPixelLocation().GetY();
                z[i] = spectrumList.spectrum(i).GetPixelLocation().GetZ();*/
            }
            return new SpectraPosition(x, y, z);
        }

    }        
}
