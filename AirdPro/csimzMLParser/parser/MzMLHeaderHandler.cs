using AirdPro.csimzMLParser.data;
using AirdPro.csimzMLParser.exceptions;
using AirdPro.csimzMLParser.mzml;
using AirdPro.csimzMLParser.obo;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace AirdPro.csimzMLParser.parser
{
    public class MzMLHeaderHandler : IDisposable
    {
        public static readonly string ACCESSION_ATTRIBUTE_NAME = "accession";
        public static readonly string VALUE_ATTRIBUTE_NAME = "value";
        private static readonly string UNIT_ACCESSION_ATTRIBUTE_NAME = "unitAccession";
        public static readonly string ID_ATTRIBUTE_NAME = "id";
        private static readonly string COUNT_ATTRIBUTE_NAME = "count";

        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(MzMLHeaderHandler));
        public event Action<IIssue> ParserListener;

        protected virtual void NotifyParserListeners(IIssue issue)
        {
            ParserListener?.Invoke(issue);
        }

        public OBO obo;
        public MzML mzML;
        public CVList cvList;
        public FileDescription fileDescription;
        public SourceFileList sourceFileList;
        public ReferenceableParamGroupList referenceableParamGroupList;
        public SampleList sampleList;
        public SoftwareList softwareList;
        public ScanSettingsList scanSettingsList;
        public ScanSettings currentScanSettings;
        public SourceFileRefList currentSourceFileRefList;
        public TargetList currentTargetList;
        public InstrumentConfigurationList instrumentConfigurationList;
        public InstrumentConfiguration currentInstrumentConfiguration;
        public ComponentList currentComponentList;
        public DataProcessingList dataProcessingList;
        public DataProcessing currentDataProcessing;
        public Run run;
        public SpectrumList spectrumList;
        public Spectrum currentSpectrum;
        public ScanList currentScanList;
        public Scan currentScan;
        public ScanWindowList currentScanWindowList;
        public PrecursorList currentPrecursorList;
        public Precursor currentPrecursor;
        public SelectedIonList currentSelectedIonList;
        public ProductList currentProductList;
        public Product currentProduct;
        public BinaryDataArrayList currentBinaryDataArrayList;
        public BinaryDataArray currentBinaryDataArray;
        public ChromatogramList chromatogramList;
        public Chromatogram currentChromatogram;
        public Stack<MzMLContent> contentStack = new Stack<MzMLContent>();
        public bool processingSpectrum;
        public bool processingChromatogram;
        public bool processingPrecursor;
        public bool processingProduct;
        public bool processingOffset;
        public StringBuilder offsetData;
        public string previousOffsetIDRef;
        public string currentOffsetIDRef;
        public long previousOffset = -1;
        public DataStorage dataStorage;
        public bool openDataStorage = true;
        public int numberOfSpectra = 0;
        public List<IParserListener> listeners;

        public MzMLHeaderHandler() { }

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

        public MzMLHeaderHandler(OBO obo, FileInfo mzMLFile) : this(obo, mzMLFile, true)
        {
            
        }

        public MzMLHeaderHandler(OBO obo, FileInfo mzMLFile, bool openDataFile) : this(obo)
        {
            if (openDataFile) 
            {
                this.dataStorage = new MzMLSpectrumDataStorage(mzMLFile);
            }
        }

        protected virtual void OnParserListener(IIssue issue)
        {
            LOGGER.Info($"Notifying listeners about the issue: {issue.GetIssueMessage()}");
            ParserListener?.Invoke(issue);
        }       

        public virtual MzML ParseMzML(string filename)
        {
            using (XmlReader reader = XmlReader.Create(filename))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
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
                                StartFileDescription();
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
                                StartAnalyzer();
                                break;
                            case "detector":
                                StartDetector();
                                break;
                            case "dataProcessingList":
                                StartDataProcessingList(reader);
                                break;
                            case "dataProcessing":
                                StartDataProcessing(reader);
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
                            case "chromatogramList":
                                StartChromatogramList(reader);
                                break;
                            case "chromatogram":
                                StartChromatogram(reader);
                                break;
                        }
                    }

                    if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        EndElement(reader);
                    }
                }
            }

            return mzML;
        }

        public virtual void StartCVParam(XmlReader reader)
        {
            if (contentStack.Count > 0)
            {
                string accession = reader.GetAttribute(ACCESSION_ATTRIBUTE_NAME);

                OBOTerm term = obo.GetTerm(accession);

                if (term == null)
                {
                    UserParam userParam = new UserParam(accession, reader.GetAttribute(VALUE_ATTRIBUTE_NAME), obo.GetTerm(reader.GetAttribute(UNIT_ACCESSION_ATTRIBUTE_NAME)));
                    ((MzMLContentWithParams)contentStack.Peek()).AddUserParam(userParam);

                    CVParamAccessionNotFoundIssue notFound = new CVParamAccessionNotFoundIssue(accession, userParam);
                    notFound.SetIssueLocation(contentStack.Peek());
                    NotifyParserListeners(notFound);
                }
                else
                {
                    if (term.IsObsolete())
                    {
                        ObsoleteTermUsed obsoleteIssue = new ObsoleteTermUsed(term);
                        obsoleteIssue.SetIssueLocation(contentStack.Peek());
                        NotifyParserListeners(obsoleteIssue);
                    }

                    try
                    {
                        CVParam.CVParamType paramType = CVParam.GetCVParamType(term);
                        CVParam cvParam;

                        string value = reader.GetAttribute(VALUE_ATTRIBUTE_NAME);
                        OBOTerm units = obo.GetTerm(reader.GetAttribute(UNIT_ACCESSION_ATTRIBUTE_NAME));

                        try
                        {
                            switch (paramType)
                            {
                                case CVParam.CVParamType.STRING:
                                    cvParam = new StringCVParam(term, value, units);
                                    break;
                                case CVParam.CVParamType.EMPTY:
                                    cvParam = new EmptyCVParam(term, units);
                                    if (value != null && value != string.Empty)
                                    {
                                        InvalidFormatIssue invalidFormatIssue = new InvalidFormatIssue(term, value);
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
                                    InvalidFormatIssue formatIssue = new InvalidFormatIssue(term, paramType);
                                    formatIssue.FixAttemptedByChangingType((StringCVParam)cvParam);
                                    formatIssue.SetIssueLocation(contentStack.Peek());
                                    NotifyParserListeners(formatIssue);
                                    break;
                            }
                        }
                        catch (FormatException nfe)
                        {
                            cvParam = new StringCVParam(term, reader.GetAttribute(VALUE_ATTRIBUTE_NAME), obo.GetTerm(reader.GetAttribute(UNIT_ACCESSION_ATTRIBUTE_NAME)));
                            InvalidFormatIssue formatIssue = new InvalidFormatIssue(term, reader.GetAttribute(VALUE_ATTRIBUTE_NAME));
                            formatIssue.FixAttemptedByChangingType((StringCVParam)cvParam);
                            formatIssue.SetIssueLocation(contentStack.Peek());
                            NotifyParserListeners(formatIssue);
                        }

                        if (contentStack.Peek() is MzMLContentWithParams contentWithParams)
                        {
                            contentWithParams.AddCVParam(cvParam);
                        }
                        else
                        {
                            throw new InvalidOperationException("Failure to add CVParam to " + contentStack.Peek());
                        }
                    }
                    catch (NonFatalParseException ex)
                    {
                        NonFatalParseIssue issue = ex.Issue;

                        issue.SetIssueLocation(contentStack.Peek());
                        NotifyParserListeners(issue);
                        LOGGER.Info(issue.GetIssueMessage(), ex);
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("<cvParam> tag without a parent.");
            }
        }

        public virtual void StartReferenceableParamGroupRef(XmlReader reader)
        {
            // Implementation...
        }

        public virtual void StartUserParam(XmlReader reader)
        {
            // Implementation...
        }

        public virtual void StartMzML(XmlReader reader)
        {
            // Implementation...
        }

        public virtual void StartCVList(XmlReader reader)
        {
            // Implementation...
        }

        public virtual void StartCV(XmlReader reader)
        {
            // Implementation...
        }

        public virtual void StartFileDescription()
        {
            // Implementation...
        }

        public virtual void StartSourceFileList(XmlReader reader)
        {
            // Implementation...
        }

        public virtual void StartSourceFile(XmlReader reader)
        {
            // Implementation...
        }

        public virtual void StartContact()
        {
            // Implementation...
        }

        public virtual void StartReferenceableParamGroupList(XmlReader reader)
        {
            // Implementation...
        }

        public virtual void StartReferenceableParamGroup(XmlReader reader)
        {
            // Implementation...
        }

        public virtual void StartSampleList(XmlReader reader)
        {
            // Implementation...
        }

        public virtual void StartSample(XmlReader reader)
        {
            // Implementation...
        }

        public virtual void StartSoftwareList(XmlReader reader)
        {
            // Implementation...
        }

        public virtual void StartSoftware(XmlReader reader)
        {
            // Implementation...
        }

        public virtual void StartScanSettingsList(XmlReader reader)
        {
            // Implementation...
        }

        public virtual void StartScanSettings(XmlReader reader)
        {
            // Implementation...
        }

        public virtual void StartSourceFileRefList(XmlReader reader)
        {
            // Implementation...
        }

        public virtual void StartSourceFileRef(XmlReader reader)
        {
            // Implementation...
        }

        public virtual void StartTargetList(XmlReader reader)
        {
            // Implementation...
        }

        public virtual void StartTarget()
        {
            // Implementation...
        }

        public virtual void StartInstrumentConfigurationList(XmlReader reader)
        {
            // Implementation...
        }

        public virtual void StartInstrumentConfiguration(XmlReader reader)
        {
            // Implementation...
        }

        public virtual void StartComponentList()
        {
            // Implementation...
        }

        public virtual void StartSource()
        {
            // Implementation...
        }

        public virtual void StartAnalyzer()
        {
            // Implementation...
        }

        public virtual void StartDetector()
        {
            // Implementation...
        }

        public virtual void StartDataProcessingList(XmlReader reader)
        {
            // Implementation...
        }

        public virtual void StartDataProcessing(XmlReader reader)
        {
            // Implementation...
        }

        public virtual void StartRun(XmlReader reader)
        {
            // Implementation...
        }

        public virtual void StartSpectrumList(XmlReader reader)
        {
            // Implementation...
        }

        public virtual void StartSpectrum(XmlReader reader)
        {
            // Implementation...
        }

        public virtual void StartChromatogramList(XmlReader reader)
        {
            // Implementation...
        }

        public virtual void StartChromatogram(XmlReader reader)
        {
            // Implementation...
        }

        public virtual void EndElement(XmlReader reader)
        {
            // Implementation...
        }

        public void Dispose()
        {
            // Clean up resources
        }

        
    }
}
