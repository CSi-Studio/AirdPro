using AirdPro.csimzMLParser.mzml;
using log4net;
using System;
using System.IO;

namespace AirdPro.csimzMLParser.imzml
{
    public class ImzML : MzML, IMSIData
    {
        private static readonly long serialVersionUID = 1L;

        private static readonly ILog logger = LogManager.GetLogger(typeof(ImzML));

        private int width;
        private int height;
        private int depth;
        private FileInfo ibdFile;
        private double[] fullmzList;
        private double[,] ticImage;
        private Spectrum[][][] spectrumGrid;
        private PixelLocation[] pixelLocations;
        private double minMZ = double.MaxValue;
        private double maxMZ = double.MinValue;
        private int dimensionality = -1;

        public ImzML(string version) : base(version)
        {

        }

        public ImzML(ImzML imzML) : base(imzML)
        {

        }

        public ImzML(MzML mzML) : base(mzML)
        {

        }

        public PixelLocation[] GetPixelList()
        {
            if (pixelLocations == null)
            {
                pixelLocations = new PixelLocation[GetRun().GetSpectrumList().Size()];
                int index = 0;

                foreach (Spectrum spectrum in GetRun().GetSpectrumList())
                {
                    pixelLocations[index++] = spectrum.GetPixelLocation();
                }
            }

            return pixelLocations;
        }

        public double[] GetFullmzList()
        {
            logger.InfoFormat("Entering {0}.{1} method.", typeof(ImzML).FullName, nameof(GetFullmzList));

            if (fullmzList == null) 
            {
                Software imzMLConverter = this.GetSoftwareList().GetSoftware("imzMLConverter");

                logger.InfoFormat("Software found: {0}", imzMLConverter);

                if (imzMLConverter != null)
                {
                    CVParam offsetParam = imzMLConverter.GetCVParam(BinaryDataArray.EXTERNAL_OFFSET_ID);
                    CVParam encodedLengthParam = imzMLConverter.GetCVParam(BinaryDataArray.EXTERNAL_ENCODED_LENGTH_ID);

                    logger.InfoFormat("Found CVParams: {0}, {1}", [offsetParam, encodedLengthParam]);

                    if (offsetParam != null)
                    {
                        try
                        {
                            long offset = offsetParam.GetValueAsLong();
                            int encodedLength = encodedLengthParam.GetValueAsInteger();
                            byte[] fullmzListBytes = dataStorage.GetData(offset, encodedLength);
                            logger.Info($"Read in {fullmzListBytes.Length} bytes");
                            int numBytesPerDouble = sizeof(double); 
                            fullmzList = new double[fullmzListBytes.Length / numBytesPerDouble];

                            using (MemoryStream ms = new(fullmzListBytes))
                            using (BinaryReader reader = new(ms))
                            {
                                for (int i = 0; i < fullmzList.Length; i++)
                                {
                                    fullmzList[i] = reader.ReadDouble();
                                }
                            }
                            logger.Info($"First double in array is {fullmzList[0]}");
                        }
                        catch (IOException ex)
                        {
                            logger.Error("Error reading data.", ex);
                        }
                    }
                }
            }
            return fullmzList;
        }

        public int GetSpatialDimensionality()
        {
            int spatialDimensionality = 0;
            spatialDimensionality += (width > 1) ? 1 : 0;
            spatialDimensionality += (height > 1) ? 1 : 0;
            spatialDimensionality += (depth > 1) ? 1 : 0;

            return spatialDimensionality;
        }

        public int GetDimensionality()
        {
            if (dimensionality <= 0)
            {
                dimensionality = 1; 
                dimensionality += GetSpatialDimensionality();
                
                Spectrum spectrum1 = GetRun().GetSpectrumList().GetSpectrum(0);
                Spectrum spectrum2 = GetRun().GetSpectrumList().GetSpectrum(1);

                if (spectrum1.GetPixelLocation().Equals(spectrum2.GetPixelLocation()))
                {
                    dimensionality++;
                }
            }
            return dimensionality;
        }

        public int GetNumberOfSpectraPerPixel()
        {
            Spectrum firstSpectrum = GetRun().GetSpectrumList().GetSpectrum(0);
            PixelLocation location = firstSpectrum.GetPixelLocation();

            int numberOfSpectraPerPixel = 1;

            int numSpectra = GetRun().GetSpectrumList().Size();
            SpectrumList spectrumList = GetRun().GetSpectrumList();
            for (int i = 1; i < numSpectra; i++)
            {
                if (spectrumList.GetSpectrum(i).GetPixelLocation().Equals(location))
                {
                    numberOfSpectraPerPixel++;
                }
            }
            return numberOfSpectraPerPixel;
        }

        public Spectrum GetSpectrum(int x, int y)
        {
            lock (this)
            {
                return GetSpectrum(x, y, 1);
            }            
        }

