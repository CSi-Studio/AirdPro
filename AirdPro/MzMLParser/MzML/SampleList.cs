using System;

namespace AirdPro.csimzMLParser.mzml
{
    public class SampleList : MzMLIDContentList<Sample>
    {
        private const long serialVersionUID = 1L;

        public SampleList(int count) : base(count)
        {        

        }

        public SampleList(SampleList sampleList, ReferenceableParamGroupList rpgList) : this(sampleList.Size())
        {
            foreach (Sample sample in sampleList)
            {
                Add(new Sample(sample, rpgList));
            }
        }

        public void AddSample(Sample sample)
        {
            Add(sample);
        }

        public Sample GetSample(int index)
        {
            return Get(index);
        }

        public Sample GetSample(String id)
        {
            return Get(id);
        }

        public Sample RemoveSample(int index)
        {
            return Remove(index);
        }

        public override string GetTagName()
        {
            return "sampleList";
        }
    }
}
