/*
 * Copyright (c) 2020 CSi Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using AirdPro.DomainsCore.Aird;
using System;
using System.Collections.Generic;
using System.IO;
using AirdPro.DomainsCore.Parser;

namespace AirdPro.Parser
{
	/// <summary>
	/// DDA模式的转换器
	/// The parser for DDA acquisition mode. The index is group like MS1-MS2 Group
	/// DDA reader using the strategy of loading all the information into the memory
	/// </summary>
	public class DDAParser : BaseParser
	{

		public DDAParser(string indexFilePath) : base(indexFilePath)
		{
		}

		/// <summary>
		/// DDA文件采用一次性读入内存的策略
		/// DDA reader using the strategy of loading all the information into the memory
		/// </summary>
		/// <returns> DDA文件中的所有信息,以MsCycle的模型进行存储 the mz-intensity pairs read from the aird. And store as MsCycle in the memory </returns>
		public virtual IList<MsCycle> parseToMsCycle()
		{
			IList<MsCycle> cycleList = new List<MsCycle>();
			IList<BlockIndex> indexList = airdInfo.indexList;
			SortedDictionary<double, MzIntensityPairs> ms1Map = parseBlockValue(airdFile, indexList[0]);
			IList<int> ms1ScanNumList = indexList[0].nums;
			IList<double> rtList = new List<double>(ms1Map.Keys);

			//将ms2 rt单位转换为分钟
			foreach (BlockIndex blockIndex in indexList)
			{
				IList<float> rts = blockIndex.rts;
				for (int i = 0; i < rts.Count; i++)
				{
					rts[i] = rts[i] / 60f;
				}
			}

			for (int i = 0; i < rtList.Count; i++)
			{
				MsCycle tempMsc = new MsCycle();

				//将ms1 rt单位转换为分钟
				tempMsc.rt = rtList[i];
				tempMsc.ms1Spectrum = ms1Map[rtList[i]];
				for (int tempBlockNum = 1; tempBlockNum < indexList.Count; tempBlockNum++)
				{
					BlockIndex tempBlockIndex = indexList[tempBlockNum];
					if (tempBlockIndex.num.Equals(ms1ScanNumList[i]))
					{
						tempMsc.rangeList = tempBlockIndex.rangeList;
						tempMsc.rts = tempBlockIndex.rts;

						SortedDictionary<double, MzIntensityPairs> ms2Map = parseBlockValue(airdFile, tempBlockIndex);
						IList<MzIntensityPairs> ms2Spectra = new List<MzIntensityPairs>(ms2Map.Values);
						tempMsc.ms2Spectrums = ms2Spectra;
						break;
					}
				}
				cycleList.Add(tempMsc);
			}
			close();
			return cycleList;
		}
	}

}