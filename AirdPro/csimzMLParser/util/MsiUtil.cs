using AirdPro.csimzMLParser.imzml;
using AirdPro.csimzMLParser.mzml;
using AirdSDK.Bean.Msi;
using System.Linq;

namespace AirdPro.csimzMLParser.util
{
    public class MsiUtil
    {
        public static MsiInfo GetMsiInfo(ImzML imzML)
        {
            MsiInfo msiInfo = new();
            //fileOrganisation: row per file, image per file，spectrum per file
            msiInfo.fileOrganisation = FileOrganisation.IMAGE_PER_FILE.Name;
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

            CVParam cvParam = fileContent?.GetCVParam(FileContent.SHA1_CHECKSUM_ID);
            if (cvParam == null)
            {
                cvParam = fileContent?.GetCVParam(FileContent.MD5_CHECKSUM_ID);
            }
            ibdInfo.checkSum = cvParam?.GetValueAsString();

            cvParam = fileContent?.GetCVParam(FileContent.BINARY_TYPE_CONTINUOUS_ID);
            if (cvParam == null)
            {
                cvParam = fileContent?.GetCVParam(FileContent.BINARY_TYPE_PROCESSED_ID);
            }
            ibdInfo.binaryType = cvParam?.GetValueAsString();

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
            ImageInfo imageInfo = new ImageInfo();

            imageInfo.maxPixelX = imzML.GetWidth();
            imageInfo.maxPixelY = imzML.GetHeight();
            imageInfo.maxPixelZ = imzML.GetDepth();

            ScanSettings scanSettings = imzML.GetScanSettingsList().GetScanSettings(0);
            imageInfo.maxDimensionX = scanSettings?.GetCVParam(ScanSettings.MAX_DIMENSION_X_ID)?.GetValueAsLong()  ?? -1;
            imageInfo.maxDimensionY = scanSettings?.GetCVParam(ScanSettings.MAX_DIMENSION_Y_ID)?.GetValueAsLong() ?? -1;
            long pixelSizeX = scanSettings?.GetCVParam(ScanSettings.PIXEL_SIZE_X_ID)?.GetValueAsLong() ?? -1;            
            long pixelSizeY = pixelSizeX;
            CVParam cvParam = scanSettings.GetCVParam(ScanSettings.PIXEL_SIZE_Y_ID);           
            if (cvParam != null)
            {
                pixelSizeY = cvParam.GetValueAsLong();
            }
            imageInfo.pixelSize = pixelSizeX;
            imageInfo.pixelSizeX = pixelSizeX;
            imageInfo.pixelSizeY = pixelSizeY;
            imageInfo.imageShape = scanSettings?.GetCVParam(ScanSettings.IMAGE_ID)?.ToString();

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
            sampleStage.targetMaterial = sample?.GetCVParam(Sample.TARGET_MATERIAL_ID)?.ToString();

            return sampleStage;
        }

        public static ScanInfo GetScanInfo(ImzML imzML)
        {
            ScanInfo scanInfo = new ScanInfo();

            ScanSettings scanSettings = imzML.GetScanSettingsList().GetScanSettings(0);
            if (scanSettings != null)
            {
                //linescanSequence
                string linescanSequence = scanSettings.GetCVParam(ScanSettings.BOTTOM_UP_ID)?.ToString();
                if(linescanSequence == null)
                {
                    linescanSequence = scanSettings.GetCVParam(ScanSettings.TOP_DOWN_ID)?.ToString();
                    if (linescanSequence == null)
                    {
                        linescanSequence = scanSettings.GetCVParam(ScanSettings.LEFT_RIGHT_ID)?.ToString();
                        if (linescanSequence == null)
                        {
                            linescanSequence = scanSettings.GetCVParam(ScanSettings.RIGHT_LEFT_ID)?.ToString();
                            if (linescanSequence == null)
                            {
                                linescanSequence = scanSettings.GetCVParam(ScanSettings.NO_DIRECTION_ID)?.ToString();
                            }
                        }
                    }
                }
                scanInfo.linescanSequence = linescanSequence;

                //scanPattern
                string scanPattern = scanSettings.GetCVParam(ScanSettings.MEANDERING_ID)?.ToString();
                if (scanPattern == null)
                {
                    scanPattern = scanSettings.GetCVParam(ScanSettings.FLYBACK_ID)?.ToString();
                    if (scanPattern == null)
                    {
                        scanPattern = scanSettings.GetCVParam(ScanSettings.RANDOM_ACCESS_ID)?.ToString();                        
                    }
                }
                scanInfo.scanPattern = scanPattern;

                //scanType
                string scanType = scanSettings.GetCVParam(ScanSettings.HORIZONTAL_LINESCAN_ID)?.ToString();
                if (scanType == null)
                {
                    scanType = scanSettings.GetCVParam(ScanSettings.VERTICAL_LINESCAN_ID)?.ToString();                    
                }
                scanInfo.scanType = scanType;

                //linescanDirection
                string linescanDirection = scanSettings.GetCVParam(ScanSettings.LINESCAN_BOTTOM_UP_ID)?.ToString();
                if (linescanDirection == null)
                {
                    linescanDirection = scanSettings.GetCVParam(ScanSettings.LINESCAN_LEFT_RIGHT_ID)?.ToString();
                    if (linescanDirection == null)
                    {
                        linescanDirection = scanSettings.GetCVParam(ScanSettings.LINESCAN_RIGHT_LEFT_ID)?.ToString();
                        if (linescanDirection == null)
                        {
                            linescanDirection = scanSettings.GetCVParam(ScanSettings.LINESCAN_TOP_DOWN_ID)?.ToString();                            
                        }
                    }
                }
                scanInfo.linescanDirection = linescanDirection;
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