        public Spectrum GetSpectrum(int x, int y, int z)
        {
            lock (this)             
            {
                if (spectrumGrid == null)
                {
                    spectrumGrid = new Spectrum[width][][]; 

                    foreach (Spectrum spectrum in GetRun().GetSpectrumList())
                    {
                        foreach (Scan scan in spectrum.GetScanList())
                        {
                            int curX = scan.GetCVParam(Scan.POSITION_X_ID).GetValueAsInteger();
                            int curY = scan.GetCVParam(Scan.POSITION_Y_ID).GetValueAsInteger();
                            int curZ = 1;

                            CVParam zPosCVParam = scan.GetCVParam(Scan.POSITION_Z_ID);

                            if (zPosCVParam != null)
                            {
                                curZ = zPosCVParam.GetValueAsInteger();
                            }

                            if (curX - 1 < 0 || curX - 1 >= spectrumGrid.Length || curY - 1 < 0 || curY - 1 >= spectrumGrid[0].Length || curZ - 1 < 0 || curZ - 1 >= spectrumGrid[0][0].Length)
                            {
                                return null;
                            }

                            spectrumGrid[curX - 1][curY - 1][curZ - 1] = spectrum;
                        }
                    }
                }

                if (spectrumGrid.Length >= 1 && (x - 1) < spectrumGrid.Length && x >= 1 &&
                    (y - 1) < spectrumGrid[0].Length && y >= 1 &&
                    (z - 1) < spectrumGrid[0][0].Length && z >= 1)
                {
                    return spectrumGrid[x - 1][y - 1][z - 1];
                }

                return null;
            }            
        }

        public int GetWidth()
        {
            if (width != 0)
            {
                return width;
            }

            ScanSettingsList scanSettingsList = GetScanSettingsList();

            if (scanSettingsList != null)
            {
                foreach (ScanSettings scanSettings in scanSettingsList)
                {
                    CVParam maxCountPixelX = scanSettings.GetCVParam(ScanSettings.MAX_COUNT_PIXEL_X_ID);

                    if (maxCountPixelX != null)
                    {
                        width = maxCountPixelX.GetValueAsInteger();
                    }
                }
            }
            return width;
        }

        public int GetHeight()
        {
            if (height != 0)
            {
                return height;
            }

            ScanSettingsList scanSettingsList = GetScanSettingsList();

            if (scanSettingsList != null)
            {
                foreach (ScanSettings scanSettings in scanSettingsList)
                {
                    CVParam maxCountPixelY = scanSettings.GetCVParam(ScanSettings.MAX_COUNT_PIXEL_Y_ID);

                    if (maxCountPixelY != null)
                    {
                        height = maxCountPixelY.GetValueAsInteger();
                    }
                }
            }
            return height;
        }

        public int GetDepth()
        {
            if (depth != 0)
            {
                return depth;
            }

            depth = 1;
            SpectrumList spectrumList = GetRun().GetSpectrumList();
            if (spectrumList != null)
            {
                foreach (Spectrum spectrum in spectrumList)
                {
                    CVParam maxCountPixelZ = spectrum.GetScanList().Get(0).GetCVParam(Scan.POSITION_Z_ID);

                    if (maxCountPixelZ != null)
                    {
                        int curDepth = maxCountPixelZ.GetValueAsInteger();

                        if (curDepth > depth)
                        {
                            depth = curDepth;
                        }
                    }
                }
            }
            return depth;
        }

        public double GetMinimumDetectedmz()
        {
            if (minMZ != double.MaxValue)
            {
                return minMZ;
            }

            foreach (Spectrum spectrum in GetRun().GetSpectrumList())
            {
                CVParam minDetectedMZ = spectrum.GetCVParam(Spectrum.LOWEST_OBSERVED_MZ_ID);

                if (minDetectedMZ == null)
                {
                    break;
                }

                double spectrumMinMZ = minDetectedMZ.GetValueAsDouble();

                if (minMZ > spectrumMinMZ)
                {
                    minMZ = spectrumMinMZ;
                }
            }

            return minMZ == double.MaxValue ? double.NaN : minMZ;
        }

        public double GetMaximumDetectedmz()
        {
            if (maxMZ != double.MinValue)
            {
                return maxMZ;
            }

            foreach (Spectrum spectrum in GetRun().GetSpectrumList())
            {
                CVParam maxDetectedMZ = spectrum.GetCVParam(Spectrum.HIGHEST_OBSERVED_MZ_ID);

                if (maxDetectedMZ == null)
                {
                    break;
                }

                double spectrumMaxMZ = maxDetectedMZ.GetValueAsDouble();

                if (maxMZ < spectrumMaxMZ)
                {
                    maxMZ = spectrumMaxMZ;
                }
            }

            return maxMZ == double.MinValue ? double.NaN : maxMZ;
        }

        public FileInfo GetIBDFile()
        {
            return ibdFile;
        }

        public void SetIBDFile(FileInfo ibdFile)
        {
            this.ibdFile = ibdFile;
        }

        public bool IsProcessed()
        {
            return GetFileDescription().GetFileContent().GetCVParam(FileContent.BINARY_TYPE_PROCESSED_ID) != null;
        }

        public bool IsContinuous()
        {
            return GetFileDescription().GetFileContent().GetCVParam(FileContent.BINARY_TYPE_CONTINUOUS_ID) != null;
        }

        public double[,] GenerateTICImage()
        {
            if (ticImage == null)
            {
                ticImage = new double[height, width];

                if (GetRun().GetSpectrumList() != null)
                {
                    foreach (Spectrum spectrum in GetRun().GetSpectrumList())
                    {
                        int x = spectrum.GetScanList().Get(0).GetCVParam(Scan.POSITION_X_ID).GetValueAsInteger() - 1;
                        int y = spectrum.GetScanList().Get(0).GetCVParam(Scan.POSITION_Y_ID).GetValueAsInteger() - 1;

                        try
                        {
                            double tic = spectrum.GetCVParam(Spectrum.TOTAL_ION_CURRENT_ID).GetValueAsDouble();
                            ticImage[y,x] = tic;
                        }
                        catch (Exception ex)
                        {
                            logger.Error(null, ex);
                        }
                    }
                }
            }

            return ticImage;
        }

       
    }
}
