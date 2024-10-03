using AirdPro.csimzMLParser.data;
using AirdPro.csimzMLParser.exceptions;
using AirdPro.csimzMLParser.mzml;
using AirdPro.csimzMLParser.obo;
using log4net;
using System.IO;
using System.Text;
using System.Xml;
using System;

namespace AirdPro.csimzMLParser.parser
{
    public class MzMLHandler : MzMLHeaderHandler
    {
        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(MzMLHandler));

        protected bool processingBinary;
        protected FileInfo temporaryBinaryFile;
        protected BinaryWriter temporaryFileStream;
        protected StringBuilder binaryData;
        protected long offset = 0;

        public MzMLHandler(OBO obo, FileInfo temporaryBinaryFile) : base(obo)
        {
            binaryData = new StringBuilder();
            this.temporaryBinaryFile = temporaryBinaryFile;
            this.dataStorage = new BinaryDataStorage(temporaryBinaryFile, true);

            temporaryFileStream = new BinaryWriter(new FileStream(temporaryBinaryFile.FullName, FileMode.Create));
        }

        public MzML ParsemzML(string filename)
        {
            return ParsemzML(filename, null);
        }

        public MzML ParsemzML(string filename, IParserListener listener)
        {
            try
            {
                OBO obo = OBO.GetOBO();

                FileInfo tmpFile = new FileInfo(Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename) + ".tmp"));
                AppDomain.CurrentDomain.ProcessExit += (sender, args) => tmpFile.Delete();

                MzMLHandler handler = new MzMLHandler(obo, tmpFile);

                if (listener != null)
                {
                    handler.RegisterParserListener(listener);
                }

                XmlReaderSettings settings = new XmlReaderSettings
                {
                    IgnoreWhitespace = true,
                    IgnoreComments = true
                };

                using (XmlReader reader = XmlReader.Create(filename, settings))
                {
                    while (reader.Read())
                    {
                        StartElement(reader);
                    }
                }

                handler.mzML.SetOBO(obo);
                return handler.mzML;
            }
            catch (XmlException ex)
            {
                LOGGER.Error(ex.Message, ex);
                throw new MzMLParseException(new FatalParseIssue("XmlException: " + ex.Message, ex.Message), ex);
            }
            catch (FileNotFoundException ex)
            {
                LOGGER.Error(ex.Message, ex);
                throw new MzMLParseException(new FatalParseIssue("File not found: " + filename, ex.Message), ex);
            }
            catch (IOException ex)
            {
                LOGGER.Error(ex.Message, ex);
                throw new MzMLParseException(new FatalParseIssue("IOException: " + ex.Message, ex.Message), ex);
            }
        }

        public bool DeleteTemporaryFile()
        {
            try
            {
                if (temporaryFileStream != null)
                {
                    temporaryFileStream.Close();
                }
            }
            catch (IOException e)
            {
                LOGGER.Error("Error closing temporary file stream.", e);
                return false; 
            }
            finally
            {
                temporaryFileStream = null;
            }
           
            if (temporaryBinaryFile.Exists)
            {
                try
                {
                    temporaryBinaryFile.Delete();
                    LOGGER.Info("Temporary file deleted successfully.");
                    return true; 
                }
                catch (IOException e)
                {
                    LOGGER.Error("Error deleting temporary file.", e);
                    return false; 
                }
            }
            else
            {
                LOGGER.Info("Temporary file does not exist, nothing to delete.");
                return true; 
            }
        }

        public override void StartElement(XmlReader reader)
        {
            if (reader.Name.Equals("binary", StringComparison.OrdinalIgnoreCase))
            {
                binaryData.Length = 0;
                processingBinary = true;
            }
            else
            {
                base.StartElement(reader);
            }
        }

        public override void Characters(char[] ch, int start, int length)
        {
            if (processingBinary)
            {
                binaryData.Append(ch, start, length);
            }
            else
            {
                base.Characters(ch, start, length);
            }
        }

        public override void EndElement(XmlReader reader)
        {
            if (reader.Name.Equals("binary", StringComparison.OrdinalIgnoreCase))
            {
                byte[] processedData = Convert.FromBase64String(binaryData.ToString());
                int lengthToWrite = processedData.Length;
                try
                {
                    temporaryFileStream.Write(processedData, 0, lengthToWrite);
                }
                catch (IOException e)
                { 
                    LOGGER.Error(null,e);
                }               

                DataLocation location = new DataLocation(dataStorage, offset, lengthToWrite);
                currentBinaryDataArray.SetDataLocation(location);
                location.SetDataTransformation(currentBinaryDataArray.GenerateDataTransformation());

                offset += lengthToWrite;
                processingBinary = false;
            }
            else
            {
                base.EndElement(reader);
            }
        }
    }
}