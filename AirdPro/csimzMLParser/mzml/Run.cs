using AirdPro.csimzMLParser.exceptions;
using AirdPro.csimzMLParser.util;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace AirdPro.csimzMLParser.mzml
{
    public class Run : MzMLContentWithParams, IReferenceableTag
    {
        private static readonly long serialVersionUID = 1L;

        public static readonly string RUN_ATTRIBUTE_ID = "MS:1000857";

        public InstrumentConfiguration defaultInstrumentConfigurationRef;
        public SourceFile defaultSourceFileRef;
        public string id;
        public Sample sampleRef;
        public DateTime startTimeStamp;
        public IReferenceList<DataProcessing> dataProcessingList;
        public SpectrumList spectrumList;
        public ChromatogramList chromatogramList;

        public Run(string id, InstrumentConfiguration defaultInstrumentConfigurationRef)
        {
            this.id = id;
            this.defaultInstrumentConfigurationRef = defaultInstrumentConfigurationRef;
        }

        public Run(Run run, ReferenceableParamGroupList rpgList, InstrumentConfigurationList icList,
                SourceFileList sourceFileList, SampleList sampleList, DataProcessingList dpList) : base(run, rpgList)
        {
            this.id = run.id;

            if (run.startTimeStamp != null)
            {
                startTimeStamp = (DateTime)run.startTimeStamp;
            }

            if (run.defaultInstrumentConfigurationRef != null && icList != null)
            {
                foreach (InstrumentConfiguration ic in icList)
                {
                    if (run.defaultInstrumentConfigurationRef.id == ic.id)
                    {
                        defaultInstrumentConfigurationRef = ic;
                        break;
                    }
                }
            }

            if (run.defaultSourceFileRef != null && sourceFileList != null)
            {
                foreach (SourceFile sourceFile in sourceFileList)
                {
                    if (run.defaultSourceFileRef.id == sourceFile.id)
                    {
                        defaultSourceFileRef = sourceFile;
                        break;
                    }
                }
            }

            if (run.sampleRef != null && sampleList != null)
            {
                foreach (Sample sample in sampleList)
                {
                    if (run.sampleRef.id == sample.id)
                    {
                        sampleRef = sample;
                        break;
                    }
                }
            }

            if (run.spectrumList != null)
            {
                spectrumList = new SpectrumList(run.spectrumList, rpgList, dpList, sourceFileList, icList);
            }
            if (run.chromatogramList != null)
            {
                chromatogramList = new ChromatogramList(run.chromatogramList, rpgList, dpList, sourceFileList);
            }
        }

        public void SetDataProcessingList(IReferenceList<DataProcessing> dataProcessingList)
        {
            this.dataProcessingList = dataProcessingList;

            if (spectrumList != null)
                spectrumList.SetDataProcessingList(dataProcessingList);

            if (chromatogramList != null)
                chromatogramList.SetDataProcessingList(dataProcessingList);
        }

        public void SetDefaultSourceFileRef(SourceFile defaultSourceFileRef)
        {
            this.defaultSourceFileRef = defaultSourceFileRef;
        }

        public SourceFile GetDefaultSourceFileRef()
        {
            return defaultSourceFileRef;
        }

        public string GetID()
        {
            return id;
        }

        public void SetSampleRef(Sample sampleRef)
        {
            this.sampleRef = sampleRef;
        }

        public void SetStartTimeStamp(DateTime startTimeStamp)
        {
            this.startTimeStamp = startTimeStamp;
        }

        public void SetSpectrumList(SpectrumList spectrumList)
        {
            spectrumList.SetParent(this);

            this.spectrumList = spectrumList;
            this.spectrumList.SetDataProcessingList(dataProcessingList);
        }

        public InstrumentConfiguration GetDefaultInstrumentConfiguration()
        {
            return defaultInstrumentConfigurationRef;
        }

        public SpectrumList GetSpectrumList()
        {
            return spectrumList;
        }

        public void SetChromatogramList(ChromatogramList chromatogramList)
        {
            this.chromatogramList = chromatogramList;

            if (chromatogramList != null)
            {
                chromatogramList.SetParent(this);

                this.chromatogramList.SetDataProcessingList(dataProcessingList);
            }
        }

        public ChromatogramList GetChromatogramList()
        {
            return chromatogramList;
        }

        public override void AddTagSpecificElementsAtXPathToCollection(ICollection<IMzMLTag> elements, string fullXPath, string currentXPath)
        {
            if (currentXPath.StartsWith("/spectrumList"))
            {
                if (spectrumList == null)
                {
                    throw new UnfollowableXPathException("No spectrumList exists, so cannot go to " + fullXPath, fullXPath, currentXPath);
                }

                spectrumList.AddElementsAtXPathToCollection(elements, fullXPath, currentXPath);
            }
            else if (currentXPath.StartsWith("/chromatogramList"))
            {
                if (chromatogramList == null)
                {
                    throw new UnfollowableXPathException("No chromatogramList exists, so cannot go to " + fullXPath, fullXPath, currentXPath);
                }

                chromatogramList.AddElementsAtXPathToCollection(elements, fullXPath, currentXPath);
            }
        }

        public override string GetXMLAttributeText()
        {
            string attributes = $"defaultInstrumentConfigurationRef=\"{XMLHelper.EnsureSafeXML(defaultInstrumentConfigurationRef.GetID())}\"";

            if (defaultSourceFileRef != null)
            {
                attributes += $" defaultSourceFileRef=\"{XMLHelper.EnsureSafeXML(defaultSourceFileRef.GetID())}\"";
            }

            attributes += $" id=\"{XMLHelper.EnsureSafeXML(id)}\"";

            if (sampleRef != null)
            {
                attributes += $" sampleRef=\"{XMLHelper.EnsureSafeXML(sampleRef.GetID())}\"";
            }
            if (startTimeStamp != null)
            {
                // 定义日期时间格式
                string xmlDateTimeFormat = "yyyy-MM-dd'T'HH:mm:ss";
                // 格式化日期时间，包括转换为 UTC
                DateTimeOffset startTimeStampOffset = new(startTimeStamp.ToUniversalTime());
                string formattedDateWithTimeZone = startTimeStampOffset.ToString(xmlDateTimeFormat, CultureInfo.InvariantCulture);
                
                attributes = $" startTimeStamp=\"{formattedDateWithTimeZone}\"";
            }
            return attributes;
        }

        public override string GetTagName()
        {
            return "run";
        }

        public override void AddChildrenToCollection(ICollection<IMzMLTag> children)
        {
            base.AddChildrenToCollection(children);
            if (spectrumList != null)
            {
                children.Add(spectrumList);
            }

            if (chromatogramList != null)
            {
                children.Add(chromatogramList);
            }
        }

        public void SetID(string id)
        {
            this.id = id;
        }

    } 
}
