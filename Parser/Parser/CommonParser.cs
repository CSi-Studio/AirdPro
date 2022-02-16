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
	/// 常规的格式转换模式,转换后的Aird文件同mzXML和mzML一样,所有的光谱图是以rt为顺序进行排序的
	/// The Parser for normal format just like mzXML or mzML, whose spectras are ordered by retention time
	/// </summary>
	public class CommonParser : BaseParser
	{

		public CommonParser(string indexFilePath) : base(indexFilePath)
		{
		}

		/// <summary>
		/// 根据序列号查询光谱
		/// get Spectrum by spectrum index
		/// </summary>
		/// <param name="index"> 光谱的索引号 the index of the spectrum </param>
		/// <returns> 光谱信息 spectrum data pairs </returns>
		public virtual MzIntensityPairs getSpectrum(int index)
		{
			IList<BlockIndex> indexList = getAirdInfo().indexList;
			for (int i = 0; i < indexList.Count; i++)
			{
				BlockIndex blockIndex = indexList[i];
				if (blockIndex.nums.Contains(index))
				{
					int targetIndex = blockIndex.nums.IndexOf(index);
					return getSpectrum(blockIndex, targetIndex);
				}
			}
			return null;
		}


		/// <summary>
		/// COMMON类型的文件中,每一个mzs和ints数组中存储的是位置而不是块大小
		/// In the common type file. every data stored in the mzs and ints is the spectrum location but not the block size
		/// </summary>
		/// <param name="index"> 索引信息 block index </param>
		/// <param name="position"> 指定的光谱位置 the specific spectrum index </param>
		/// <returns> 该光谱中的信息 spectrum data pairs </returns>

		public virtual MzIntensityPairs getSpectrum(BlockIndex index, int position)
		{
			try
			{
				long? start = index.mzs[position];
				if (start == null)
                {
					return null;
				}
				airdFile.Seek(start.Value, SeekOrigin.Begin);
				long? delta = index.ints[position] - start.Value;
				byte[] reader = new byte[delta.Value];
				airdFile.Read(reader, 0, (int)delta);
				
				float[] mzArray = getMzValues(reader);
				start = index.ints[position];
				airdFile.Seek(start.Value, SeekOrigin.Begin);
				if (position == (index.ints.Count - 1))
				{
					delta = index.endPtr - start.Value;
				}
				else
				{
					delta = index.mzs[position + 1] - start.Value;
				}
				reader = new byte[delta.Value];
				airdFile.Read(reader, 0, (int)delta);

				float[] intensityArray = null;
                intensityArray = getIntValues(reader);

                return new MzIntensityPairs(mzArray, intensityArray);

			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				Console.Write(e.StackTrace);
				airdFile.Close();
			}

			return null;
		}

	}

}