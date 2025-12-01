using AirdPro.csimzMLParser.data;
using AirdPro.csimzMLParser.exceptions;
using AirdPro.csimzMLParser.imzml;
using AirdPro.csimzMLParser.mzml;
using AirdPro.csimzMLParser.obo;
using log4net;
using System;
using System.IO;
using System.Xml;

namespace AirdPro.csimzMLParser.parser
{
    public class ImzMLHandler : MzMLHeaderHandler
    {
        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(ImzMLHandler));

        private FileInfo ibdFile;        
        private long currentOffset;
        private long currentNumBytes;
        private bool processingSCiLS3DData = false;
        private int imageMaxX;
        private int imageMaxY;
        private int datasetMaxX;
        private int datasetMaxY;
        private int currentZ = 0;
        private double current3DPositionZ = double.PositiveInfinity;
        private bool haveDoneCheck = false;
        private int numImagesX = 0;
        private int numImagesY = 0;
        private int maxImagesX;
        private int previousMaxX = 0;
        private int previousMaxY = 0;
        private int currentMaxX = 0;
        private int currentMaxY = 0;

        //byte[] uncompressedData = null;

        public ImzMLHandler(OBO obo) : base(obo)
        {
        }

        public ImzMLHandler(OBO obo, FileInfo ibdFile, bool openDataStorage) : base(obo)
        {
            this.ibdFile = ibdFile;

            if (openDataStorage)
            {
                dataStorage = new BinaryDataStorage(ibdFile, false);
            }
        }

        public static ImzML ParseimzML(string filename)
        {
            return ParseimzML(filename, true);
        }

        public static ImzML ParseimzML(string filename, bool openDataStorage)
        {
            return ParseimzML(filename, openDataStorage, null);
        }

        public static ImzML ParseimzML(string filename, bool openDataStorage, IParserListener listener)
        {
            ImzMLHandler handler;
            FileStream inputStream = null;
            try
            {
                OBO obo = OBO.GetOBO();                
                FileInfo ibdFile = new(Path.ChangeExtension(filename, ".ibd"));

                handler = new ImzMLHandler(obo, ibdFile, openDataStorage);

                if (listener != null)
                {
                    handler.RegisterParserListener(listener);
                }

                XmlReaderSettings settings = new XmlReaderSettings
                {
                    IgnoreWhitespace = true,
                    IgnoreComments = true
                };

                using (inputStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                using (XmlReader reader = XmlReader.Create(inputStream, settings))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            handler.StartElement(reader);
                        }
                        else
                        {
                            handler.EndElement(reader);
                        }
                    }
                }

                ImzML imzML = handler.GetImzML();
                imzML.SetOBO(obo);

