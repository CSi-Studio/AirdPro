using AirdPro.csimzMLParser.util;
using HZH_Controls;
using System;
using System.Collections.Generic;

namespace AirdPro.csimzMLParser.mzml
{
    public class Precursor : MzMLContentWithParams
    {
        private static readonly long serialVersionUID = 1L;

        public string ExternalSpectrumID;
        public SourceFile SourceFileRef;
        public Spectrum SpectrumRef;
        public IsolationWindow IsolationWindow;
        public SelectedIonList SelectedIonList;
        public Activation Activation;

        public Precursor() : base()
        {
            
        }

        public Precursor(Precursor precursor, ReferenceableParamGroupList rpgList, SourceFileList SourceFileList)
        {
            this.ExternalSpectrumID = precursor.ExternalSpectrumID;
            this.SpectrumRef = precursor.SpectrumRef;

            if (precursor.SourceFileRef != null && SourceFileList != null)
            {
                foreach (SourceFile SourceFile in SourceFileList)
                {
                    if (precursor.SourceFileRef.id.Equals(SourceFile.id))
                    {
                        this.SourceFileRef = SourceFile;

                        break;
                    }
                }
            }

            if (precursor.IsolationWindow != null)
            {
                IsolationWindow = new IsolationWindow(precursor.IsolationWindow, rpgList);
            }
            if (precursor.SelectedIonList != null)
            {
                SelectedIonList = new SelectedIonList(precursor.SelectedIonList, rpgList);
            }
            if (precursor.Activation != null)
            {
                Activation = new Activation(precursor.Activation, rpgList);
            }
        }

        public void SetExternalSpectrum(SourceFile sourceFileRef, String externalSpectrumID)
        {
            this.SourceFileRef = sourceFileRef;
            this.ExternalSpectrumID = externalSpectrumID;
        }

        public void SetSpectrumRef(Spectrum spectrumRef)
        {
            this.SpectrumRef = spectrumRef;
        }
        
        public void SetIsolationWindow(IsolationWindow isolationWindow)
        {
            IsolationWindow.SetParent(this);

            this.IsolationWindow = isolationWindow;
        }

        public IsolationWindow GetIsolationWindow()
        {
            return IsolationWindow;
        }

        public void SetSelectedIonList(SelectedIonList selectedIonList)
        {
            SelectedIonList.SetParent(this);

            this.SelectedIonList = selectedIonList;
        }

        public SelectedIonList GetSelectedIonList()
        {
            return SelectedIonList;
        }

        public void SetActivation(Activation activation)
        {
            Activation.SetParent(this);

            this.Activation = activation;
        }

        public Activation GetActivation()
        {
            return Activation;
        }

        public override string GetXMLAttributeText()
        {
            String attributeText = base.GetXMLAttributeText();

            if (ExternalSpectrumID != null)
            {
                attributeText += " externalSpectrumID=\"" + XMLHelper.EnsureSafeXML(ExternalSpectrumID) + "\"";
            }
            if (SourceFileRef != null)
            {
                attributeText += " sourceFileRef=\"" + XMLHelper.EnsureSafeXML(SourceFileRef.id) + "\"";
            }
            if (SpectrumRef != null)
            {
                attributeText += " spectrumRef=\"" + XMLHelper.EnsureSafeXML(SpectrumRef.id) + "\"";
            }

            if (attributeText.StartsWith(" "))
                attributeText = attributeText.Substring(1);
            return attributeText;
        }

        public override string ToString()
        {
            return "precursor: "
                    + ((SpectrumRef != null) ? " spectrumRef=\"" + SpectrumRef.id + "\"" : "")
                    + ((ExternalSpectrumID != null && !ExternalSpectrumID.IsEmpty()) ? " externalSpectrumID=\"" + ExternalSpectrumID + "\"" : "")
                    + ((SourceFileRef != null) ? " sourceFileRef=\"" + SourceFileRef.id + "\"" : "");
        }

        public override string GetTagName()
        {
            return "precursor";
        }       

        public override void AddChildrenToCollection(ICollection<IMzMLTag> children)
        {
            if (IsolationWindow != null)
                children.Add(IsolationWindow);
            if (SelectedIonList != null)
                children.Add(SelectedIonList);
            if (Activation != null)
                children.Add(Activation);

            base.AddChildrenToCollection(children);
        }
    }
}
