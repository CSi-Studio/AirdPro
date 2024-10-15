using AirdPro.csimzMLParser.data;
using System.Linq;
using static AirdPro.csimzMLParser.data.DataTypeTransform;
using static AirdPro.csimzMLParser.mzml.BinaryDataArray;

namespace AirdPro.csimzMLParser.mzml
{
    public class BinaryDataArrayList : MzMLContentList<BinaryDataArray>
    {
        private static readonly long serialVersionUID = 1L;

        public BinaryDataArrayList(int count) : base(count)
        {
        }

        public BinaryDataArrayList(BinaryDataArrayList bdaList, ReferenceableParamGroupList rpgList, DataProcessingList dpList) : this(bdaList.Size())
        {
            foreach (var bda in bdaList)
            {
                this.Add(new BinaryDataArray(bda, rpgList, dpList));
            }
        }

        public BinaryDataArray GetMzArray()
        {
            BinaryDataArray mzArray = null;
            foreach (BinaryDataArray binaryDataArray in GetList())
            {
                if (binaryDataArray.IsMzArray())
                {
                    mzArray = binaryDataArray;
                    break;
                }
            }

            return mzArray;
        }

        public BinaryDataArray GetIntensityArray()
        {
            BinaryDataArray intensityArray = null;
            foreach (BinaryDataArray binaryDataArray in GetList())
            {
                if (binaryDataArray.IsIntensityArray())
                {
                    intensityArray =  binaryDataArray;
                    break;
                }
            }

            return intensityArray;
        }

        public void AddBinaryDataArray(BinaryDataArray bda)
        {
            Add(bda);
        }

        public BinaryDataArray GetBinaryDataArray(int index)
        {
            return Get(index);
        }

        public void SetCompression(CompressionType compression)
        {
            foreach (BinaryDataArray bda in this)
            {
                bda.SetCompression(compression);
            }
        }

        public void SetDataType(DataType dataType)
        {
            foreach (BinaryDataArray bda in this)
            {
                bda.SetDataType(dataType);
            }
        }

        public override string GetTagName()
        {
            return "binaryDataArrayList";
        }
    }
}
