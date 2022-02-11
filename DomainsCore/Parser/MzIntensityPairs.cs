/*
 * Copyright (c) 2020 CSi Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

namespace AirdPro.DomainsCore.Parser
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
