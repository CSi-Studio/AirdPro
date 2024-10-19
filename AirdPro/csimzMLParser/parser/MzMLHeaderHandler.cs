using AirdPro.csimzMLParser.data;
using AirdPro.csimzMLParser.exceptions;
using AirdPro.csimzMLParser.mzml;
using AirdPro.csimzMLParser.obo;
using HZH_Controls;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.Query.Dynamic;
using System.Xml;

namespace AirdPro.csimzMLParser.parser
{
    public class MzMLHeaderHandler
    {
        public const string ACCESSION_ATTRIBUTE_NAME = "accession";
        public const string VALUE_ATTRIBUTE_NAME = "value";
        private const string UNIT_ACCESSION_ATTRIBUTE_NAME = "unitAccession";
        public const string ID_ATTRIBUTE_NAME = "id";
        private const string COUNT_ATTRIBUTE_NAME = "count";

        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(MzMLHeaderHandler));

        protected MzML mzML;
        protected OBO obo;
        protected CVList cvList;
        protected FileDescription fileDescription;
        protected SourceFileList sourceFileList;
        protected ReferenceableParamGroupList referenceableParamGroupList;
        protected SampleList sampleList;
        protected SoftwareList softwareList;
        protected ScanSettingsList scanSettingsList;
        protected ScanSettings currentScanSettings;
        private SourceFileRefList currentSourceFileRefList;
        private TargetList currentTargetList;
        protected InstrumentConfigurationList instrumentConfigurationList;
        private InstrumentConfiguration currentInstrumentConfiguration;
        private ComponentList currentComponentList;
        private DataProcessingList dataProcessingList;
        private DataProcessing currentDataProcessing;
        protected Run run;
        protected SpectrumList spectrumList;
        private Spectrum currentSpectrum;
        private ScanList currentScanList;
        protected Scan currentScan;
        private ScanWindowList currentScanWindowList;
        private PrecursorList currentPrecursorList;
        private Precursor currentPrecursor;
        private SelectedIonList currentSelectedIonList;
        private ProductList currentProductList;
        private Product currentProduct;
        private BinaryDataArrayList currentBinaryDataArrayList;
        protected BinaryDataArray currentBinaryDataArray;
        private ChromatogramList chromatogramList;
        private Chromatogram currentChromatogram;
        protected Stack<MzMLContent> contentStack = new();
        private bool processingSpectrum;
        private bool processingChromatogram;
        private bool processingPrecursor;
        private bool processingProduct;
        private bool processingOffset;
        private readonly StringBuilder offsetData;
        private string previousOffsetIDRef;
        private string currentOffsetIDRef;
        private long previousOffset = -1;
        protected DataStorage dataStorage;
        private bool openDataStorage = true;
        protected int numberOfSpectra = 0;
        private readonly List<IParserListener> listeners;

        public MzMLHeaderHandler(OBO obo)
        {
            this.obo = obo;

            processingSpectrum = false;
            processingChromatogram = false;
            processingPrecursor = false;
            processingProduct = false;

            offsetData = new StringBuilder();

            listeners = [];
        }

        public void SetOpenDataStorage(bool openDataStorage)
        {
            this.openDataStorage = openDataStorage;
        }

        public MzMLHeaderHandler(OBO obo, FileInfo mzMLFile) : this(obo, mzMLFile, true) { }

        public MzMLHeaderHandler(OBO obo, FileInfo mzMLFile, bool openDataFile) : this(obo)
        {     
            if (openDataFile)
            {
                this.dataStorage = new MzMLSpectrumDataStorage(mzMLFile);
            }
        }

        public void RegisterParserListener(IParserListener listener)
        {
            this.listeners.Add(listener);

            LOGGER.Info($"Registered listener {listener}: {listeners.ToArray()}");
        }

        protected void NotifyParserListeners(IIssue issue)
        {
            LOGGER.Info($"Notifying {listeners.ToArray()} listeners about the issue {issue}");

            foreach (IParserListener listener in listeners)
            {
                listener.IssueFound(issue);
            }
        }

        public static MzML ParsemzMLHeader(string filename)
        {
            return ParsemzMLHeader(filename, true);
        }

        public static MzML ParsemzMLHeader(string filename, bool openDataFile)
        {
            return ParsemzMLHeader(filename, openDataFile, null);
        }

        public static MzML ParsemzMLHeader(string filename, IParserListener listener)
        {
            return ParsemzMLHeader(filename, true, listener);
        }

