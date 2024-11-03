using AirdPro.csimzMLParser.imzml;
using AirdPro.csimzMLParser.mzml;
using AirdSDK.Bean.Msi;
using AirdSDK.Enums.Msi;
using SharpCompress.Common;
using System.IO;
using System.Linq;

namespace AirdPro.Utils.imzml
{
    public class MsiUtil
    {
        public static MsiInfo GetMsiInfo(ImzML imzML)
        {
            MsiInfo msiInfo = new();
            //fileOrganisation: row per file, image per file，spectrum per file
            msiInfo.fileOrganisation = FileOrganisation.IMAGE_PER_FILE;
            //ibd info
            msiInfo.ibdInfo = GetIbdInfo(imzML);
            //image info
            msiInfo.imageInfo = GetImageInfo(imzML);
            //spectrum position
            msiInfo.spectraPosition = GetSpectraPosition(imzML.GetSpectrumList());
            //sample stage
            msiInfo.sampleStage = GetSampleStage(imzML);
            //public ScanInfo scanInfo
            msiInfo.scanInfo = GetScanInfo(imzML);

            return msiInfo;
        }        

        public static IbdInfo GetIbdInfo(ImzML imzML)
        {
            IbdInfo ibdInfo = new();

            FileContent fileContent = imzML.GetFileDescription().GetFileContent();
            ibdInfo.fileUri = fileContent?.GetCVParam(FileContent.IBD_FILE_ID)?.ToString();  
            
            if (fileContent?.GetCVParam(FileContent.SHA1_CHECKSUM_ID) != null)
            {
                ibdInfo.checkSum = CheckSum.SHA1;               
            }
           else if (fileContent?.GetCVParam(FileContent.MD5_CHECKSUM_ID) != null)
            {
                ibdInfo.checkSum = CheckSum.MD5;
            }

            if (fileContent?.GetCVParam(FileContent.BINARY_TYPE_CONTINUOUS_ID) != null)
            {
                ibdInfo.binaryType = BinaryType.CONTINUOUS;
            }
            else if (fileContent?.GetCVParam(FileContent.BINARY_TYPE_PROCESSED_ID) != null)
            {
                ibdInfo.binaryType = BinaryType.PROCESSED;
            }


            ibdInfo.identification = fileContent?.GetCVParam(FileContent.IDB_IDENTIFICATION_ID)?.ToString();

            OffsetHandle offsetHandle = new OffsetHandle();
            SpectrumList spectrumList = imzML.GetSpectrumList();
            if (spectrumList == null || spectrumList.Size() == 0)
            {
                ibdInfo.offsetHandle = null;
                return ibdInfo;
            }
            offsetHandle.mzExternalArrayLength = new long[spectrumList.Size()];
            offsetHandle.mzExternalOffset = new long[spectrumList.Size()];
            offsetHandle.mzExternalEncodedLength = new long[spectrumList.Size()];
            offsetHandle.intensityExternalArrayLength = new long[spectrumList.Size()];
            offsetHandle.intensityExternalOffset = new long[spectrumList.Size()];
            offsetHandle.intensityExternalEncodedLength = new long[spectrumList.Size()];            
            for (int i = 0; i < spectrumList.Size(); i++)
            {
                BinaryDataArray mzArray = spectrumList.GetSpectrum(i).GetBinaryDataArrayList().GetBinaryDataArray(0);                
                offsetHandle.mzExternalArrayLength[i] = mzArray.GetExternalArrayLength();
                offsetHandle.mzExternalOffset[i] = mzArray.GetExternalOffset();
                offsetHandle.mzExternalEncodedLength[i] = mzArray.GetExternalEncodedLength();
                BinaryDataArray intensityArray = spectrumList.GetSpectrum(i).GetBinaryDataArrayList().GetBinaryDataArray(1);
                offsetHandle.intensityExternalArrayLength[i] = intensityArray.GetExternalArrayLength();
                offsetHandle.intensityExternalOffset[i] = intensityArray.GetExternalOffset();
                offsetHandle.intensityExternalEncodedLength[i] = intensityArray.GetExternalEncodedLength();
            }
            ibdInfo.offsetHandle = offsetHandle;

            return ibdInfo;
        }       

        public static ImageInfo GetImageInfo(ImzML imzML)
        {
            ImageInfo imageInfo = new()
            {
                maxPixelX = imzML.GetWidth(),
                maxPixelY = imzML.GetHeight(),
                maxPixelZ = imzML.GetDepth()
            };

            ScanSettings scanSettings = imzML.GetScanSettingsList().GetScanSettings(0);
            double pixelSizeX = scanSettings?.GetCVParam(ScanSettings.PIXEL_SIZE_X_ID)?.GetValueAsDouble() ?? 1;
            double pixelSizeY = scanSettings.GetCVParam(ScanSettings.PIXEL_SIZE_Y_ID)?.GetValueAsDouble() ?? pixelSizeX;

            imageInfo.pixelSizeX = pixelSizeX;
            imageInfo.pixelSizeY = pixelSizeY;    
            
            imageInfo.imageShape = scanSettings?.GetCVParam(ScanSettings.IMAGE_ID)?.ToString() ?? "no data";
            //mz range
            imageInfo.minMZ = DataUtil.GetMinMZ(imzML.GetSpectrumList());
            imageInfo.maxMZ = DataUtil.GetMaxMZ(imzML.GetSpectrumList());
            //spectra per pixel
            imageInfo.spectraPerPixel = imzML.GetNumberOfSpectraPerPixel();

            return imageInfo;
        }