                // Check if Bruker data, and then correct the image to be relative rather than absolute
                InstrumentConfiguration ic = imzML.GetInstrumentConfigurationList().GetInstrumentConfiguration(0);
                if (ic.GetCVParamOrChild("MS:1000122") != null)
                {
                    int minX = int.MaxValue;
                    int minY = int.MaxValue;

                    foreach (Spectrum spectrum in imzML.GetRun().GetSpectrumList())
                    {
                        PixelLocation location = spectrum.GetPixelLocation();
                        if (location.GetX() < minX)
                            minX = location.GetX();
                        if (location.GetY() < minY)
                            minY = location.GetY();
                    }

                    foreach (Spectrum spectrum in imzML.GetRun().GetSpectrumList())
                    {
                        PixelLocation location = spectrum.GetPixelLocation();
                        spectrum.SetPixelLocation(location.GetX() - minX + 1, location.GetY() - minY + 1);
                    }

                    CVParam curWidth = imzML.GetScanSettingsList().GetScanSettings(0).GetCVParam(ScanSettings.MAX_COUNT_PIXEL_X_ID);
                    curWidth.SetValueAsString("" + (curWidth.GetValueAsLong() - minX + 1));

                    CVParam curHeight = imzML.GetScanSettingsList().GetScanSettings(0).GetCVParam(ScanSettings.MAX_COUNT_PIXEL_Y_ID);
                    curHeight.SetValueAsString("" + (curHeight.GetValueAsLong() - minY + 1));
                }
            }
            catch (XmlException ex)
            {
                LOGGER.Error(ex.Message, ex);
                throw new ImzMLParseException(new FatalParseIssue("XmlException: " + ex.Message, ex.Message), ex);
            }
            catch (FileNotFoundException ex)
            {
                LOGGER.Error(ex.Message, ex);
                throw new ImzMLParseException(new FatalParseIssue("File not found: " + ex.Message, ex.Message), ex);
            }
            catch (IOException ex)
            {
                LOGGER.Error(ex.Message, ex);
                throw new ImzMLParseException(new FatalParseIssue("IOException: " + ex.Message, ex.Message), ex);
            }
            return handler.GetImzML();
        }


        protected override void StartCVParam(XmlReader reader)
        {
            string accession = reader.GetAttribute(ACCESSION_ATTRIBUTE_NAME);
            if (accession.Equals(BinaryDataArray.EXTERNAL_ENCODED_LENGTH_ID))
            {
                try
                {
                    currentNumBytes = long.Parse(reader.GetAttribute(VALUE_ATTRIBUTE_NAME));
                }
                catch (FormatException)
                {
                    currentNumBytes = (long)double.Parse(reader.GetAttribute(VALUE_ATTRIBUTE_NAME));
                }
            }
            else if (accession.Equals(BinaryDataArray.EXTERNAL_OFFSET_ID))
            {               
                try
                {
                    currentOffset = long.Parse(reader.GetAttribute(VALUE_ATTRIBUTE_NAME));
                }
                catch (FormatException)
                {
                    currentOffset = (long)double.Parse(reader.GetAttribute(VALUE_ATTRIBUTE_NAME));
                }
            }
            base.StartCVParam(reader);
        }

        protected override void StartUserParam(XmlReader reader)
        {
            string name = reader.GetAttribute("name");
            if ("3DPositionZ".Equals(name))
            {
                processingSCiLS3DData = true;

                double z = double.Parse(reader.GetAttribute(VALUE_ATTRIBUTE_NAME));

                if (z != current3DPositionZ)
                {
                    if (current3DPositionZ != double.PositiveInfinity && !haveDoneCheck)
                    {
                        int imageSize = imageMaxX * imageMaxY;
                        int numImagesGuess = (int)Math.Ceiling((numberOfSpectra * 1.0) / spectrumList.Size());

                        maxImagesX = (int)Math.Ceiling(Math.Sqrt(numImagesGuess));

                        LOGGER.InfoFormat("Found image size {0} ({1}, {2})", [imageSize, imageMaxX, imageMaxY]);
                        LOGGER.InfoFormat("Guessing we have {0} images based on {1} spectrumList", [numImagesGuess, spectrumList.Size()]);
                        LOGGER.InfoFormat("Putting {0} images in x", maxImagesX);

                        haveDoneCheck = true;
                    }

                    numImagesX++;
                    previousMaxX = currentMaxX;

                    LOGGER.InfoFormat( "Changing previousMaxX to {0}", currentMaxX);

                    if (numImagesX > maxImagesX)
                    {
                        LOGGER.Info("Moving to next line");

                        numImagesX = 0;
                        numImagesY++;

                        currentMaxX = 0;
                        previousMaxX = 0;
                        previousMaxY = currentMaxY;
                    }
                    current3DPositionZ = z;
                    currentZ++;
                }
            }
            base.StartUserParam(reader);
        }

        protected override void StartMzML(XmlReader reader)
        {
            mzML = new ImzML(reader.GetAttribute("version"));

            // Add optional attributes
            if (reader.GetAttribute(ACCESSION_ATTRIBUTE_NAME) != null)
            {
                mzML.SetAccession(reader.GetAttribute(ACCESSION_ATTRIBUTE_NAME));
            }

            if (reader.GetAttribute(ID_ATTRIBUTE_NAME) != null)
            {
                mzML.SetID(reader.GetAttribute(ID_ATTRIBUTE_NAME));
            }

            contentStack.Push(mzML);
        }

        public override void EndElement(XmlReader reader)
        {
            string qName = reader.Name;
            if (ibdFile != null && qName.Equals("binaryDataArray"))
            {
                if (currentOffset < 0)
                {
                    InvalidExternalOffset issue = new InvalidExternalOffset((MzMLDataContainer)currentBinaryDataArray.GetParent().GetParent(), currentOffset);

                    NotifyParserListeners(issue);

                    currentOffset += DataLocation.EXTENDED_OFFSET;
                    currentBinaryDataArray.GetCVParam(BinaryDataArray.EXTERNAL_OFFSET_ID).SetValueAsString("" + currentOffset);
                }

                DataLocation location = new DataLocation(dataStorage, currentOffset, (int)currentNumBytes);
                currentBinaryDataArray.SetDataLocation(location);
                location.SetDataTransformation(currentBinaryDataArray.GenerateDataTransformation());
            }

            if ("scan".Equals(qName) && processingSCiLS3DData)
            {
                int x = currentScan.GetCVParam(Scan.POSITION_X_ID).GetValueAsInteger();

                if (x > imageMaxX)
                {
                    imageMaxX = x;
                }

                int newX = (previousMaxX + x);

                if (newX > currentMaxX)
                {
                    currentMaxX = newX;
                }
                if (newX > datasetMaxX)
                {
                    datasetMaxX = newX;
                }

                if (currentScan.GetCVParam(Scan.POSITION_X_ID).GetValueAsInteger() != x)
                {
                    LOGGER.InfoFormat("Mismatch between the X value in the currentScan ({0}) and the local variable x ({1})",
                            [currentScan.GetCVParam(Scan.POSITION_X_ID).GetValueAsInteger(), x]);
                }

                currentScan.GetCVParam(Scan.POSITION_X_ID).SetValueAsString("" + newX);

                int y = currentScan.GetCVParam(Scan.POSITION_Y_ID).GetValueAsInteger();

                if (y > imageMaxY)
                {
                    imageMaxY = y;
                }

                int newY = (previousMaxY + y);

                if (newY > currentMaxY)
                {
                    currentMaxY = newY;
                }
                if (newY > datasetMaxY)
                {
                    datasetMaxY = newY;
                }

                if (currentScan.GetCVParam(Scan.POSITION_Y_ID).GetValueAsInteger() != y)
                {
                    LOGGER.InfoFormat("Mismatch between the Y value in the currentScan ({0}) and the local variable y ({1})",
                            [currentScan.GetCVParam(Scan.POSITION_Y_ID).GetValueAsInteger(), y]);
                }

                currentScan.GetCVParam(Scan.POSITION_Y_ID).SetValueAsString("" + newY);
            }

            if ("run".Equals(qName) && processingSCiLS3DData)
            {
                currentScanSettings.GetCVParam("IMS:1000042").SetValueAsString("" + datasetMaxX);
                currentScanSettings.GetCVParam("IMS:1000043").SetValueAsString("" + datasetMaxY);
            }

            base.EndElement(reader);
        }

        public ImzML GetImzML()
        {
            ImzML imzML = (ImzML)mzML;

            imzML.SetIBDFile(ibdFile);
            imzML.SetDataStorage(dataStorage);

            return imzML;
        }
    }
}
