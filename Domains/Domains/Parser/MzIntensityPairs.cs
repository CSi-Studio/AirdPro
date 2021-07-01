using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.Domains.Parser
{
	public class MzIntensityPairs
	{

		/// <summary>
		/// m/z array with float type
		/// 使用float类型进行存储的mz数组
		/// </summary>
		internal float[] mzArray;

		/// <summary>
		/// m/z array with integer type which is directly from Aird file
		/// 使用int类型进行存储的mz数组
		/// </summary>
		internal int[] mz;

		/// <summary>
		/// intensity array with float type
		/// </summary>
		internal float[] intensityArray;

		public MzIntensityPairs()
		{
		}

		public MzIntensityPairs(float[] mzArray, float[] intensityArray)
		{
			this.mzArray = mzArray;
			this.intensityArray = intensityArray;
		}

		public MzIntensityPairs(int[] mz, float[] intensityArray)
		{
			this.mz = mz;
			this.intensityArray = intensityArray;
		}

		public float[] getMzArray()
        {
			return mzArray;
		}

		public float[] getIntensityArray()
		{
			return intensityArray;
		}
	}

}