        public static SampleStage GetSampleStage(ImzML imzML)
        {
            SampleStage sampleStage = new SampleStage();

            SampleList sampleList = imzML.GetSampleList();
            if(sampleList == null || sampleList.Count() == 0)
            {
                return null;
            }
            Sample sample = sampleList.GetSample(0);
            sampleStage.positionAccuracy = sample?.GetCVParam(Sample.POSITION_ACCURACY_ID)?.GetValueAsDouble() ?? -1;
            sampleStage.stepSize = sample?.GetCVParam(Sample.STEP_SIZE_ID)?.GetValueAsDouble() ?? -1;
            sampleStage.targetMaterial = sample?.GetCVParam(Sample.TARGET_MATERIAL_ID)?.ToString() ?? "no data";

            return sampleStage;
        }

        public static ScanInfo GetScanInfo(ImzML imzML)
        {
            ScanInfo scanInfo = new ScanInfo();
            ScanSettings scanSettings = imzML.GetScanSettingsList().GetScanSettings(0);

            //scan pattern
            if(scanSettings?.GetCVParam(ScanSettings.FLYBACK_ID) != null)
            {
                scanInfo.scanPattern = ScanPattern.FLY_BACK;
            }
            else if(scanSettings?.GetCVParam(ScanSettings.MEANDERING_ID) != null)
            {
                scanInfo.scanPattern = ScanPattern.MEANDERING;
            }
            else if(scanSettings?.GetCVParam(ScanSettings.RANDOM_ACCESS_ID) != null)
            {
				scanInfo.scanPattern = ScanPattern.RANDOM_ACCESS;
            }

            //scan sequence
            if (scanSettings?.GetCVParam(ScanSettings.LEFT_RIGHT_ID) != null)
            {
                scanInfo.scanSequence = ScanSequence.LEFT_RIGHT;
            }
            else if (scanSettings?.GetCVParam(ScanSettings.RIGHT_LEFT_ID) != null)
            {
                scanInfo.scanSequence = ScanSequence.RIGHT_LEFT;
            }
            else if (scanSettings?.GetCVParam(ScanSettings.TOP_DOWN_ID) != null)
            {
                scanInfo.scanSequence = ScanSequence.TOP_DOWN;
            }
            else if (scanSettings?.GetCVParam(ScanSettings.BOTTOM_UP_ID) != null)
            {
                scanInfo.scanSequence = ScanSequence.BOTTOM_UP;
            }

            //scanType
            if (scanSettings?.GetCVParam(ScanSettings.HORIZONTAL_LINESCAN_ID) != null)
            {
                scanInfo.scanType = ScanType.HORIZONTAL;
            }
            else if (scanSettings?.GetCVParam(ScanSettings.VERTICAL_LINESCAN_ID) != null)
            {
                scanInfo.scanType = ScanType.VERTICAL;
            }

            //scanDirection
            if (scanSettings?.GetCVParam(ScanSettings.LINESCAN_LEFT_RIGHT_ID) != null)
            {
                scanInfo.scanDirection = ScanDirection.LINESCAN_LEFT_RIGHT;
            }
            else if (scanSettings?.GetCVParam(ScanSettings.LINESCAN_RIGHT_LEFT_ID) != null)
            {
                scanInfo.scanDirection = ScanDirection.LINESCAN_RIGHT_LEFT;
            }
            else if(scanSettings?.GetCVParam(ScanSettings.LINESCAN_TOP_DOWN_ID) != null)
            {
                scanInfo.scanDirection = ScanDirection.LINESCAN_TOP_DOWN;
            }
            else if(scanSettings?.GetCVParam(ScanSettings.LINESCAN_BOTTOM_UP_ID) != null)
            {
                scanInfo.scanDirection = ScanDirection.LINESCAN_BOTTOM_UP;
            }

            return scanInfo;
        }

        public static SpectraPosition GetSpectraPosition(SpectrumList spectrumList)
        {
            if (spectrumList == null || spectrumList.Size() == 0)
            {
                return null;
            }
            int totalSpectrumCount = spectrumList.Size();
            int[] x = new int[totalSpectrumCount];
            int[] y = new int[totalSpectrumCount];
            int[] z = new int[totalSpectrumCount];            
            
            for (int i = 0; i < totalSpectrumCount; i++)
            {
                x[i] = spectrumList.GetSpectrum(i).GetPixelLocation().GetX();
                y[i] = spectrumList.GetSpectrum(i).GetPixelLocation().GetY();
                z[i] = spectrumList.GetSpectrum(i).GetPixelLocation().GetZ();
            }
            return new SpectraPosition(x, y, z);
        }

    }
}
