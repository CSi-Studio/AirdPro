using AirdPro.csimzMLParser.data;
using AirdPro.csimzMLParser.exceptions;
using AirdPro.csimzMLParser.imzml;
using AirdPro.csimzMLParser.mzml;
using AirdPro.csimzMLParser.obo;
using log4net;
using System;
using System.IO;
using System.Xml;
using static AirdPro.csimzMLParser.mzml.BinaryDataArray;

namespace AirdPro.csimzMLParser.parser
{
    public class ImzMLHandler : MzMLHeaderHandler
    {
        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(ImzMLHandler));

        private FileInfo ibdFile;
        byte[] uncompressedData = null;
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

        public ImzMLHandler(OBO obo) : base(obo)
        {
        }

        public ImzMLHandler(OBO obo, FileInfo ibdFile, bool openDataStorage) : base(obo)
        {
            this.ibdFile = ibdFile;

            if (openDataStorage)
            {
                this.dataStorage = new BinaryDataStorage(ibdFile, false);
            }
        }

        public ImzML ParseimzML(string filename)
        {
            return ParseimzML(filename, true);
        }

        public ImzML ParseimzML(string filename, bool openDataStorage)
        {
            return ParseimzML(filename, openDataStorage, null);
        }

        public ImzML ParseimzML(string filename, bool openDataStorage, IParserListener listener)
        {
            ImzMLHandler handler;
            FileStream inputStream = null;            
            try
            {
                OBO obo = OBO.GetOBO();
                FileInfo ibdFile = new FileInfo(Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename) + ".ibd"));
               
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

                inputStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                byte[] compressedData = new byte[inputStream.Length];
                // 根据文件扩展名选择正确的解压方式
                if (filename.EndsWith(".lz4", StringComparison.OrdinalIgnoreCase))
                {
                    uncompressedData = new LZ4DataTransform().ReverseTransform(compressedData);
                }
                else if (filename.EndsWith(".zlib", StringComparison.OrdinalIgnoreCase))
                {
                    uncompressedData = new ZlibDataTransform().ReverseTransform(compressedData);
                }
                else if (filename.EndsWith(".zstd", StringComparison.OrdinalIgnoreCase))
                {
                    uncompressedData = new ZstdDataTransform().ReverseTransform(compressedData);
                }
                else if (filename.EndsWith(".xz", StringComparison.OrdinalIgnoreCase))
                {
                    uncompressedData = new XZDataTransform().ReverseTransform(compressedData);
                }
                else
                {
                    uncompressedData = compressedData;
                }
                //
                using (XmlReader reader = XmlReader.Create(inputStream, settings))
                {
                    while (reader.Read())
                    {
                        StartElement(reader);
                    }
                }

                ImzML imzML = handler.GetImzML();
                imzML.SetOBO(obo);

                return imzML;
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
            finally
            {
                if (inputStream != null)
                {
                    inputStream.Close();
                }
            }
        }

        protected override void StartCVParam(XmlReader reader)
        {
            string accession = reader.GetAttribute(MzMLHeaderHandler.ACCESSION_ATTRIBUTE_NAME);
            if (accession == Accessions.EXTERNAL_ENCODED_LENGTH_ID)
            {
                try
                {
                    currentNumBytes = long.Parse(reader.GetAttribute(MzMLHeaderHandler.VALUE_ATTRIBUTE_NAME));
                }
                catch (FormatException)
                {
                    currentNumBytes = (long)double.Parse(reader.GetAttribute(MzMLHeaderHandler.VALUE_ATTRIBUTE_NAME));
                }
            }
            else if (accession == Accessions.EXTERNAL_OFFSET_ID)
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

                double z = double.Parse(reader.GetAttribute(MzMLHeaderHandler.VALUE_ATTRIBUTE_NAME));

                if (z != current3DPositionZ)
                {
                    if (current3DPositionZ != double.PositiveInfinity && !haveDoneCheck)
                    {
                        int imageSize = imageMaxX * imageMaxY;
                        int numImagesGuess = (int)Math.Ceiling((numberOfSpectra * 1.0) / spectrumList.Size());

                        maxImagesX = (int)Math.Ceiling(Math.Sqrt(numImagesGuess));

                        LOGGER.InfoFormat("Found image size {0} ({1}, {2})", new Object[] { imageSize, imageMaxX, imageMaxY });
                        LOGGER.InfoFormat("Guessing we have {0} images based on {1} spectra", new Object[] { numImagesGuess, spectrumList.Size() });
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
            if (reader.GetAttribute(MzMLHeaderHandler.ACCESSION_ATTRIBUTE_NAME) != null)
            {
                mzML.SetAccession(reader.GetAttribute(MzMLHeaderHandler.ACCESSION_ATTRIBUTE_NAME));
            }

            if (reader.GetAttribute(MzMLHeaderHandler.ID_ATTRIBUTE_NAME) != null)
            {
                mzML.SetID(reader.GetAttribute(MzMLHeaderHandler.ID_ATTRIBUTE_NAME));
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
                    currentBinaryDataArray.GetCVParam(Accessions.EXTERNAL_OFFSET_ID).SetValueAsString("" + currentOffset);
                }

                DataLocation location = new DataLocation(this.dataStorage, currentOffset, (int)this.currentNumBytes);
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
                            new Object[] { currentScan.GetCVParam(Scan.POSITION_X_ID).GetValueAsInteger(), x });
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
                            new Object[] { currentScan.GetCVParam(Scan.POSITION_Y_ID).GetValueAsInteger(), y });
                }

                currentScan.GetCVParam(Scan.POSITION_Y_ID).SetValueAsString("" + newY);
            }

            if ("run".Equals(qName) && processingSCiLS3DData)
            {
                this.currentScanSettings.GetCVParam("IMS:1000042").SetValueAsString("" + this.datasetMaxX);
                this.currentScanSettings.GetCVParam("IMS:1000043").SetValueAsString("" + this.datasetMaxY);
            }

            base.EndElement(reader);
        }

        public ImzML GetImzML()
        {
            ImzML imzML = (ImzML)mzML;

            imzML.ibdFile = ibdFile;
            imzML.SetDataStorage(dataStorage);

            return imzML;
        }
    }
}
