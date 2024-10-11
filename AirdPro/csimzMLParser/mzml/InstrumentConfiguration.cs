using AirdPro.csimzMLParser.util;
using System;
using System.Collections.Generic;

namespace AirdPro.csimzMLParser.mzml
{
    public class InstrumentConfiguration : MzMLContentWithParams, IReferenceableTag
    {
        private const long serialVersionUID = 1L;

        public const string INSTRUMENT_MODEL_ID = "MS:1000031"; // Required child (1)
        public const string INSTRUMENT_ATTRIBUTE_ID = "MS:1000496"; // Optional child (1+)
        public const string ION_OPTICS_TYPE_ID = "MS:1000597"; // Optional child (1)
        public const string ION_OPTICS_ATTRIBUTE_ID = "MS:1000487"; // Optional child (1+)

        public string id; // Required
        public ScanSettings scanSettingsRef; // Optional
        public ComponentList componentList; // Optional
        public SoftwareRef softwareRef; // Optional

        public InstrumentConfiguration(string id)
        {
            this.id = id;
        }

        public InstrumentConfiguration(string id, ScanSettings scanSettingsRef)
        {
            this.id = id;
            this.scanSettingsRef = scanSettingsRef;
        }

        public InstrumentConfiguration(InstrumentConfiguration ic, ReferenceableParamGroupList rpgList, ScanSettingsList ssList, SoftwareList softwareList)
            : base(ic, rpgList) // Assuming base class constructor takes these parameters
        {
            this.id = ic.id;

            if (ic.scanSettingsRef != null && ssList != null)
            {
                foreach (ScanSettings ss in ssList)
                {
                    if (ic.scanSettingsRef.GetID() == ss.GetID())
                    {
                        scanSettingsRef = ss;
                        break;
                    }
                }
            }

            if (ic.componentList != null)
            {
                componentList = new ComponentList(ic.componentList, rpgList);
            }

            if (ic.softwareRef != null && softwareList != null)
            {
                foreach (Software software in softwareList)
                {
                    if (ic.softwareRef.GetReference().GetID() == software.GetID())
                    {
                        softwareRef = new SoftwareRef(software);
                        break;
                    }
                }
            }
        }

        public void SetScanSettingsRef(ScanSettings scanSettingsRef)
        {
            this.scanSettingsRef = scanSettingsRef;
        }

        public ScanSettings GetScanSettingsRef()
        {
            return scanSettingsRef;
        }

        public void SetComponentList(ComponentList componentList)
        {
            if (componentList != null)
            {
                componentList.SetParent(this);
            }
            this.componentList = componentList;
        }

        public ComponentList GetComponentList()
        {
            if (componentList == null)
            {
                componentList = new ComponentList();
            }
            return componentList;
        }

        public void SetSoftwareRef(SoftwareRef software)
        {
            softwareRef = software;
        }

        public SoftwareRef GetSoftwareRef()
        {
            return softwareRef;
        }

        public string GetID()
        {
            return id;
        }

        protected void AddTagSpecificElementsAtXPathToCollection(List<IMzMLTag> elements, string fullXPath, string currentXPath)
        {
            if (currentXPath.StartsWith("/componentList"))
            {
                if (componentList == null)
                {
                    throw new Exception("No componentList exists, so cannot go to " + fullXPath);
                }

                componentList.AddElementsAtXPathToCollection(elements, fullXPath, currentXPath);
            }
        }

        public override string GetXMLAttributeText()
        {
            string attributeText = base.GetXMLAttributeText();

            if (scanSettingsRef != null)
            {
                attributeText += $" scanSettingsRef=\"{XMLHelper.EnsureSafeXML(scanSettingsRef.GetID())}\"";
            }

            return attributeText;
        }

        public override string ToString()
        {
            return $"instrumentConfiguration: {id}";
        }

        public override string GetTagName()
        {
            return "instrumentConfiguration";
        }

        public void AddChildrenToCollection(List<IMzMLTag> children)
        {
            base.AddChildrenToCollection(children);

            if (componentList != null)
                children.Add(componentList);
            if (softwareRef != null)
                children.Add(softwareRef);
        }

        public void SetID(string id)
        {
            this.id = id;
        }

    }
}
