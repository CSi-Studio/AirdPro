using AirdPro.csimzMLParser.data;
using AirdPro.csimzMLParser.exceptions;
using AirdPro.csimzMLParser.obo;
using AirdPro.csimzMLParser.util;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;

namespace AirdPro.csimzMLParser.mzml
{
    [Serializable]
    public class MzML : MzMLContentWithParams
    {
        private static readonly long serialVersionUID = 1L;

        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(MzML));

        public const string NAMESPACE = "http://psi.hupo.org/ms/mzml";
        public const string XSI = "http://www.w3.org/2001/XMLSchema-instance";
        public const string SCHEMA_LOCATION = "http://psi.hupo.org/ms/mzml http://psidev.info/files/ms/mzML/xsd/mzML1.1.0.xsd";
        public const string IDX_SCHEMA_LOCATION = "http://psi.hupo.org/ms/mzml http://psidev.info/files/ms/mzML/xsd/mzML1.1.2_idx.xsd";
        public const string CURRENT_VERSION = "1.1.0";
        
        public DataStorage dataStorage;

        public string accession;
        public string id;
        public string version;

        public CVList cvList;
        public FileDescription fileDescription;
        public ReferenceableParamGroupList referenceableParamGroupList;
        public SampleList sampleList;
        public SoftwareList softwareList;
        public ScanSettingsList scanSettingsList;
        public InstrumentConfigurationList instrumentConfigurationList;
        public DataProcessingList dataProcessingList;
        public Run run;
        public OBO obo;

        public MzML(string version)
        {
            this.version = version;
        }

        public MzML(MzML mzML)
        {
            accession = mzML.accession;
            id = mzML.id;
            version = mzML.version;
            obo = mzML.obo;

            if (mzML.referenceableParamGroupList != null)
            {
                referenceableParamGroupList = new ReferenceableParamGroupList(mzML.referenceableParamGroupList);
            }

            cvList = new CVList(mzML.cvList);
            fileDescription = new FileDescription(mzML.fileDescription, referenceableParamGroupList);

            if (mzML.sampleList != null)
            {
                sampleList = new SampleList(mzML.sampleList, referenceableParamGroupList);
            }

            softwareList = new SoftwareList(mzML.softwareList, referenceableParamGroupList);

            if (mzML.scanSettingsList != null)
            {
                scanSettingsList = new ScanSettingsList(mzML.scanSettingsList, referenceableParamGroupList, fileDescription.sourceFileList);
            }

            instrumentConfigurationList = new InstrumentConfigurationList(mzML.instrumentConfigurationList, referenceableParamGroupList, scanSettingsList, softwareList);
            dataProcessingList = new DataProcessingList(mzML.dataProcessingList, referenceableParamGroupList, softwareList);
            run = new Run(mzML.run, referenceableParamGroupList, instrumentConfigurationList, fileDescription.sourceFileList, sampleList, dataProcessingList);
        }

        //private readonly object lockObject = new();

        public void SetDataStorage(DataStorage dataStorage)
        {
            this.dataStorage = dataStorage;
            /*lock (lockObject)
            {
                this.dataStorage = dataStorage;
            }*/
        }

        public void SetOBO(OBO obo)
        {
            this.obo = obo;
        }

        public OBO GetOBO()
        {
            return obo;
        }

        public void SetVersion(string version)
        {
            this.version = version;
        }

        public string GetVersion()
        {
            return version;
        }

        public void SetAccession(string accession)
        {
            this.accession = accession;
        }

        public string Accession()
        {
            return accession;
        }

        public void SetID(string id)
        {
            this.id = id;
        }

        public string GetID()
        {
            return id;
        }

        public void AddSpectrum(Spectrum spectrum)
        {
            run.spectrumList.Add(spectrum);
        }

        public SpectrumList GetSpectrumList()
        {
            return run.spectrumList;
        }

        public ChromatogramList GetChromatogramList()
        {
            return run.chromatogramList;
        }

        public void SetCVList(CVList cvList)
        {
            cvList.SetParent(this);

            this.cvList = cvList;
        }

        public CVList GetCVList()
        {
            return cvList;
        }

        public void SetFileDescription(FileDescription fileDescription)
        {
            fileDescription.SetParent(this);

            this.fileDescription = fileDescription;
        }

