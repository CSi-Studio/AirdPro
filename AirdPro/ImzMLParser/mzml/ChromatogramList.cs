using AirdPro.ImzMLParser.util;
using System;

namespace AirdPro.ImzMLParser.mzml
{
    public class ChromatogramList : MzMLIDContentList<Chromatogram>
    {
        private static readonly long serialVersionUID = 1L;

        public DataProcessing defaultDataProcessingRef;
        public IReferenceList<DataProcessing> dataProcessingList;

        public ChromatogramList(int count, DataProcessing defaultDataProcessingRef) : base(count)
        {
            this.defaultDataProcessingRef = defaultDataProcessingRef;
        }

        public ChromatogramList(ChromatogramList chromatogramList, ReferenceableParamGroupList rpgList,
                DataProcessingList dpList, SourceFileList sourceFileList) : base(chromatogramList.Size())
        {
            if (chromatogramList.defaultDataProcessingRef != null && dpList != null)
            {
                foreach (DataProcessing dp in dpList)
                {
                    if (chromatogramList.defaultDataProcessingRef.GetID().Equals(dp.GetID()))
                    {
                        this.defaultDataProcessingRef = dp;

                        break;
                    }
                }
            }

            foreach (Chromatogram chromatogram in chromatogramList)
            {
                this.Add(new Chromatogram(chromatogram, rpgList, dpList, sourceFileList));
            }
        }

        public DataProcessing GetDefaultDataProcessingRef()
        {
            return defaultDataProcessingRef;
        }

        public void SetDataProcessingList(IReferenceList<DataProcessing> dataProcessingList)
        {
            this.dataProcessingList = dataProcessingList;

            foreach (Chromatogram chromatogram in this)
            {
                chromatogram.SetDataProcessingList(dataProcessingList);
            }
        }

        public override void Add(Chromatogram chromatogram)
        {
            base.Add(chromatogram);

            if (dataProcessingList != null)
                chromatogram.SetDataProcessingList(dataProcessingList);
        }

        public void AddChromatogram(Chromatogram chromatogram)
        {
            Add(chromatogram);
        }

        public Chromatogram GetChromatogram(int index)
        {
            return Get(index);
        }

        public Chromatogram GetChromatogram(String id)
        {
            return Get(id);
        }

        public override string GetXMLAttributeText()
        {
            return base.GetXMLAttributeText() + " defaultDataProcessingRef=\"" + XMLHelper.EnsureSafeXML(defaultDataProcessingRef.GetID()) + "\"";
        }

        public override string GetTagName()
        {
            return "chromatogramList";
        }

        public override void EnsureValidReferences()
        {
            if (dataProcessingList != null)
                defaultDataProcessingRef = dataProcessingList.GetValidReference(defaultDataProcessingRef);
        }
    }
}