        public static MzML ParsemzMLHeader(string filename, bool openDataFile, IParserListener listener)
        {
            OBO obo = OBO.GetOBO();
            MzMLHeaderHandler handler;

            try
            {
                // Parse mzML
                handler = new MzMLHeaderHandler(obo, new FileInfo(filename), openDataFile);
                handler.SetOpenDataStorage(openDataFile);

                if (listener != null)
                {
                    handler.RegisterParserListener(listener);
                }

                XmlReaderSettings settings = new()
                {
                    IgnoreWhitespace = true
                };

                using (FileStream fs = new(filename, FileMode.Open, FileAccess.Read))
                using (XmlReader reader = XmlReader.Create(fs, settings))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            handler.StartElement(reader);
                        }
                    }
                }

                handler.mzML.SetOBO(obo);
            }
            catch (FatalRuntimeParseException runtimeException)
            {
                throw new MzMLParseException(runtimeException.GetIssue(), runtimeException);
            }
            catch (XmlException ex)
            {
                LOGGER.Error(nameof(ParsemzMLHeader), ex);

                throw new MzMLParseException(new InvalidMzMLIssue("XmlException: " + ex, ex.Message), ex);
            }
            catch (FileNotFoundException ex)
            {
                LOGGER.Error(nameof(ParsemzMLHeader), ex);

                throw new MzMLParseException(new FatalParseIssue("File not found: " + filename, ex.Message), ex);
            }
            catch (IOException ex)
            {
                LOGGER.Error(nameof(ParsemzMLHeader), ex);

                throw new MzMLParseException(new FatalParseIssue("IOException: " + ex, ex.Message), ex);
            }

            return handler.mzML;
        }

        protected int GetCountAttribute(XmlReader reader)
        {
            string countString = reader.GetAttribute(COUNT_ATTRIBUTE_NAME);
            int count = countString == null ? 0 : int.Parse(countString);

            return count;
        }

        protected virtual void StartCVParam(XmlReader reader)
        {
            if (contentStack.Count > 0)
            {
                string accession = reader.GetAttribute(ACCESSION_ATTRIBUTE_NAME);

                OBOTerm term = obo.GetTerm(accession);

                if (term == null)
                {
                    UserParam userParam = new(reader.GetAttribute(ACCESSION_ATTRIBUTE_NAME), reader.GetAttribute(VALUE_ATTRIBUTE_NAME), obo.GetTerm(reader.GetAttribute(UNIT_ACCESSION_ATTRIBUTE_NAME)));
                    ((MzMLContentWithParams)contentStack.Peek()).AddUserParam(userParam);

                    CVParamAccessionNotFoundIssue notFound = new(reader.GetAttribute(ACCESSION_ATTRIBUTE_NAME), userParam);

                    notFound.SetIssueLocation(contentStack.Peek());

                    NotifyParserListeners(notFound);
                }
                else
                {
                    if (term.IsObsolete())
                    {
                        ObsoleteTermUsed obsoleteIssue = new (term);
                        obsoleteIssue.SetIssueLocation(contentStack.Peek());

                        NotifyParserListeners(obsoleteIssue);
                    }
                    CVParam cvParam;
                    try
                    {
                        CVParam.CVParamType paramType = CVParam.GetCVParamType(term);                     

                        string value = reader.GetAttribute(VALUE_ATTRIBUTE_NAME);
                        OBOTerm units = obo.GetTerm(reader.GetAttribute(UNIT_ACCESSION_ATTRIBUTE_NAME));

                        switch (paramType)
                        {
                            case CVParam.CVParamType.STRING:
                                cvParam = new StringCVParam(term, value, units);
                                break;
                            case CVParam.CVParamType.EMPTY:
                                cvParam = new EmptyCVParam(term, units);

                                if (value != null && value.Length > 0)
                                {
                                    InvalidFormatIssue invalidFormatIssue = new (term, reader.GetAttribute(VALUE_ATTRIBUTE_NAME));
                                    invalidFormatIssue.SetIssueLocation(contentStack.Peek());

                                    NotifyParserListeners(invalidFormatIssue);
                                }
                                break;
                            case CVParam.CVParamType.LONG:
                                cvParam = new LongCVParam(term, long.Parse(value), units);
                                break;
                            case CVParam.CVParamType.DOUBLE:
                                cvParam = new DoubleCVParam(term, double.Parse(value), units);
                                break;
                            case CVParam.CVParamType.BOOLEAN:
                                cvParam = new BooleanCVParam(term, bool.Parse(value), units);
                                break;
                            case CVParam.CVParamType.INTEGER:
                                cvParam = new IntegerCVParam(term, int.Parse(value), units);
                                break;
                            default:
                                cvParam = new StringCVParam(term, reader.GetAttribute(VALUE_ATTRIBUTE_NAME), obo.GetTerm(reader.GetAttribute(UNIT_ACCESSION_ATTRIBUTE_NAME)));

                                InvalidFormatIssue formatIssue = new(term, paramType);
                                formatIssue.FixAttemptedByChangingType((StringCVParam)cvParam);
                                formatIssue.SetIssueLocation(contentStack.Peek());
                                NotifyParserListeners(formatIssue);

                                break;
                        }
                    }
                    catch (FormatException)
                    {
                        cvParam = new StringCVParam(term, reader.GetAttribute(VALUE_ATTRIBUTE_NAME), obo.GetTerm(reader.GetAttribute(UNIT_ACCESSION_ATTRIBUTE_NAME)));

                        InvalidFormatIssue formatIssue = new (term, reader.GetAttribute(VALUE_ATTRIBUTE_NAME));
                        formatIssue.FixAttemptedByChangingType((StringCVParam)cvParam);
                        formatIssue.SetIssueLocation(contentStack.Peek());

                        NotifyParserListeners(formatIssue);
                    }

                    if (contentStack.Peek() is MzMLContentWithParams)
                    {
                        ((MzMLContentWithParams)contentStack.Peek()).AddCVParam(cvParam);
                    }
                    else
                    {
                        throw new InvalidOperationException("Failure to add CVParam to " + contentStack.Peek());
                    }
                }
            }
            else
            {
                throw new FatalRuntimeParseException(new InvalidMzMLIssue("<cvParam> tag without a parent."));
            }
        }

        protected void StartReferenceableParamGroupRef(XmlReader reader)
        {
            bool foundReference = false;

            if (referenceableParamGroupList != null)
            {
                string refValue = reader.GetAttribute("ref");
                ReferenceableParamGroup group = referenceableParamGroupList.GetReferenceableParamGroup(refValue);

                if (group != null)
                {
                    ReferenceableParamGroupRef rpgRef = new (group);

                    if (contentStack.Count > 0)
                    {
                        foundReference = true;

                        ((MzMLContentWithParams)contentStack.Peek()).AddReferenceableParamGroupRef(rpgRef);
                    }
                }
            }

            if (!foundReference)
            {
                string refValue = reader.GetAttribute("ref");
                MissingReferenceIssue missingRefIssue = new(refValue, "referenceableParamGroupRef", "ref");
                missingRefIssue.SetIssueLocation(contentStack.Peek());
                missingRefIssue.FixAttemptedByRemovingReference();

                NotifyParserListeners(missingRefIssue);
            }
        }

        protected void StartRun(XmlReader reader)
        {
            string instrumentConfigurationRef = reader.GetAttribute("defaultInstrumentConfigurationRef");
            InstrumentConfiguration instrumentConfiguration = null;
            if (instrumentConfigurationRef != null && !instrumentConfigurationRef.IsEmpty())
            {
                try
                {
                    instrumentConfiguration = instrumentConfigurationList.GetInstrumentConfiguration(instrumentConfigurationRef);
                }
                catch (Exception ex)
                {
                    throw new FatalRuntimeParseException(new InvalidMzMLIssue("<instrumentConfigurationList> tag not defined prior to defining <run> tag.", ex.Message), ex);
                }
            }
            if (instrumentConfiguration != null)
            {
                run = new Run(reader.GetAttribute(ID_ATTRIBUTE_NAME), instrumentConfiguration);
            }
            else
            {
                MissingReferenceIssue missingRefIssue = new (instrumentConfigurationRef, "run", "defaultInstrumentConfigurationRef");
                missingRefIssue.SetIssueLocation(contentStack.Peek());

                if (currentInstrumentConfiguration != null)
                {
                    missingRefIssue.FixAttemptedByChangingReference(currentInstrumentConfiguration);
                    run = new Run(reader.GetAttribute(ID_ATTRIBUTE_NAME), currentInstrumentConfiguration);
                }
                else
                {
                    missingRefIssue.FixAttemptedByRemovingReference();
                    run = new Run(reader.GetAttribute(ID_ATTRIBUTE_NAME), null);
                }
                NotifyParserListeners(missingRefIssue);
            }

            string defaultSourceFileRef = reader.GetAttribute("defaultSourceFileRef");
            if (defaultSourceFileRef != null)
            {
                bool foundRef = false;
                if (sourceFileList != null)
                {
                    SourceFile sourceFile = sourceFileList.GetSourceFile(defaultSourceFileRef);
                    if (sourceFile != null)
                    {
                        run.SetDefaultSourceFileRef(sourceFile);
                        foundRef = true;
                    }
                }
                if (!foundRef)
                {
                    MissingReferenceIssue missingRefIssue = new (defaultSourceFileRef, "run", "defaultSourceFileRef");
                    missingRefIssue.SetIssueLocation(contentStack.Peek());
                    missingRefIssue.FixAttemptedByRemovingReference();
                    NotifyParserListeners(missingRefIssue);
                }
            }

            string sampleRef = reader.GetAttribute("sampleRef");
            if (sampleRef != null)
            {
                bool foundRef = false;
                if (sampleList != null)
                {
                    Sample sample = sampleList.GetSample(sampleRef);
                    if (sample != null)
                    {
                        foundRef = true;
                        run.SetSampleRef(sample);
                    }
                }
                if (!foundRef)
                {
                    MissingReferenceIssue missingRefIssue = new(sampleRef, "run", "sampleRef");
                    missingRefIssue.SetIssueLocation(contentStack.Peek());
                    missingRefIssue.FixAttemptedByRemovingReference();
                    NotifyParserListeners(missingRefIssue);
                }
            }

            string startTimeStamp = reader.GetAttribute("startTimeStamp");
            if (startTimeStamp != null)
            {
                string format = "yyyy-MM-dd'T'HH:mm:ss";
                try
                {
                    DateTime dateTime;
                    if (startTimeStamp.Contains("BST"))
                    {
                        format = "ddd MMM dd HH:mm:ss 'BST' yyyy";
                        // 将BST替换为标准的时区表示
                        dateTime = DateTimeOffset.ParseExact(startTimeStamp, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).UtcDateTime;
                    }
                    else
                    {
                        dateTime = DateTime.ParseExact(startTimeStamp, "yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    }                                   
                    
                    run.SetStartTimeStamp(dateTime);
                }
                catch (ParseException)
                {
                    InvalidFormatIssue formatIssue = new ("startTimeStamp", format, startTimeStamp);
                    formatIssue.SetIssueLocation(contentStack.Peek());
                    NotifyParserListeners(formatIssue);
                    try
                    {
                        DateTime parsed = DateTime.ParseExact(startTimeStamp, "EEE MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                        run.SetStartTimeStamp(parsed);
                    }
                    catch (ParseException)
                    {
                        InvalidFormatIssue secondFormatIssue = new ("startTimeStamp", "EEE MMM dd HH:mm:ss zzz yyyy", startTimeStamp);
                        secondFormatIssue.SetIssueLocation(contentStack.Peek());
                        NotifyParserListeners(secondFormatIssue);
                    }
                }
            }
            mzML.SetRun(run);
            contentStack.Push(run);
        }

        protected void StartInstrumentConfiguration(XmlReader reader)
        {
            currentInstrumentConfiguration = new InstrumentConfiguration(reader.GetAttribute(ID_ATTRIBUTE_NAME));
            string scanSettingsRef = reader.GetAttribute("scanSettingsRef");
            if (scanSettingsRef != null)
            {
                ScanSettings scanSettings = scanSettingsList.GetScanSettings(scanSettingsRef);  
                if (scanSettings != null)
                {
                    currentInstrumentConfiguration.SetScanSettingsRef(scanSettings);
                }
                else
                {
                    MissingReferenceIssue missingRefIssue = new(scanSettingsRef, "instrumentConfiguration", "scanSettingsRef");
                    missingRefIssue.SetIssueLocation(contentStack.Peek());
                    missingRefIssue.FixAttemptedByRemovingReference();
                    NotifyParserListeners(missingRefIssue);
                }
            }
            try
            {
                instrumentConfigurationList.AddInstrumentConfiguration(currentInstrumentConfiguration);                
            }
            catch (Exception ex)
            {
                throw new FatalRuntimeParseException(new InvalidMzMLIssue("<instrumentConfigurationList> tag not defined prior to defining <instrumentConfiguration> tag.", ex.Message), ex);
            }
            contentStack.Push(currentInstrumentConfiguration);
        }

        protected void StartScan(XmlReader reader)
        {
            currentScan = new Scan();

            string externalSpectrumID = reader.GetAttribute("externalSpectrumID");
            if (!string.IsNullOrEmpty(externalSpectrumID))
            {
                currentScan.ExternalSpectrumID = externalSpectrumID;
            }

            string instrumentConfigurationRef = reader.GetAttribute("instrumentConfigurationRef");
            if (!string.IsNullOrEmpty(instrumentConfigurationRef))
            {
                InstrumentConfiguration instrumentConfiguration = null;

                try
                {
                    if (instrumentConfigurationList != null)
                    {
                        instrumentConfiguration = instrumentConfigurationList.GetInstrumentConfiguration(instrumentConfigurationRef);
                    }
                }
                catch (Exception ex)
                {
                    throw new FatalRuntimeParseException(new InvalidMzMLIssue("<instrumentConfigurationList> tag not defined prior to defining <scan> tag.", ex.Message), ex);
                }

                if (instrumentConfiguration != null)
                {
                    currentScan.InstrumentConfigurationRef = instrumentConfiguration;
                }
                else
                {
                    MissingReferenceIssue refIssue = new (instrumentConfigurationRef, "scan", "instrumentConfigurationRef");
                    refIssue.SetIssueLocation(contentStack.Peek());

                    if (currentInstrumentConfiguration != null)
                    {
                        currentScan.InstrumentConfigurationRef = currentInstrumentConfiguration;
                        refIssue.FixAttemptedByChangingReference(currentInstrumentConfiguration);
                    }
                    else
                    {
                        refIssue.FixAttemptedByRemovingReference();
                    }

                    NotifyParserListeners(refIssue);
                }
            }
            else
            {
                InstrumentConfiguration defaultIC = run.GetDefaultInstrumentConfiguration();

                if (defaultIC != null)
                {
                    currentScan.InstrumentConfigurationRef = defaultIC;
                }
            }

            string sourceFileRef = reader.GetAttribute("sourceFileRef");
            if (!string.IsNullOrEmpty(sourceFileRef))
            {
                bool foundRef = false;

                if (sourceFileList != null)
                {
                    SourceFile sourceFile = sourceFileList.GetSourceFile(sourceFileRef);

                    if (sourceFile != null)
                    {
                        currentScan.SourceFileRef = sourceFile;
                        foundRef = true;
                    }
                }

                if (!foundRef)
                {
                    MissingReferenceIssue refIssue = new (sourceFileRef, "scan", "sourceFileRef");
                    refIssue.SetIssueLocation(contentStack.Peek());
                    refIssue.FixAttemptedByRemovingReference();

                    NotifyParserListeners(refIssue);
                }
            }

            if (reader.GetAttribute("spectrumRef") != null)
            {
                currentScan.SpectrumRef = reader.GetAttribute("spectrumRef");
            }

            try
            {
                if (currentScanList != null)
                {
                    currentScanList.AddScan(currentScan);
                }
                else
                {
                    throw new InvalidOperationException("currentScanList is not defined.");
                }
            }
            catch (Exception ex)
            {
                throw new FatalRuntimeParseException(new InvalidMzMLIssue("<scanList> tag not defined prior to defining <scan> tag.", ex.Message), ex);
            }

            contentStack.Push(currentScan);
        }

        protected void StartPrecursor(XmlReader reader)
        {
            processingPrecursor = true;
            currentPrecursor = new Precursor();

            string sourceFileRef = reader.GetAttribute("sourceFileRef");

            if (!string.IsNullOrEmpty(sourceFileRef))
            {
                bool foundRef = false;

                if (sourceFileList != null)
                {
                    SourceFile sourceFile = sourceFileList.GetSourceFile(sourceFileRef);

                    if (sourceFile != null)
                    {
                        foundRef = true;
                        currentPrecursor.SetExternalSpectrum(sourceFile, reader.GetAttribute("externalSpectrumID"));
                    }
                }

                if (!foundRef)
                {
                    MissingReferenceIssue refIssue = new (sourceFileRef, "precursor", "sourceFileRef");
                    refIssue.SetIssueLocation(contentStack.Peek());
                    refIssue.FixAttemptedByRemovingReference();

                    NotifyParserListeners(refIssue);
                }
            }

            if (reader.GetAttribute("spectrumRef") != null)
            {
                currentPrecursor.SetSpectrumRef(spectrumList.GetSpectrum(reader.GetAttribute("spectrumRef")));
            }

            if (processingSpectrum)
            {
                try
                {
                    currentPrecursorList.AddPrecursor(currentPrecursor);
                }
                catch (Exception ex)
                {
                    throw new FatalRuntimeParseException(new InvalidMzMLIssue("<precursorList> tag not defined prior to defining <precursor> tag.", ex.Message), ex);
                }
            }
            else if (processingChromatogram)
            {
                try
                {
                    currentChromatogram.SetPrecursor(currentPrecursor);
                }
                catch (Exception ex)
                {
                    throw new FatalRuntimeParseException(new InvalidMzMLIssue("<chromatogram> tag not defined prior to defining <precursor> tag.", ex.Message), ex);
                }
            }

            contentStack.Push(currentPrecursor);
        }

        protected void StartProcessingMethod(XmlReader reader)
        {
            string softwareRef = reader.GetAttribute("softwareRef");

            bool referenceFound = false;
            Software software = null;

            if (softwareList != null)
            {
                software = softwareList.GetSoftware(softwareRef);

                if (software != null)
                {
                    referenceFound = true;
                }
                else
                {
                    software = softwareList.Size() > 0 ? softwareList.GetSoftware(0) : null;
                }
            }

            ProcessingMethod pm = new (software);
            currentDataProcessing.AddProcessingMethod(pm);

            contentStack.Push(pm);

            if (!referenceFound && software != null)
            {
                MissingReferenceIssue missingRefIssue = new (softwareRef, "processingMethod", "softwareRef");
                missingRefIssue.SetIssueLocation(contentStack.Peek());
                missingRefIssue.FixAttemptedByChangingReference(software);

                NotifyParserListeners(missingRefIssue);
            }
            else if (!referenceFound)
            {
                MissingReferenceIssue missingRefIssue = new(softwareRef, "processingMethod", "softwareRef");
                missingRefIssue.SetIssueLocation(contentStack.Peek());
                missingRefIssue.FixAttemptedByRemovingReference();

                NotifyParserListeners(missingRefIssue);
            }
        }

        protected void StartBinaryDataArray(XmlReader reader)
        {
            int encodedLength = int.Parse(reader.GetAttribute("encodedLength"));
            currentBinaryDataArray = new BinaryDataArray(encodedLength);

            string arrayLength = reader.GetAttribute("arrayLength");
            if (!string.IsNullOrEmpty(arrayLength))
            {
                currentBinaryDataArray.SetArrayLength(int.Parse(arrayLength));
            }

            string dataProcessingRef = reader.GetAttribute("dataProcessingRef");

            if (!string.IsNullOrEmpty(dataProcessingRef))
            {
                bool foundRef = false;
                DataProcessing dataProcessing;

                if (dataProcessingList != null)
                {
                    dataProcessing = dataProcessingList.GetDataProcessing(dataProcessingRef);

                    if (dataProcessing != null)
                    {
                        foundRef = true;
                        currentBinaryDataArray.SetDataProcessingRef(dataProcessing);
                    }
                }

                if (!foundRef)
                {
                    MissingReferenceIssue refIssue = new(dataProcessingRef, "binaryDataArray", "dataProcessingRef");
                    refIssue.SetIssueLocation(contentStack.Peek());
                    refIssue.FixAttemptedByRemovingReference();

                    NotifyParserListeners(refIssue);
                }
            }

            try
            {
                if (currentBinaryDataArrayList != null)
                {
                    currentBinaryDataArrayList.AddBinaryDataArray(currentBinaryDataArray);
                }
                else
                {
                    throw new InvalidOperationException("currentBinaryDataArrayList is not defined.");
                }
            }
            catch (Exception ex)
            {
                throw new FatalRuntimeParseException(new InvalidMzMLIssue("<binaryDataArrayList> tag not defined prior to defining <binaryDataArray> tag.", ex.Message), ex);
            }

            contentStack.Push(currentBinaryDataArray);
        }

        protected void StartSpectrumList(XmlReader reader)
        {
            string defaultDataProcessingRef = reader.GetAttribute("defaultDataProcessingRef");
            DataProcessing dataProcessing = null;
            bool foundRef = false;

            if (defaultDataProcessingRef != null && dataProcessingList != null)
            {
                dataProcessing = dataProcessingList.GetDataProcessing(defaultDataProcessingRef);

                if (dataProcessing != null)
                {
                    numberOfSpectra = int.Parse(reader.GetAttribute(COUNT_ATTRIBUTE_NAME));
                    foundRef = true;
                }
            }

            if (!foundRef)
            {
                MissingReferenceIssue refIssue = new(defaultDataProcessingRef, "spectrumList", "defaultDataProcessingRef");
                refIssue.SetIssueLocation(contentStack.Peek());
                refIssue.FixAttemptedByRemovingReference();

                NotifyParserListeners(refIssue);
            }

            spectrumList = new SpectrumList(numberOfSpectra, dataProcessing);

            if (run == null)
            {
                throw new FatalRuntimeParseException(new InvalidMzMLIssue("<run> tag not defined prior to defining <spectrumList> tag."));
            }

            run.SetSpectrumList(spectrumList);

            contentStack.Push(spectrumList);
        }

        protected void StartSpectrum(XmlReader reader)
        {
            currentSpectrum = new Spectrum(reader.GetAttribute(ID_ATTRIBUTE_NAME), int.Parse(reader.GetAttribute("defaultArrayLength")));

            string dataProcessingRef = reader.GetAttribute("dataProcessingRef");

            if (dataProcessingRef != null)
            {
                bool foundRef = false;

                if (dataProcessingList != null)
                {
                    DataProcessing dataProcessing = dataProcessingList.GetDataProcessing(dataProcessingRef);

                    if (dataProcessing != null)
                    {
                        currentSpectrum.SetDataProcessingRef(dataProcessing);
                        foundRef = true;
                    }
                }

                if (!foundRef)
                {
                    MissingReferenceIssue refIssue = new (dataProcessingRef, "spectrum", "dataProcessingRef");
                    refIssue.SetIssueLocation(contentStack.Peek());
                    refIssue.FixAttemptedByRemovingReference();

                    NotifyParserListeners(refIssue);
                }
            }
            else
            {
                // If a specific dataProcessingRef has not been defined, then assign the default.
                currentSpectrum.SetDataProcessingRef(spectrumList.GetDefaultDataProcessingRef());
            }

            string sourceFileRef = reader.GetAttribute("sourceFileRef");

            if (sourceFileRef != null)
            {
                bool foundRef = false;

                if (sourceFileList != null)
                {
                    SourceFile sourceFile = sourceFileList.GetSourceFile(sourceFileRef);

                    if (sourceFile != null)
                    {
                        currentSpectrum.SetSourceFileRef(sourceFile);
                        foundRef = true;
                    }
                }

                if (!foundRef)
                {
                    MissingReferenceIssue refIssue = new(sourceFileRef, "spectrum", "sourceFileRef");
                    refIssue.SetIssueLocation(contentStack.Peek());
                    refIssue.FixAttemptedByRemovingReference();

                    NotifyParserListeners(refIssue);
                }
            }
            else
            {
                currentSpectrum.SetSourceFileRef(run.GetDefaultSourceFileRef());
            }

            if (reader.GetAttribute("spotID") != null)
            {
                currentSpectrum.SetSpotID(reader.GetAttribute("spotID"));
            }

            processingSpectrum = true;

            try
            {
                spectrumList.AddSpectrum(currentSpectrum);
            }
            catch (Exception ex)
            {
                throw new FatalRuntimeParseException(new InvalidMzMLIssue("<spectrumList> tag not defined prior to defining <spectrum> tag.", ex.Message), ex);
            }

            contentStack.Push(currentSpectrum);
        }

        protected void StartChromatogramList(XmlReader reader)
        {
            string defaultDataProcessingRef = reader.GetAttribute("defaultDataProcessingRef");

            if (!string.IsNullOrEmpty(defaultDataProcessingRef))
            {
                DataProcessing dataProcessing;

                try
                {
                    dataProcessing = dataProcessingList.GetDataProcessing(defaultDataProcessingRef);
                }
                catch (Exception ex)
                {
                    throw new FatalRuntimeParseException(new InvalidMzMLIssue("<dataProcessingList> tag not defined prior to defining <chromatogramList> tag.", ex.Message), ex);
                }

                if (dataProcessing != null)
                {
                    int count = int.Parse(reader.GetAttribute(COUNT_ATTRIBUTE_NAME));
                    chromatogramList = new ChromatogramList(count, dataProcessing);
                }
                else
                {
                    throw new FatalRuntimeParseException(new InvalidMzMLIssue("Can't find defaultDataProcessingRef '" + defaultDataProcessingRef + "' referenced by chromatogramList."));
                }
            }
            else
            {
                // msconvert doesn't include default data processing so try and fix it
                throw new FatalRuntimeParseException(new InvalidMzMLIssue("No defaultProcessingRef attribute in chromatogramList."));
            }

            try
            {
                run.SetChromatogramList(chromatogramList);
                contentStack.Push(chromatogramList);
            }
            catch (Exception ex)
            {
                throw new FatalRuntimeParseException(new InvalidMzMLIssue("<run> tag not defined prior to defining <chromatogramList> tag.", ex.Message), ex);
            }
        }

        protected void StartChromatogram(XmlReader reader)
        {
            processingChromatogram = true;
            string id = reader.GetAttribute(ID_ATTRIBUTE_NAME);
            int defaultArrayLength = int.Parse(reader.GetAttribute("defaultArrayLength"));
            currentChromatogram = new Chromatogram(id, defaultArrayLength);

            string dataProcessingRef = reader.GetAttribute("dataProcessingRef");

            if (!string.IsNullOrEmpty(dataProcessingRef))
            {
                DataProcessing dataProcessing;

                try
                {
                    dataProcessing = dataProcessingList.GetDataProcessing(dataProcessingRef);
                }
                catch (Exception ex)
                {
                    throw new FatalRuntimeParseException(new InvalidMzMLIssue("<dataProcessingList> tag not defined prior to defining <chromatogram> tag.", ex.Message), ex);
                }

                if (dataProcessing != null)
                {
                    currentChromatogram.SetDataProcessingRef(dataProcessing);
                }
                else
                {
                    throw new FatalRuntimeParseException(new InvalidMzMLIssue("Can't find dataProcessingRef '" + dataProcessingRef + "' referenced by chromatogram '" + currentChromatogram.GetID() + "'."));
                }
            }
            else
            {
                currentChromatogram.SetDataProcessingRef(chromatogramList.GetDefaultDataProcessingRef());
            }

            try
            {
                chromatogramList.AddChromatogram(currentChromatogram);
            }
            catch (Exception ex)
            {
                throw new FatalRuntimeParseException(new InvalidMzMLIssue("<chromatogramList> tag not defined prior to defining <chromatogram> tag.", ex.Message), ex);
            }

            contentStack.Push(currentChromatogram);
        }

        protected virtual void StartUserParam(XmlReader reader)
        {
            if (contentStack.Count > 0)
            {
                string name = reader.GetAttribute("name");
                UserParam userParam = new(name);

                string type = reader.GetAttribute("type");
                if (!string.IsNullOrEmpty(type))
                {
                    userParam.SetType(type);
                }
                string value = reader.GetAttribute(VALUE_ATTRIBUTE_NAME);
                if (!string.IsNullOrEmpty(value))
                {
                    userParam.SetValue(value);
                }

                string unitAccession = reader.GetAttribute(UNIT_ACCESSION_ATTRIBUTE_NAME);
                userParam.SetUnits(obo.GetTerm(unitAccession));

                ((MzMLContentWithParams)contentStack.Peek()).AddUserParam(userParam);
            }
        }

        protected virtual void StartMzML(XmlReader reader)
        {
            MzML mzML = new(reader.GetAttribute("version"));

            mzML.SetDataStorage(dataStorage);
            mzML.SetOBO(obo);

            // Add optional attributes
            string accession = reader.GetAttribute(ACCESSION_ATTRIBUTE_NAME);
            if (!string.IsNullOrEmpty(accession))
            {
                mzML.SetAccession(accession);
            }

            string id = reader.GetAttribute(ID_ATTRIBUTE_NAME);
            if (!string.IsNullOrEmpty(id))
            {
                mzML.SetID(id);
            }

            contentStack.Push(mzML);
        }

        protected void StartCVList(XmlReader reader)
        {
            //int count = int.Parse(reader.GetAttribute(COUNT_ATTRIBUTE_NAME));
            int count = reader.GetAttribute(COUNT_ATTRIBUTE_NAME) != null ? int.Parse(reader.GetAttribute(COUNT_ATTRIBUTE_NAME)) : 0;
            cvList = new CVList(count);

            try
            {
                mzML.SetCVList(cvList);
            }
            catch (Exception ex)
            {
                throw new FatalRuntimeParseException(new InvalidMzMLIssue("<mzML> tag not defined prior to defining <cvList> tag.", ex.Message), ex);
            }

            contentStack.Push(cvList);
        }

        protected void StartCV(XmlReader reader)
        { 
            OBO childOBO = OBO.GetOBO().GetOBOWithID(reader.GetAttribute(ID_ATTRIBUTE_NAME));           

            if (childOBO != null)
            {
                CV cv = new(childOBO);

                cvList.Add(cv);

                contentStack.Push(cv);
            }
            else
            {
                contentStack.Push(null);
                LOGGER.ErrorFormat("WEIRD ONTOLOGY FOUND! {0}", reader.GetAttribute(ID_ATTRIBUTE_NAME));
            }
        }

        protected void StartFileContent()
        {
            FileContent fc = new();

            try
            {
                fileDescription.SetFileContent(fc);
            }
            catch (Exception ex)
            {
                throw new FatalRuntimeParseException(new InvalidMzMLIssue("<fileDescription> tag not defined prior to defining <fileContent> tag.", ex.Message), ex);
            }

            contentStack.Push(fc);
        }

        protected void StartSourceFileList(XmlReader reader)
        {
            int count = int.Parse(reader.GetAttribute(COUNT_ATTRIBUTE_NAME));
            sourceFileList = new SourceFileList(count);

            try
            {
                fileDescription.SetSourceFileList(sourceFileList);
            }
            catch (Exception ex)
            {
                throw new FatalRuntimeParseException(new InvalidMzMLIssue("<fileDescription> tag not defined prior to defining <sourceFileList> tag.", ex.Message), ex);
            }

            contentStack.Push(sourceFileList);
        }

        protected void StartSourceFile(XmlReader reader)
        {
            string id = reader.GetAttribute(ID_ATTRIBUTE_NAME);
            string location = reader.GetAttribute("location");
            string name = reader.GetAttribute("name");
            SourceFile sf = new(id, location, name);

            try
            {
                sourceFileList.AddSourceFile(sf);
            }
            catch (Exception ex)
            {
                throw new FatalRuntimeParseException(new InvalidMzMLIssue("<sourceFileList> tag not defined prior to defining <sourceFile> tag.", ex.Message), ex);
            }

            contentStack.Push(sf);
        }

        protected void StartContact()
        {
            Contact contact = new();

            try
            {
                fileDescription.AddContact(contact);
            }
            catch (Exception ex)
            {
                throw new FatalRuntimeParseException(new InvalidMzMLIssue("<fileDescription> tag not defined prior to defining <contact> tag.", ex.Message), ex);
            }

            contentStack.Push(contact);
        }

        protected void StartReferenceableParamGroupList(XmlReader reader)
        {
            int count = int.Parse(reader.GetAttribute(COUNT_ATTRIBUTE_NAME));
            referenceableParamGroupList = new ReferenceableParamGroupList(count);

            try
            {
                mzML.SetReferenceableParamGroupList(referenceableParamGroupList);
            }
            catch (Exception ex)
            {
                throw new FatalRuntimeParseException(new InvalidMzMLIssue("<mzML> tag not defined prior to defining <referenceableParamGroupList> tag.", ex.Message), ex);
            }

            contentStack.Push(referenceableParamGroupList);
        }

        protected void StartReferenceableParamGroup(XmlReader reader)
        {
            string id = reader.GetAttribute(ID_ATTRIBUTE_NAME);
            ReferenceableParamGroup rpg = new(id);
            contentStack.Push(rpg);

            referenceableParamGroupList.AddReferenceableParamGroup(rpg);
        }

        protected void StartSampleList(XmlReader reader)
        {
            int count = int.Parse(reader.GetAttribute(COUNT_ATTRIBUTE_NAME));
            sampleList = new SampleList(count);

            try
            {
                mzML.SetSampleList(sampleList);
            }
            catch (Exception ex)
            {
                throw new FatalRuntimeParseException(new InvalidMzMLIssue("<mzML> tag not defined prior to defining <sampleList> tag.", ex.Message), ex);
            }

            contentStack.Push(sampleList);
        }

        protected void StartSample(XmlReader reader)
        {
            string id = reader.GetAttribute(ID_ATTRIBUTE_NAME);
            Sample sample = new(id);

            string name = reader.GetAttribute("name");
            if (!string.IsNullOrEmpty(name))
            {
                sample.SetName(name);
            }

            try
            {
                sampleList.AddSample(sample);
            }
            catch (Exception ex)
            {
                throw new FatalRuntimeParseException(new InvalidMzMLIssue("<sampleList> tag not defined prior to defining <sample> tag.", ex.Message), ex);
            }

            contentStack.Push(sample);
        }

        protected void StartSoftwareList(XmlReader reader)
        {
            int count = int.Parse(reader.GetAttribute(COUNT_ATTRIBUTE_NAME));
            softwareList = new SoftwareList(count);

            try
            {
                mzML.SetSoftwareList(softwareList);
            }
            catch (Exception ex)
            {
                throw new FatalRuntimeParseException(new InvalidMzMLIssue("<mzML> tag not defined prior to defining <softwareList> tag.", ex.Message), ex);
            }

            contentStack.Push(softwareList);
        }

        protected void StartSoftware(XmlReader reader)
        {
            string id = reader.GetAttribute(ID_ATTRIBUTE_NAME);
            string version = reader.GetAttribute("version");
            Software sw = new(id, version);

            try
            {
                softwareList.AddSoftware(sw);
            }
            catch (Exception ex)
            {
                throw new FatalRuntimeParseException(new InvalidMzMLIssue("<softwareList> tag not defined prior to defining <software> tag.", ex.Message), ex);
            }
            contentStack.Push(sw);
        }

        protected void StartScanSettingsList(XmlReader reader)
        {
            int count = int.Parse(reader.GetAttribute(COUNT_ATTRIBUTE_NAME));
            scanSettingsList = new ScanSettingsList(count);

            try
            {
                mzML.SetScanSettingsList(scanSettingsList);
            }
            catch (Exception ex)
            {
                throw new FatalRuntimeParseException(new InvalidMzMLIssue("<mzML> tag not defined prior to defining <scanSettingsList> tag.", ex.Message), ex);
            }

            contentStack.Push(scanSettingsList);
        }

        protected void StartScanSettings(XmlReader reader)
        {
            string id = reader.GetAttribute(ID_ATTRIBUTE_NAME);
            currentScanSettings = new ScanSettings(id);

            try
            {
                scanSettingsList.AddScanSettings(currentScanSettings);
            }
            catch (Exception ex)
            {
                throw new FatalRuntimeParseException(new InvalidMzMLIssue("<scanSettingsList> tag not defined prior to defining <scanSettings> tag.", ex.Message), ex);
            }

            contentStack.Push(currentScanSettings);
        }

        protected void StartSourceFileRefList(XmlReader reader)
        {
            int count = int.Parse(reader.GetAttribute(COUNT_ATTRIBUTE_NAME));
            currentSourceFileRefList = new SourceFileRefList(count);

            try
            {
                currentScanSettings.SetSourceFileRefList(currentSourceFileRefList);
            }
            catch (Exception ex)
            {
                throw new FatalRuntimeParseException(new InvalidMzMLIssue("<scanSettings> tag not defined prior to defining <sourceFileRefList> tag.", ex.Message), ex);
            }

            contentStack.Push(currentSourceFileRefList);
        }

        protected void StartSourceFileRef(XmlReader reader)
        {
            string refId = reader.GetAttribute("ref");

            bool foundReference = false;

            if (sourceFileList != null)
            {
                SourceFile sourceFile = sourceFileList.GetSourceFile(refId);

                if (sourceFile != null)
                {
                    currentSourceFileRefList.AddSourceFileRef(new SourceFileRef(sourceFile));
                    foundReference = true;
                }
            }

            if (!foundReference)
            {
                MissingReferenceIssue missingRefIssue = new(reader.GetAttribute("ref"), "sourceFileRef", "ref");
                missingRefIssue.SetIssueLocation(contentStack.Peek());
                missingRefIssue.FixAttemptedByRemovingReference();

                NotifyParserListeners(missingRefIssue);
            }
        }
        
        protected void StartTargetList(XmlReader reader)
        {
            int count = int.Parse(reader.GetAttribute(COUNT_ATTRIBUTE_NAME));
            currentTargetList = new TargetList(count);

            try
            {
                currentScanSettings.SetTargetList(currentTargetList);
            }
            catch (Exception ex)
            {
                throw new FatalRuntimeParseException(new InvalidMzMLIssue("<scanSettings> tag not defined prior to defining <targetList> tag.", ex.Message), ex);
            }

            contentStack.Push(currentTargetList);
        }

        protected void StartTarget()
        {
            Target target = new();

            try
            {
                currentTargetList.AddTarget(target);
            }
            catch (Exception ex)
            {
                throw new FatalRuntimeParseException(new InvalidMzMLIssue("<targetList> tag not defined prior to defining <target> tag.", ex.Message), ex);
            }

            contentStack.Push(target);
        }

        protected void StartInstrumentConfigurationList(XmlReader reader)
        {
            int count = GetCountAttribute(reader);
            instrumentConfigurationList = new InstrumentConfigurationList(count);

            try
            {
                mzML.SetInstrumentConfigurationList(instrumentConfigurationList);
            }
            catch (Exception ex)
            {
                throw new FatalRuntimeParseException(new InvalidMzMLIssue("<mzML> tag not defined prior to defining <instrumentConfigurationList> tag.", ex.Message), ex);
            }

            contentStack.Push(instrumentConfigurationList);
        }

        protected void StartComponentList()
        {
            currentComponentList = [];

            try
            {
                currentInstrumentConfiguration.SetComponentList(currentComponentList);
            }
            catch (Exception ex)
            {
                throw new FatalRuntimeParseException(new InvalidMzMLIssue("<instrumentConfiguration> tag not defined prior to defining <componentList> tag.", ex.Message), ex);
            }

            contentStack.Push(currentComponentList);
        }

        protected void StartSource()
        {
            Source source = new();

            try
            {
                currentComponentList.AddSource(source);
            }
            catch (Exception ex)
            {
                throw new FatalRuntimeParseException(new InvalidMzMLIssue("<componentList> tag not defined prior to defining <source> tag.", ex.Message), ex);
            }

            contentStack.Push(source);
        }

        public virtual void StartElement(XmlReader reader)
        {
            switch (reader.Name)
            {
                case "cvParam":
                    StartCVParam(reader);
                    break;
                case "referenceableParamGroupRef":
                    StartReferenceableParamGroupRef(reader);
                    break;
                case "userParam":
                    StartUserParam(reader);
                    break;
                case "mzML":
                    StartMzML(reader);
                    break;
                case "cvList":
                    StartCVList(reader);
                    break;
                case "cv":
                    StartCV(reader);
                    break;
                case "fileDescription":
                    fileDescription = new FileDescription();
                    mzML.SetFileDescription(fileDescription);
                    contentStack.Push(fileDescription);
                    break;
                case "fileContent":
                    StartFileContent();
                    break;
                case "sourceFileList":
                    StartSourceFileList(reader);
                    break;
                case "sourceFile":
                    StartSourceFile(reader);
                    break;
                case "contact":
                    StartContact();
                    break;
                case "referenceableParamGroupList":
                    StartReferenceableParamGroupList(reader);
                    break;
                case "referenceableParamGroup":
                    StartReferenceableParamGroup(reader);
                    break;
                case "sampleList":
                    StartSampleList(reader);
                    break;
                case "sample":
                    StartSample(reader);
                    break;
                case "softwareList":
                    StartSoftwareList(reader);
                    break;
                case "software":
                    StartSoftware(reader);
                    break;
                case "scanSettingsList":
                    StartScanSettingsList(reader);
                    break;
                case "scanSettings":
                    StartScanSettings(reader);
                    break;
                case "sourceFileRefList":
                    StartSourceFileRefList(reader);
                    break;
                case "sourceFileRef":
                    StartSourceFileRef(reader);
                    break;
                case "targetList":
                    StartTargetList(reader);
                    break;
                case "target":
                    StartTarget();
                    break;
                case "instrumentConfigurationList":
                    StartInstrumentConfigurationList(reader);
                    break;
                case "instrumentConfiguration":
                    StartInstrumentConfiguration(reader);
                    break;
                case "componentList":
                    StartComponentList();
                    break;
                case "source":
                    StartSource();
                    break;
                case "analyzer":
                    Analyser analyser = new();
                    try
                    {
                        currentComponentList.AddAnalyser(analyser);
                    }
                    catch (NullReferenceException ex)
                    {
                        throw new FatalRuntimeParseException(new InvalidMzMLIssue("<componentList> tag not defined prior to defining <analyser> tag.", ex.Message), ex);
                    }
                    contentStack.Push(analyser);
                    break;
                case "detector":
                    Detector detector = new();
                    try
                    {
                        currentComponentList.AddDetector(detector);
                    }
                    catch (NullReferenceException ex)
                    {
                        throw new FatalRuntimeParseException(new InvalidMzMLIssue("<componentList> tag not defined prior to defining <detector> tag.", ex.Message), ex);
                    }
                    contentStack.Push(detector);
                    break;
                case "softwareRef":
                    string softwareRef = reader.GetAttribute("ref");
                    bool foundReference = false;
                    if (softwareList != null && currentInstrumentConfiguration != null)
                    {
                        Software software = softwareList.GetSoftware(softwareRef);
                        if (software != null)
                        {
                            foundReference = true;
                            currentInstrumentConfiguration.SetSoftwareRef(new SoftwareRef(software));
                        }
                    }

                    if (!foundReference)
                    {
                        MissingReferenceIssue missingRefIssue = new(softwareRef, "softwareRef", "ref");
                        missingRefIssue.SetIssueLocation(contentStack.Peek());
                        missingRefIssue.FixAttemptedByRemovingReference();
                        NotifyParserListeners(missingRefIssue);
                    }
                    break;
                case "dataProcessingList":
                    dataProcessingList = new DataProcessingList(GetCountAttribute(reader));
                    try
                    {
                        mzML.SetDataProcessingList(dataProcessingList);
                    }
                    catch (NullReferenceException ex)
                    {
                        throw new FatalRuntimeParseException(new InvalidMzMLIssue("<mzML> tag not defined prior to defining <dataProcessingList> tag.", ex.Message), ex);
                    }
                    contentStack.Push(dataProcessingList);
                    break;
                case "dataProcessing":
                    DataProcessing dp = new(reader.GetAttribute(ID_ATTRIBUTE_NAME));
                    try
                    {
                        dataProcessingList.AddDataProcessing(dp);
                    }
                    catch (NullReferenceException ex)
                    {
                        throw new FatalRuntimeParseException(new InvalidMzMLIssue("<dataProcessingList> tag not defined prior to defining <dataProcessing> tag.", ex.Message), ex);
                    }

                    currentDataProcessing = dp;
                    contentStack.Push(dp);
                    break;
                case "processingMethod":
                    StartProcessingMethod(reader);
                    break;
                case "run":
                    StartRun(reader);
                    break;
                case "spectrumList":
                    StartSpectrumList(reader);
                    break;
                case "spectrum":
                    StartSpectrum(reader);
                    break;
                case "scanList":
                    currentScanList = new ScanList(int.Parse(reader.GetAttribute(COUNT_ATTRIBUTE_NAME)));

                    try
                    {
                        currentSpectrum.SetScanList(currentScanList);
                    }
                    catch (NullReferenceException)
                    {
                        throw new FatalRuntimeParseException(new InvalidMzMLIssue("<spectrum> tag not defined prior to defining <scanList> tag."));
                    }

                    contentStack.Push(currentScanList);
                    break;
                case "scan":
                    StartScan(reader);
                    break;
                case "scanWindowList":
                    currentScanWindowList = new ScanWindowList(int.Parse(reader.GetAttribute(COUNT_ATTRIBUTE_NAME)));

                    try
                    {
                        currentScan.ScanWindowList = currentScanWindowList;
                    }
                    catch (NullReferenceException ex)
                    {
                        throw new FatalRuntimeParseException(new InvalidMzMLIssue("<scan> tag not defined prior to defining <scanWindowList> tag.", ex.Message), ex);
                    }

                    contentStack.Push(currentScanWindowList);
                    break;
                case "scanWindow":
                    ScanWindow scanWindow = new();
                    try
                    {
                        currentScanWindowList.AddScanWindow(scanWindow);
                    }
                    catch (NullReferenceException ex)
                    {
                        throw new FatalRuntimeParseException(new InvalidMzMLIssue("<scanWindowList> tag not defined prior to defining <scanWindow> tag.", ex.Message), ex);
                    }

                    contentStack.Push(scanWindow);
                    break;
                case "precursorList":
                    currentPrecursorList = new PrecursorList(int.Parse(reader.GetAttribute(COUNT_ATTRIBUTE_NAME)));

                    try
                    {
                        currentSpectrum.SetPrecursorList(currentPrecursorList);
                    }
                    catch (NullReferenceException ex)
                    {
                        throw new FatalRuntimeParseException(new InvalidMzMLIssue("<spectrum> tag not defined prior to defining <precursorList> tag.", ex.Message), ex);
                    }

                    contentStack.Push(currentPrecursorList);
                    break;
                case "precursor":
                    StartPrecursor(reader);
                    break;
                case "isolationWindow":
                    IsolationWindow isolationWindow = new();
                    if (processingPrecursor)
                    {
                        try
                        {
                            currentPrecursor.SetIsolationWindow(isolationWindow);
                        }
                        catch (NullReferenceException ex)
                        {
                            throw new FatalRuntimeParseException(new InvalidMzMLIssue("<precursor> tag not defined prior to defining <isolationWindow> tag.", ex.Message), ex);
                        }
                    }
                    else if (processingProduct)
                    {
                        try
                        {
                            currentProduct.SetIsolationWindow(isolationWindow);
                        }
                        catch (NullReferenceException ex)
                        {
                            throw new FatalRuntimeParseException(new InvalidMzMLIssue("<product> tag not defined prior to defining <isolationWindow> tag.", ex.Message), ex);
                        }
                    }

                    contentStack.Push(isolationWindow);
                    break;
                case "selectedIonList":
                    currentSelectedIonList = new SelectedIonList(int.Parse(reader.GetAttribute(COUNT_ATTRIBUTE_NAME)));

                    try
                    {
                        currentPrecursor.SetSelectedIonList(currentSelectedIonList);
                    }
                    catch (NullReferenceException ex)
                    {
                        throw new FatalRuntimeParseException(new InvalidMzMLIssue("<precursor> tag not defined prior to defining <selectedIonList> tag.", ex.Message), ex);
                    }

                    contentStack.Push(currentSelectedIonList);
                    break;
                case "selectedIon":
                    SelectedIon selectedIon = new();

                    try
                    {
                        currentSelectedIonList.AddSelectedIon(selectedIon);
                    }
                    catch (NullReferenceException ex)
                    {
                        throw new FatalRuntimeParseException(new InvalidMzMLIssue("<selectedIonList> tag not defined prior to defining <selectedIon> tag.", ex.Message), ex);
                    }

                    contentStack.Push(selectedIon);
                    break;
                case "activation":
                    Activation activation = new();

                    try
                    {
                        currentPrecursor.SetActivation(activation);
                    }
                    catch (NullReferenceException ex)
                    {
                        throw new FatalRuntimeParseException(new InvalidMzMLIssue("<precursor> tag not defined prior to defining <activation> tag.", ex.Message), ex);
                    }

                    contentStack.Push(activation);
                    break;
                case "productList":
                    currentProductList = new ProductList(int.Parse(reader.GetAttribute(COUNT_ATTRIBUTE_NAME)));

                    try
                    {
                        currentSpectrum.SetProductList(currentProductList);
                    }
                    catch (NullReferenceException ex)
                    {
                        throw new FatalRuntimeParseException(new InvalidMzMLIssue("<spectrum> tag not defined prior to defining <productList> tag.", ex.Message), ex);
                    }

                    contentStack.Push(currentProductList);
                    break;
                case "product":
                    processingProduct = true;
                    currentProduct = new Product();

                    if (processingSpectrum)
                    {
                        try
                        {
                            currentProductList.AddProduct(currentProduct);
                        }
                        catch (NullReferenceException ex)
                        {
                            throw new FatalRuntimeParseException(new InvalidMzMLIssue("<productList> tag not defined prior to defining <product> tag.", ex.Message), ex);
                        }
                    }
                    else if (processingChromatogram)
                    {
                        try
                        {
                            currentChromatogram.SetProduct(currentProduct);
                        }
                        catch (NullReferenceException ex)
                        {
                            throw new FatalRuntimeParseException(new InvalidMzMLIssue("<chromatogram> tag not defined prior to defining <product> tag.", ex.Message), ex);
                        }
                    }

                    contentStack.Push(currentProduct);
                    break;
                case "binaryDataArrayList":
                    currentBinaryDataArrayList = new BinaryDataArrayList(int.Parse(reader.GetAttribute(COUNT_ATTRIBUTE_NAME)));

                    if (processingSpectrum)
                    {
                        try
                        {
                            currentSpectrum.SetBinaryDataArrayList(currentBinaryDataArrayList);
                        }
                        catch (NullReferenceException ex)
                        {
                            throw new FatalRuntimeParseException(new InvalidMzMLIssue("<spectrum> tag not defined prior to defining <binaryDataArrayList> tag.", ex.Message), ex);
                        }
                    }
                    else if (processingChromatogram)
                    {
                        try
                        {
                            currentChromatogram.SetBinaryDataArrayList(currentBinaryDataArrayList);
                        }
                        catch (NullReferenceException ex)
                        {
                            throw new FatalRuntimeParseException(new InvalidMzMLIssue("<chromatogram> tag not defined prior to defining <binaryDataArrayList> tag.", ex.Message), ex);
                        }
                    }
                    contentStack.Push(currentBinaryDataArrayList);
                    break;
                case "binaryDataArray":
                    StartBinaryDataArray(reader);
                    break;
                case "binary":
                    // Ignore binary data for the header
                    break;
                case "chromatogramList":
                    StartChromatogramList(reader);
                    break;
                case "chromatogram":
                    StartChromatogram(reader);
                    break;
                case "offset":
                case "indexListOffset":
                    previousOffsetIDRef = currentOffsetIDRef;

                    if ("offset".Equals(reader.Name))
                    {
                        this.currentOffsetIDRef = reader.GetAttribute("idRef");
                    }

                    offsetData.Length = 0;
                    processingOffset = true;
                    break;
                case "index":
                    if (reader.GetAttribute("name").Equals("chromatogram"))
                    {
                        this.processingChromatogram = true;
                    }
                    else
                    {
                        this.processingSpectrum = true;
                    }
                    break;
                case "indexedmzML":
                case "IndexList":
                    // Do nothing
                    break;
                default:
                    LOGGER.InfoFormat("No processing for tag <{0}>", reader.Name);
                    break;
            }
        }

        public virtual void Characters(char[] ch, int start, int length)
        {
            if (processingOffset) {
                offsetData.Append(ch, start, length);
            }
        }

        protected MzMLDataContainer GetDataContainer()
        {
            MzMLDataContainer dataContainer;
    
            if (processingSpectrum)
            {
                dataContainer = spectrumList.GetSpectrum(previousOffsetIDRef);

                if (dataContainer == null)
                {
                    dataContainer = spectrumList.GetSpectrum(spectrumList.Size() - 1);
                }
            }
            else
            {
                dataContainer = chromatogramList.GetChromatogram(previousOffsetIDRef);

                if (dataContainer == null)
                {
                    dataContainer = chromatogramList.GetChromatogram(chromatogramList.Size() - 1);
                }
            }
            return dataContainer;
        }

        protected long GetOffset()
        {
            return long.Parse(offsetData.ToString());
        }

        protected void SetDataContainer(MzMLDataContainer dataContainer, long offset)
        {
            if (previousOffset != -1 && openDataStorage && dataContainer != null)
            {
                DataLocation dataLocation = new(dataStorage, previousOffset, (int)(offset - previousOffset));

                dataContainer.SetDataLocation(dataLocation);
            }
        }

        public virtual void EndElement(XmlReader reader) 
        {
            string qName = reader.Name;
            if ("spectrum".Equals(qName))
            {
                processingSpectrum = false;

                // Try and tidy up spectrum
                CVParam cvParam = currentSpectrum.GetCVParamOrChild("MS:1000294");
                if (currentSpectrum.ContainsCVParam(cvParam))
                {
                    ReferenceableParamGroup bestGroup = currentSpectrum.FindBestFittingRPG(referenceableParamGroupList);

                    if (bestGroup == null)
                    {
                        // TODO: Give it a better name
                        bestGroup = new ReferenceableParamGroup();

                        if (referenceableParamGroupList == null)
                        {
                            referenceableParamGroupList = new ReferenceableParamGroupList(1);
                            mzML.SetReferenceableParamGroupList(referenceableParamGroupList);
                        }

                        referenceableParamGroupList.Add(bestGroup);

                        bestGroup.AddCVParam(currentSpectrum.GetCVParamOrChild("MS:1000294"));
                        bestGroup.AddCVParam(currentSpectrum.GetCVParamOrChild("MS:1000511")); // ms level
                        bestGroup.AddCVParam(currentSpectrum.GetCVParamOrChild(Spectrum.SCAN_POLARITY_ID));
                        bestGroup.AddCVParam(currentSpectrum.GetCVParamOrChild("MS:1000525")); // spectrum representation
                    }

                    currentSpectrum.ReplaceCVParamsWithRPG(bestGroup);
                }
            }
            else if ("scan".Equals(qName))
            {
                // Try and tidy up scan
                CVParam cvParam = currentScan.GetCVParamOrChild("MS:1000616");
                if (currentScan.ContainsCVParam(cvParam))
                {
                    ReferenceableParamGroup bestGroup = currentScan.FindBestFittingRPG(referenceableParamGroupList);

                    if (bestGroup == null)
                    {
                        // TODO: Give it a better name
                        bestGroup = new ReferenceableParamGroup();

                        if (referenceableParamGroupList == null)
                        {
                            referenceableParamGroupList = new ReferenceableParamGroupList(1);
                            mzML.SetReferenceableParamGroupList(referenceableParamGroupList);
                        }

                        referenceableParamGroupList.Add(bestGroup);

                        bestGroup.AddCVParam(currentScan.GetCVParamOrChild("MS:1000512"));
                        bestGroup.AddCVParam(currentScan.GetCVParamOrChild("MS:1000616"));
                        bestGroup.AddCVParam(currentScan.GetCVParamOrChild("MS:1000927"));
                    }

                    currentScan.ReplaceCVParamsWithRPG(bestGroup);
                }
            }
            else if ("scanWindow".Equals(qName))
            {
                if (contentStack.Peek() is MzMLContentWithParams)
                {
                    CVParam cvParam = ((MzMLContentWithParams)contentStack.Peek()).GetCVParamOrChild("MS:1000501");

                    if (((MzMLContentWithParams)contentStack.Peek()).ContainsCVParam(cvParam))
                    {
                        ReferenceableParamGroup bestGroup = ((MzMLContentWithParams)contentStack.Peek()).FindBestFittingRPG(referenceableParamGroupList);

                        if (bestGroup == null)
                        {
                            // TODO: Give it a better name
                            bestGroup = new ReferenceableParamGroup();

                            if (referenceableParamGroupList == null)
                            {
                                referenceableParamGroupList = new ReferenceableParamGroupList(1);
                                mzML.SetReferenceableParamGroupList(referenceableParamGroupList);
                            }

                            referenceableParamGroupList.Add(bestGroup);

                            bestGroup.AddCVParam(((MzMLContentWithParams)contentStack.Peek()).GetCVParamOrChild("MS:1000501"));
                            bestGroup.AddCVParam(((MzMLContentWithParams)contentStack.Peek()).GetCVParamOrChild("MS:1000500"));
                        }

                        ((MzMLContentWithParams)contentStack.Peek()).ReplaceCVParamsWithRPG(bestGroup);
                    }
                }
            }
            else if ("chromatogram".Equals(qName))
            {
                processingChromatogram = false;
            }
            else if ("precursor".Equals(qName))
            {
                processingPrecursor = false;
            }
            else if ("product".Equals(qName))
            {
                processingProduct = false;
            }
            else if ("offset".Equals(qName) || "indexListOffset".Equals(qName))
            {
                long offset = GetOffset();

                MzMLDataContainer dataContainer = GetDataContainer();

                if (processingSpectrum && processingChromatogram)
                {
                    processingSpectrum = false;
                }

                SetDataContainer(dataContainer, offset);

                previousOffset = offset;
                processingOffset = false;
            }
            else if ("mzML".Equals(qName) || "indexedmzML".Equals(qName))
            {
                // Go through spectrumList and chromatogramList to convert the data storage if necessary
                if (spectrumList != null)
                {
                    foreach (Spectrum spectrum in spectrumList)
                    {
                        try
                        {
                            spectrum.EnsureLoadableData();
                        }
                        catch (IOException ex)
                        {
                            LOGGER.Error(null, ex);
                        }
                    }
                }
            }

            if (!qName.Equals("indexedmzML") && !qName.Equals("cvParam") && !qName.Equals("userParam") && !qName.Equals("softwareRef")
                && !qName.Equals("binary") && !qName.Equals("referenceableParamGroupRef") && !qName.Equals("sourceFileRef")
                && !qName.Equals("index") && !qName.Equals("IndexList") && !qName.Equals("offset") && !qName.Equals("indexListOffset")
                && !qName.Equals("fileChecksum"))
            {
                contentStack.Pop();
            }                    
        }

        public MzML GetMzML()
        {
            return mzML;
        }

    }
}