        public FileDescription GetFileDescription()
        {
            return fileDescription;
        }

        public void SetReferenceableParamGroupList(ReferenceableParamGroupList referenceableParamGroupList)
        {
            referenceableParamGroupList.SetParent(this);

            this.referenceableParamGroupList = referenceableParamGroupList;
        }

        public ReferenceableParamGroupList GetReferenceableParamGroupList()
        {
            if (referenceableParamGroupList == null)
            {
                referenceableParamGroupList = new ReferenceableParamGroupList(0);
            }

            return referenceableParamGroupList;
        }

        public void SetSampleList(SampleList sampleList)
        {
            sampleList.SetParent(this);

            this.sampleList = sampleList;
        }

        public SampleList GetSampleList()
        {
            if (sampleList == null)
            {
                sampleList = new SampleList(0);
                sampleList.SetParent(this);
            }

            return sampleList;
        }

        public void SetSoftwareList(SoftwareList softwareList)
        {
            softwareList.SetParent(this);

            this.softwareList = softwareList;
        }

        public SoftwareList GetSoftwareList()
        {
            if (softwareList == null)
            {
                softwareList = new SoftwareList(0);
            }

            return softwareList;
        }

        public void SetScanSettingsList(ScanSettingsList scanSettingsList)
        {
            scanSettingsList.SetParent(this);

            this.scanSettingsList = scanSettingsList;
        }

        public ScanSettingsList GetScanSettingsList()
        {
            if (scanSettingsList == null)
            {
                scanSettingsList = new ScanSettingsList(0);
            }

            return scanSettingsList;
        }

        public void SetInstrumentConfigurationList(InstrumentConfigurationList instrumentConfigurationList)
        {
            instrumentConfigurationList.SetParent(this);

            this.instrumentConfigurationList = instrumentConfigurationList;
        }

        public InstrumentConfigurationList GetInstrumentConfigurationList()
        {
            if (instrumentConfigurationList == null)
            {
                instrumentConfigurationList = new InstrumentConfigurationList(0);
            }

            return instrumentConfigurationList;
        }

        public override string GetTagName()
        {
            return "mzML";
        }

        public override void AddChildrenToCollection(ICollection<IMzMLTag> children)
        {
            if (cvList != null)
                children.Add(cvList);
            if (fileDescription != null)
                children.Add(fileDescription);
            if (referenceableParamGroupList != null)
                children.Add(referenceableParamGroupList);
            if (sampleList != null)
                children.Add(sampleList);
            if (softwareList != null)
                children.Add(softwareList);
            if (scanSettingsList != null)
                children.Add(scanSettingsList);
            if (instrumentConfigurationList != null)
                children.Add(instrumentConfigurationList);
            if (dataProcessingList != null)
                children.Add(dataProcessingList);
            if (run != null)
                children.Add(run);

            base.AddChildrenToCollection(children);
        }

        public override void AddTagSpecificElementsAtXPathToCollection(ICollection<IMzMLTag> elements, string fullXPath, string currentXPath)
        {
            if (currentXPath.StartsWith("/" + cvList.GetTagName())) 
            {
                cvList.AddElementsAtXPathToCollection(elements, fullXPath, currentXPath);
            } else if (currentXPath.StartsWith("/fileDescription")) {
                fileDescription.AddElementsAtXPathToCollection(elements, fullXPath, currentXPath);
            } 
            else if (currentXPath.StartsWith("/referenceableParamGroupList"))
            {
                if (referenceableParamGroupList == null)
                {
                    throw new UnfollowableXPathException("No referenceableParamGroupList exists, so cannot go to " + fullXPath, fullXPath, currentXPath);
                }
                referenceableParamGroupList.AddElementsAtXPathToCollection(elements, fullXPath, currentXPath);
            }
            else if (currentXPath.StartsWith("/sampleList"))
            {
                if (sampleList == null)
                {
                    throw new UnfollowableXPathException("No sampleList exists, so cannot go to " + fullXPath, fullXPath, currentXPath);
                }
                sampleList.AddElementsAtXPathToCollection(elements, fullXPath, currentXPath);
            }
            else if (currentXPath.StartsWith("/softwareList"))
            {
                softwareList.AddElementsAtXPathToCollection(elements, fullXPath, currentXPath);
            }
            else if (currentXPath.StartsWith("/scanSettingsList"))
            {
                if (scanSettingsList != null)
                {
                    scanSettingsList.AddElementsAtXPathToCollection(elements, fullXPath, currentXPath);
                }
            }
            else if (currentXPath.StartsWith("/instrumentConfigurationList"))
            {
                instrumentConfigurationList.AddElementsAtXPathToCollection(elements, fullXPath, currentXPath);
            }
            else if (currentXPath.StartsWith("/dataProcessingList"))
            {
                dataProcessingList.AddElementsAtXPathToCollection(elements, fullXPath, currentXPath);
            }
            else if (currentXPath.StartsWith("/run"))
            {
                run.AddElementsAtXPathToCollection(elements, fullXPath, currentXPath);
            }
        }

