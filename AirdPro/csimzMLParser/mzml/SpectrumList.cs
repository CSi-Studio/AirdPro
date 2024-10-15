using AirdPro.csimzMLParser.util;
using System.Collections.Generic;

namespace AirdPro.csimzMLParser.mzml
{
    public class SpectrumList : MzMLIDContentList<Spectrum>
    {
        private static readonly long serialVersionUID = 1L;

        public Dictionary<string, Spectrum> spectrumDictionary;
        public DataProcessing defaultDataProcessingRef;
        public IReferenceList<DataProcessing> dataProcessingList;

        public SpectrumList(int count) : base(count)
        {

        }

        public SpectrumList(int count, DataProcessing defaultDataProcessingRef) : this(count)
        {
            this.defaultDataProcessingRef = defaultDataProcessingRef;
        }

        public SpectrumList(SpectrumList spectrumList, ReferenceableParamGroupList rpgList, DataProcessingList dpList,
                            SourceFileList sourceFileList, InstrumentConfigurationList icList) : this(spectrumList.Size())
        {
            foreach (Spectrum spectrum in spectrumList)
            {
                Spectrum newSpectrum = new(spectrum, rpgList, dpList, sourceFileList, icList);
                Add(newSpectrum);
            }

            if (spectrumList.defaultDataProcessingRef != null && dpList != null)
            {
                foreach(DataProcessing dp in dpList)
                {
                    if (spectrumList.defaultDataProcessingRef.GetID().Equals(dp.id))
                    {
                        defaultDataProcessingRef = dp;
                    }
                }
            }
        }

        public void SetDefaultDataProcessingRef(DataProcessing dp)
        {
            defaultDataProcessingRef = dp;
            EnsureValidReferences();
        }

        public DataProcessing GetDefaultDataProcessingRef()
        {
            return defaultDataProcessingRef;
        }

        public void SetDataProcessingList(IReferenceList<DataProcessing> dataProcessingList)
        {
            this.dataProcessingList = dataProcessingList;
            foreach (Spectrum spectrum in this)
            {
                spectrum.SetDataProcessingList(dataProcessingList);
            }
        }

        public override void Add(Spectrum spectrum)
        {
            base.Add(spectrum);
            if (dataProcessingList != null)
            {
                spectrum.SetDataProcessingList(dataProcessingList);
            }
            if (spectrumDictionary != null)
            {
                spectrumDictionary[spectrum.id] = spectrum;
            }
        }

        public void AddSpectrum(Spectrum spectrum)
        { 
            Add(spectrum);
        }

            public Spectrum GetSpectrum(int index)
        {
            return Get(index);
        }

        public override Spectrum Get(string id)
        {
            List<Spectrum> list = GetList();
            if (spectrumDictionary == null)
            {
                spectrumDictionary = new Dictionary<string, Spectrum>(list.Count);
                foreach (Spectrum spectrum in list) 
                { 
                    spectrumDictionary[spectrum.id] = spectrum;
                }
            }            
            return spectrumDictionary[id];
        }

        public Spectrum GetSpectrum(string id)
        {
            return Get(id);
        }

        public override bool Remove(Spectrum spectrum)
        {
            bool success = GetList().Remove(spectrum);
            Spectrum removedSpectrum = spectrum;
            if (spectrumDictionary != null)
            {
                removedSpectrum = spectrumDictionary[spectrum.id];
                spectrumDictionary.Remove(removedSpectrum.id);
            }
            return success && (removedSpectrum.Equals(spectrum));
        }

        public bool RemoveSpectrum(Spectrum spectrum)
        {            
            return Remove(spectrum);
        }

        public override string GetXMLAttributeText()
        {
            return base.GetXMLAttributeText() + "defaultDataProcessingRef=\""
                + XMLHelper.EnsureSafeXML(defaultDataProcessingRef.id + "\"");
        }

        public override string GetTagName()
        {
            return "spectrumList";
        }


        public override void EnsureValidReferences()
        {
            if (dataProcessingList != null)
            {
                defaultDataProcessingRef = dataProcessingList.GetValidReference(defaultDataProcessingRef);
            }
        }
    }
}
