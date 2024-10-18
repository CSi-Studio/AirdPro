using AirdPro.csimzMLParser.util;
using System;
using System.Collections.Generic;

namespace AirdPro.csimzMLParser.mzml
{
    [Serializable]
    public class Scan : MzMLContentWithParams
    {
        private static readonly long serialVersionUID = 1L;

        public static readonly string SCAN_ATTRIBUTE_ID = "MS:1000503";
        public static readonly string SCAN_DIRECTION_ID = "MS:1000018";
        public static readonly string SCAN_LAW_ID = "MS:1000019";
        public static readonly string POSITION_X_ID = "IMS:1000050";
        public static readonly string POSITION_Y_ID = "IMS:1000051";
        public static readonly string POSITION_Z_ID = "IMS:1000052";
        public static readonly string ELUTION_TIME_ID = "MS:1000826";
        public static readonly string SCAN_START_TIME_ID = "MS:1000016";
        public static readonly string ION_MOBILITY_DRIFT_TIME_ID = "MS:1002476";
        public static readonly string SCAN_FILTER_STRING_ID = "MS:1000512";
        public static readonly string SCAN_INVERSE_REDUCED_ION_MOBILITY_ID = "MS:1002815";
        public static readonly string SCAN_ION_MOBILITY_DRIFT_TIME_ID = "MS:1002476";
        public static readonly string SCAN_ION_INJECTION_TIME_ID = "MS:1000927";

        private string externalSpectrumID;
        private InstrumentConfiguration instrumentConfigurationRef;
        private SourceFile sourceFileRef;
        private string spectrumRef;
        private ScanWindowList scanWindowList;

        public Scan() : base()
        {
        }

        public Scan(Scan scan, ReferenceableParamGroupList rpgList, InstrumentConfigurationList icList, SourceFileList sourceFileList)
            : base(scan, rpgList)
        {
            externalSpectrumID = scan.externalSpectrumID;
            spectrumRef = scan.spectrumRef;

            if (scan.instrumentConfigurationRef != null && icList != null)
            {
                foreach (InstrumentConfiguration ic in icList)
                {
                    if (scan.instrumentConfigurationRef.id.Equals(ic.id))
                    {
                        instrumentConfigurationRef = ic;
                        break;
                    }
                }
            }

            if (scan.sourceFileRef != null && sourceFileList != null)
            {
                foreach (SourceFile sourceFile in sourceFileList)
                {
                    if (scan.sourceFileRef.id.Equals(sourceFile.id))
                    {
                        sourceFileRef = sourceFile;
                        break;
                    }
                }
            }

            if (scan.scanWindowList != null)
            {
                scanWindowList = new ScanWindowList(scan.scanWindowList, rpgList);
            }
        }

        public string ExternalSpectrumID
        {
            get { return externalSpectrumID; }
            set { externalSpectrumID = value; }
        }

        public InstrumentConfiguration InstrumentConfigurationRef
        {
            get { return instrumentConfigurationRef; }
            set { instrumentConfigurationRef = value; }
        }

        public SourceFile SourceFileRef
        {
            get { return sourceFileRef; }
            set { sourceFileRef = value; }
        }

        public string SpectrumRef
        {
            get { return spectrumRef; }
            set { spectrumRef = value; }
        }

        public ScanWindowList ScanWindowList
        {
            get { return scanWindowList; }
            set
            {
                value.SetParent(this);
                scanWindowList = value;
            }
        }

        public override void AddTagSpecificElementsAtXPathToCollection(ICollection<IMzMLTag> elements, string fullXPath, string currentXPath)
        {
            if (currentXPath.StartsWith("/scanWindowList"))
            {
                if (scanWindowList == null)
                {
                    throw new InvalidOperationException("No scanWindowList exists, so cannot go to " + fullXPath);
                }

                scanWindowList.AddElementsAtXPathToCollection(elements, fullXPath, currentXPath);
            }
        }

        public override string GetXMLAttributeText()
        {
            string attributeText = base.GetXMLAttributeText();

            if (externalSpectrumID != null)
            {
                attributeText += $" externalSpectrumID=\"{XMLHelper.EnsureSafeXML(externalSpectrumID)}\"";
            }
            if (instrumentConfigurationRef != null)
            {
                attributeText += $" instrumentConfigurationRef=\"{XMLHelper.EnsureSafeXML(instrumentConfigurationRef.id)}\"";
            }
            if (sourceFileRef != null)
            {
                attributeText += $" sourceFileRef=\"{XMLHelper.EnsureSafeXML(sourceFileRef.id)}\"";
            }
            if (spectrumRef != null)
            {
                attributeText += $" spectrumRef=\"{XMLHelper.EnsureSafeXML(spectrumRef)}\"";
            }

            if (attributeText.StartsWith(" "))
            {
                attributeText = attributeText.Substring(1);
            }

            return attributeText;
        }

        public override string ToString()
        {
            string description = "scan: ";

            if (externalSpectrumID != null && !string.IsNullOrEmpty(externalSpectrumID))
            {
                description += $" externalSpectrumID=\"{externalSpectrumID}\"";
            }

            if (instrumentConfigurationRef != null)
            {
                description += $" instrumentConfigurationRef=\"{instrumentConfigurationRef.id}\"";
            }

            if (sourceFileRef != null)
            {
                description += $" sourceFileRef=\"{sourceFileRef.id}\"";
            }

            if (spectrumRef != null && !string.IsNullOrEmpty(spectrumRef))
            {
                description += $" spectrumRef=\"{spectrumRef}\"";
            }

            return description;
        }

        public override string GetTagName()
        {
            return "scan";
        }

        public override void AddChildrenToCollection(ICollection<IMzMLTag> children)
        {
            base.AddChildrenToCollection(children);

            if (scanWindowList != null)
            {
                children.Add(scanWindowList);
            }
        }

        public static Scan Create()
        {
            return new Scan();
        }
    }
}