        public void SetDataProcessingList(DataProcessingList dataProcessingList)
        {
            dataProcessingList.SetParent(this);

            this.dataProcessingList = dataProcessingList;

            if (run != null)
                run.SetDataProcessingList(dataProcessingList);
        }

        public DataProcessingList GetDataProcessingList()
        {
            if (dataProcessingList == null)
            {
                dataProcessingList = new DataProcessingList(0);
            }

            return dataProcessingList;
        }

        public void SetRun(Run run)
        {
            run.SetParent(this);

            this.run = run;

            run.SetDataProcessingList(dataProcessingList);
        }

        public Run GetRun()
        {
            return run;
        }

        public override string GetXMLAttributeText()
        {
            string attributeText = "";

            // Set up namespaces
            attributeText += "xmlns:xsi=\"" + XSI + "\"";
            attributeText += " xsi:schemaLocation=\"" + SCHEMA_LOCATION + "\"";
            attributeText += " xmlns=\"" + NAMESPACE + "\"";
            // Attributes
            attributeText += " version=\"" + XMLHelper.EnsureSafeXML(version) + "\"";
            if (accession != null)
            {
                attributeText += " accession=\"" + XMLHelper.EnsureSafeXML(accession) + "\"";
            }
            if (id != null)
            {
                attributeText += " id=\"" + XMLHelper.EnsureSafeXML(id) + "\"";
            }

            return attributeText;
        }
        
        public override string ToString()
        {
            return "mzML";
        }

        public void Close()
        {
            if (dataStorage != null)
            {
                try
                {
                    dataStorage.Close();
                }
                catch (IOException ex)
                {
                    LOGGER.Error("Error closing data storage.", ex);
                }
            }

            SpectrumList spectrumList = run.spectrumList;

            foreach (Spectrum spectrum in spectrumList)
            {
                CloseDataStorage(spectrum.dataLocation);

                foreach (BinaryDataArray bda in spectrum.binaryDataArrayList)
                {
                    CloseDataStorage(bda.GetDataLocation());
                }
            }
        }

        protected static void CloseDataStorage(DataLocation dataLocation)
        {
            if (dataLocation != null)
            {
                DataStorage dataStorage = dataLocation.GetDataStorage();

                if (dataStorage != null)
                {
                    try
                    {
                        dataStorage.Close();
                    }
                    catch (IOException ex)
                    {
                        LOGGER.Error("Failed to close DataStorage", ex);
                    }
                }
            }
        }

        protected static void CreateDefaults(MzML mzML)
        {
            CVList cvList = CVList.Create();
            mzML.SetCVList(cvList);

            FileDescription fd = FileDescription.Create();
            mzML.SetFileDescription(fd);

            SoftwareList softwareList = SoftwareList.Create();
            mzML.SetSoftwareList(softwareList);

            InstrumentConfigurationList icList = InstrumentConfigurationList.Create();
            mzML.SetInstrumentConfigurationList(icList);

            DataProcessingList dpList = DataProcessingList.Create(softwareList.Get(0));
            mzML.SetDataProcessingList(dpList);

            Run run = new Run("run", icList.Get(0));
            mzML.SetRun(run);

            SpectrumList spectrumList = new SpectrumList(0, dpList.Get(0));
            run.SetSpectrumList(spectrumList);
        }

        public static MzML Create()
        {
            MzML mzML = new(CURRENT_VERSION);

            CreateDefaults(mzML);

            return mzML;
        }

    }
}
