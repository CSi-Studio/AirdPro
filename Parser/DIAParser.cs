using AirdPro.Domains.Aird;
using System;
using System.Collections.Generic;
using System.IO;
using AirdPro.Domains.Parser;
using AirdPro.Constants;
using System.Diagnostics;
using System.Linq;
/*
* Copyright (c) 2020 CSi Biotech
* Aird and AirdPro are licensed under Mulan PSL v2.
* You can use this software according to the terms and conditions of the Mulan PSL v2.
* You may obtain a copy of Mulan PSL v2 at:
*          http://license.coscl.org.cn/MulanPSL2
* THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.
* See the Mulan PSL v2 for more details.
*/

namespace AirdPro.Parser
{
	/// <summary>
	/// DIA/SWATH模式的转换器
	/// DIA Parser will convert the original order which is ordered by retention time to a precursor m/z grouped block.
	/// spectras will be grouped by Precursor m/z range
	/// </summary>
	public class DIAParser : BaseParser
	{

		public DIAParser(string indexFilePath) : base(indexFilePath)
		{
		}

		public DIAParser(string airdPath, Compressor mzCompressor, Compressor intCompressor, int mzPrecision) : base(airdPath, mzCompressor, intCompressor, mzPrecision, AirdType.DIA_SWATH)
		{
		}

		public SortedDictionary<float, MzIntensityPairs> getSpectrums(BlockIndex index)
		{
			if (mzCompressor.methods.Contains(Compressor.METHOD_STACK))
			{
				return getSpectrums(index.startPtr, index.endPtr, index.rts, index.mzs, index.tags, index.ints);
			}
			else
			{
				return getSpectrums(index.startPtr, index.endPtr, index.rts, index.mzs, index.ints);
			}
		}

		/// <summary>
		/// 返回值是一个map,其中key为rt,value为这个rt对应点原始谱图信息
		/// 特别需要注意的是,本函数在使用完raf对象以后并不会直接关闭该对象,需要使用者在使用完DIAParser对象以后手动关闭该对象
		/// <para>
		/// the result key is rt,value is the spectrum(mz-intensity pairs)
		/// In particular, this function will not close the RAF object directly after using it.
		/// Users need to close the object manually after using the diaparser object
		/// 
		/// </para>
		/// </summary>
		/// <param name="startPtr">    起始指针位置 start point </param>
		/// <param name="endPtr">      结束指针位置 end point </param>
		/// <param name="rtList">      rt列表,包含所有的光谱产出时刻 the retention time list </param>
		/// <param name="mzSizeList">  mz块的大小列表 the mz block size list </param>
		/// <param name="intSizeList"> intensity块的大小列表 the intensity block size list </param>
		/// <returns> 每一个时刻对应的光谱信息 the spectrum of the target retention time </returns>
		public virtual SortedDictionary<float, MzIntensityPairs> getSpectrums(long startPtr, long endPtr, IList<float> rtList, IList<long> mzSizeList, IList<long> intSizeList)
		{
			try
			{
				SortedDictionary<float, MzIntensityPairs> map = new SortedDictionary<float, MzIntensityPairs>();
				airdFile.Seek(startPtr, SeekOrigin.Begin);
				
				long delta = endPtr - startPtr;
				byte[] result = new byte[(int)delta];
				airdFile.Read(result, 0, (int)delta);

				Debug.Assert(rtList.Count == mzSizeList.Count);
				Debug.Assert(mzSizeList.Count == intSizeList.Count);
				int start = 0;
				for (int i = 0; i < rtList.Count; i++)
				{
					float[] intensityArray = null;
					float[] mzArray = getMzValues(result, start, ((int)mzSizeList[i]));
					start = start + ((int)mzSizeList[i]);
					if (intCompressor.methods.Contains(Compressor.METHOD_LOG10))
					{
						intensityArray = getLogedIntValues(result, start, ((int)intSizeList[i]));
					}
					else
					{
						intensityArray = getIntValues(result, start, ((int)intSizeList[i]));
					}
					start = start + ((int)intSizeList[i]);
					map[rtList[i]] = new MzIntensityPairs(mzArray, intensityArray);
				}
				return map;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				Console.Write(e.StackTrace);
			}
			return null;
		}

		/// <summary>
		/// 返回值是一个map,其中key为rt,value为这个rt对应点原始谱图信息
		/// 特别需要注意的是,本函数在使用完raf对象以后并不会直接关闭该对象,需要使用者在使用完DIAParser对象以后手动关闭该对象
		/// <para>
		/// the result key is rt,value is the spectrum(mz-intensity pairs)
		/// In particular, this function will not close the RAF object directly after using it.
		/// Users need to close the object manually after using the diaparser object
		/// 
		/// </para>
		/// </summary>
		/// <param name="startPtr">    起始指针位置 start point </param>
		/// <param name="endPtr">      结束指针位置 end point </param>
		/// <param name="rtList">      rt列表,包含所有的光谱产出时刻 the retention time list </param>
		/// <param name="mzSizeList">  mz块的大小列表 the mz block size list </param>
		/// <param name="tagSizeList"> tag块的大小列表 the tag block size list </param>
		/// <param name="intSizeList"> intensity块的大小列表 the intensity block size list </param>
		/// <returns> 每一个时刻对应的光谱信息 the spectrum of the target retention time </returns>
		public virtual SortedDictionary<float, MzIntensityPairs> getSpectrums(long startPtr, long endPtr, IList<float> rtList, IList<long> mzSizeList, IList<long> tagSizeList, IList<long> intSizeList)
		{
			try
			{
				SortedDictionary<float, MzIntensityPairs> map = new SortedDictionary<float, MzIntensityPairs>();
				airdFile.Seek(startPtr, SeekOrigin.Begin);
				long delta = endPtr - startPtr;
				byte[] result = new byte[(int)delta];
				airdFile.Read(result, 0, (int)delta);
				Debug.Assert(tagSizeList.Count == mzSizeList.Count);
				Debug.Assert(mzSizeList.Count == intSizeList.Count);

				int start = 0;
				int maxTag = (int)Math.Pow(2, mzCompressor.digit);
				for (int i = 0; i < mzSizeList.Count; i++)
				{
					float[] mzArray = getMzValues(result, start, ((int)mzSizeList[i]));
					start = start + ((int)mzSizeList[i]);

					int[] tagArray = getTags(result, start, ((int)tagSizeList[i]));
					start += ((int)tagSizeList[i]);

					Dictionary<int, int> tagMap = new Dictionary<int, int>();
					foreach (int tag in tagArray)
					{
						if (tagMap.ContainsKey(tag))
                        {
							tagMap[tag]++;
						}
                        else
                        {
							tagMap.Add(tag, 1);
						}
					}

					IList<float[]> mzGroup = new List<float[]>();
					int layerNum = tagMap.Keys.Count;
					for (int j = 0; j < layerNum; j++)
					{
						mzGroup.Add(new float[tagMap[j]]);
					}
					int[] p = new int[layerNum];
					for (int j = 0; j < tagArray.Length; j++)
					{
						mzGroup[tagArray[j]][p[tagArray[j]]++] = mzArray[j];
					}

					float[] intensityArray = null;
					if (intCompressor.methods.Contains(Compressor.METHOD_LOG10))
					{
						intensityArray = getLogedIntValues(result, start, ((int)intSizeList[i]));
					}
					else
					{
						intensityArray = getIntValues(result, start, ((int)intSizeList[i]));
					}
					start = start + ((int)intSizeList[i]);
					IList<float[]> intensityGroup = new List<float[]>();
					int initFlag = 0;
					for (int j = 0; j < layerNum; j++)
					{
						intensityGroup.Add(intensityArray.Skip(initFlag).Take(tagMap[j]).ToArray());
						initFlag += tagMap[j];
					}

					for (int j = 0; j < layerNum; j++)
					{
						map[rtList[i * maxTag + j]] = new MzIntensityPairs(mzGroup[j], intensityGroup[j]);
					}

				}
				return map;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				Console.Write(e.StackTrace);
			}
			return null;
		}

		/// <summary>
		/// 从aird文件中获取某一条记录
		/// 查询条件: 1.起始坐标 2.全rt列表 3.mz块体积列表 4.intensity块大小列表 5.rt
		/// <para>
		/// Read a spectrum from aird with multiple query criteria.
		/// Query Criteria: 1.Start Point 2.rt list 3.mz block size list 4.intensity block size list 5.rt
		/// 
		/// </para>
		/// </summary>
		/// <param name="startPtr">    起始位置 the start point of the target spectrum </param>
		/// <param name="rtList">      全部时刻列表 all the retention time list </param>
		/// <param name="mzSizeList">  mz数组长度列表 mz size block list </param>
		/// <param name="intSizeList"> int数组长度列表 intensity size block list </param>
		/// <param name="rt">          获取某一个时刻原始谱图 the retention time of the target spectrum </param>
		/// <returns> 某个时刻的光谱信息 the spectrum of the target retention time </returns>
		public virtual MzIntensityPairs getSpectrumByRt(long startPtr, IList<float> rtList, IList<long> mzSizeList, IList<long> intSizeList, float rt)
		{
			int position = rtList.IndexOf(rt);
			return getSpectrumByIndex(startPtr, mzSizeList, intSizeList, position);
		}

		/// <summary>
		/// 从aird文件中获取某一条记录
		/// 查询条件: 1.起始坐标 2.全rt列表 3.mz块体积列表 4.tag块大小列表 5.intensity块大小列表 6.rt
		/// <para>
		/// Read a spectrum from aird with multiple query criteria.
		/// Query Criteria: 1.Start Point 2.rt list 3.mz block size list 4.intensity block size list 5.rt
		/// 
		/// </para>
		/// </summary>
		/// <param name="startPtr">    起始位置 the start point of the target spectrum </param>
		/// <param name="rtList">      全部时刻列表 all the retention time list </param>
		/// <param name="mzSizeList">  mz数组长度列表 mz size block list </param>
		/// <param name="tagSizeList">  tag数组长度列表 tag size block list </param>
		/// <param name="intSizeList"> int数组长度列表 intensity size block list </param>
		/// <param name="rt">          获取某一个时刻原始谱图 the retention time of the target spectrum </param>
		/// <returns> 某个时刻的光谱信息 the spectrum of the target retention time </returns>
		public virtual MzIntensityPairs getSpectrumByRt(long startPtr, IList<float> rtList, IList<long> mzSizeList, IList<long> tagSizeList, IList<long> intSizeList, float rt)
		{
			int position = rtList.IndexOf(rt);
			return getSpectrumByIndex(startPtr, mzSizeList, tagSizeList, intSizeList, position);
		}


		/// <summary>
		/// 从一个完整的Swath Block块中取出一条记录
		/// 查询条件: 1. block索引号 2. rt
		/// <para>
		/// Read a spectrum from aird with block index and target rt
		/// 
		/// </para>
		/// </summary>
		/// <param name="index"> block index </param>
		/// <param name="rt">    retention time of the target spectrum </param>
		/// <returns> the target spectrum </returns>
		public virtual MzIntensityPairs getSpectrumByRt(BlockIndex index, float rt)
		{
			IList<float> rts = index.rts;
			int position = rts.IndexOf(rt);
			return getSpectrumByIndex(index, position);
		}

		/// <summary>
		/// 从aird文件中获取某一条记录
		/// 查询条件: 1.起始坐标 2.mz块体积列表 3.intensity块大小列表 4.光谱在块中的索引位置
		/// <para>
		/// Read a spectrum from aird with multiple query criteria.
		/// Query Criteria: 1.Start Point 2.mz block size list 3.intensity block size list  4.spectrum index in the block
		/// 
		/// </para>
		/// </summary>
		/// <param name="startPtr">    起始位置 the start point of the target spectrum </param>
		/// <param name="mzSizeList">  mz数组长度列表 mz size block list </param>
		/// <param name="intSizeList"> int数组长度列表 intensity size block list </param>
		/// <param name="index">       光谱在block块中的索引位置 the spectrum index in the block </param>
		/// <returns> 某个时刻的光谱信息 the spectrum of the target retention time </returns>
		public virtual MzIntensityPairs getSpectrumByIndex(long startPtr, IList<long> mzSizeList, IList<long> intSizeList, int index)
		{
			try
			{
				long start = startPtr;

				for (int i = 0; i < index; i++)
				{
					start += mzSizeList[i];
					start += intSizeList[i];
				}
				airdFile.Seek(start, SeekOrigin.Begin);
				byte[] reader = new byte[(int)mzSizeList[index]];
				airdFile.Read(reader, 0, (int)mzSizeList[index]);
				float[] mzArray = getMzValues(reader);
				start += ((int)mzSizeList[index]);
				airdFile.Seek(start, SeekOrigin.Begin);
				reader = new byte[(int)intSizeList[index]];
				airdFile.Read(reader, 0, (int)intSizeList[index]);

				float[] intensityArray = null;
				if (intCompressor.methods.Contains(Compressor.METHOD_LOG10))
				{
					intensityArray = getLogedIntValues(reader);
				}
				else
				{
					intensityArray = getIntValues(reader);
				}
				return new MzIntensityPairs(mzArray, intensityArray);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				Console.Write(e.StackTrace);
			}
			return null;
		}


		/// <summary>
		/// 从aird文件中获取某一条记录
		/// 查询条件: 1.起始坐标 2.mz块体积列表 3.tag块大小列表 4.intensity块大小列表 5.光谱在块中的索引位置
		/// <para>
		/// Read a spectrum from aird with multiple query criteria.
		/// Query Criteria: 1.Start Point 2.mz block size list 3.intensity block size list  4.spectrum index in the block
		///     
		/// </para>
		/// </summary>
		/// <param name="startPtr">    起始位置 the start point of the target spectrum </param>
		/// <param name="mzSizeList">  mz数组长度列表 mz size block list </param>
		/// <param name="tagSizeList"> tag块的大小列表 the tag block size list </param>
		/// <param name="intSizeList"> int数组长度列表 intensity size block list </param>
		/// <param name="index">       光谱在block块中的索引位置 the spectrum index in the block </param>
		/// <returns> 某个时刻的光谱信息 the spectrum of the target retention time </returns>
		public virtual MzIntensityPairs getSpectrumByIndex(long startPtr, IList<long> mzSizeList, IList<long> tagSizeList, IList<long> intSizeList, int index)
		{
			try
			{
				long start = startPtr;

				int maxTag = (int)Math.Pow(2, mzCompressor.digit);
				int mzIndex = index / maxTag;

				for (int i = 0; i < mzIndex; i++)
				{
					start += mzSizeList[i];
					start += tagSizeList[i];
					start += intSizeList[i];
				}

				airdFile.Seek(start, SeekOrigin.Begin);
				byte[] reader = new byte[((int)mzSizeList[mzIndex])];
				airdFile.Read(reader, 0, (int)mzSizeList[mzIndex]);
				float[] mzArray = getMzValues(reader);
				start += ((int)mzSizeList[mzIndex]);

				airdFile.Seek(start, SeekOrigin.Begin);
				reader = new byte[((int)tagSizeList[mzIndex])];
				airdFile.Read(reader, 0, (int)tagSizeList[mzIndex]);
				int[] tagArray = getTags(reader);
				start += ((int)tagSizeList[mzIndex]);

				airdFile.Seek(start, SeekOrigin.Begin);
				reader = new byte[((int)intSizeList[mzIndex])];
				airdFile.Read(reader, 0, (int)intSizeList[mzIndex]);
				float[] intensityArray = null;
				if (intCompressor.methods.Contains(Compressor.METHOD_LOG10))
				{
					intensityArray = getLogedIntValues(reader);
				}
				else
				{
					intensityArray = getIntValues(reader);
				}
				IDictionary<int, int> tagMap = new Dictionary<int, int>();
				foreach (int tag in tagArray)
				{
					if (tagMap.ContainsKey(tag))
					{
						tagMap[tag]++;
					}
					else
					{
						tagMap.Add(tag, 1);
					}
				}
				IList<float[]> mzGroup = new List<float[]>();
				int layerNum = tagMap.Keys.Count;
				for (int j = 0; j < layerNum; j++)
				{
					mzGroup.Add(new float[tagMap[j]]);
				}
				int[] p = new int[layerNum];
				for (int j = 0; j < tagArray.Length; j++)
				{
					mzGroup[tagArray[j]][p[tagArray[j]]++] = mzArray[j];
				}
				IList<float[]> intensityGroup = new List<float[]>();
				int initFlag = 0;
				for (int j = 0; j < layerNum; j++)
				{
					intensityGroup.Add(intensityArray.Skip(initFlag).Take(tagMap[j]).ToArray());
					initFlag += tagMap[j];
				}
				int remainder = index - mzIndex * maxTag;
				return new MzIntensityPairs(mzGroup[remainder], intensityGroup[remainder]);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				Console.Write(e.StackTrace);
			}
			return null;
		}

		/// <param name="blockIndex"> 块索引 </param>
		/// <param name="index"> 块内索引值 </param>
		/// <returns> 对应光谱数据 </returns>
		public virtual MzIntensityPairs getSpectrumByIndex(BlockIndex blockIndex, int index)
		{
			if (mzCompressor.methods.Contains(Compressor.METHOD_STACK))
			{
				return getSpectrumByIndex(blockIndex.startPtr, blockIndex.mzs, blockIndex.tags, blockIndex.ints, index);
			}
			else
			{
				return getSpectrumByIndex(blockIndex.startPtr, blockIndex.mzs, blockIndex.ints, index);
			}
		}


		/// <summary>
		/// 从aird文件中获取某一条记录,同时返回它的原始二进制数组
		/// 查询条件: 1.起始坐标 2.mz块体积列表 3.intensity块大小列表 4.光谱在块中的索引位置
		/// <para>
		/// Read a spectrum from aird with multiple query criteria.
		/// Query Criteria: 1.Start Point 2.mz block size list 3.intensity block size list  4.spectrum index in the block
		/// 
		/// </para>
		/// </summary>
		/// <param name="startPtr">    起始位置 the start point of the target spectrum </param>
		/// <param name="mzSizeList">  mz数组长度列表 mz size block list </param>
		/// <param name="intSizeList"> int数组长度列表 intensity size block list </param>
		/// <param name="index">       光谱在block块中的索引位置 the spectrum index in the block </param>
		/// <returns> 某个时刻的光谱信息 the spectrum of the target retention time, The spectrum contains the original bytes array of mz and intensity </returns>
		public virtual SpectrumDetail getSpectrumDetailByIndex(long startPtr, IList<long> mzSizeList, IList<long> intSizeList, int index)
		{
			SpectrumDetail detail = new SpectrumDetail();
			try
			{
				long start = startPtr;

				for (int i = 0; i < index; i++)
				{
					start += mzSizeList[i];
					start += intSizeList[i];
				}

				airdFile.Seek(start, SeekOrigin.Begin);
				byte[] reader = new byte[((int)mzSizeList[index])];
				airdFile.Read(reader, 0, (int)mzSizeList[index]);
				detail.mzBytes = (byte[])reader.Clone();
				float[] mzArray = getMzValues(reader);
				start += ((int)mzSizeList[index]);
				airdFile.Seek(start, SeekOrigin.Begin);
				reader = new byte[((int)intSizeList[index])];
				airdFile.Read(reader, 0, (int)intSizeList[index]);
				detail.intensityBytes = ((byte[])reader.Clone());
				float[] intensityArray = null;
				if (intCompressor.methods.Contains(Compressor.METHOD_LOG10))
				{
					intensityArray = getLogedIntValues(reader);
				}
				else
				{
					intensityArray = getIntValues(reader);
				}

				detail.mzs = mzArray;
				detail.intensities = intensityArray;
				return detail;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				Console.Write(e.StackTrace);
			}
			return null;
		}


		/// <summary>
		/// 根据序列号查询光谱
		/// </summary>
		/// <param name="index"> 索引序列号 </param>
		/// <returns> 该索引号对应的光谱信息 </returns>
		public virtual MzIntensityPairs getSpectrum(int index)
		{
			IList<BlockIndex> indexList = getAirdInfo().indexList;
			for (int i = 0; i < indexList.Count; i++)
			{
				BlockIndex blockIndex = indexList[i];
				if (blockIndex.nums.Contains(index))
				{
					int targetIndex = blockIndex.nums.IndexOf(index);
					return getSpectrumByIndex(blockIndex, targetIndex);
				}
			}
			return null;
		}

		public virtual SpectrumDetail getSpectrumDetail(int index)
		{
			IList<BlockIndex> indexList = getAirdInfo().indexList;
			for (int i = 0; i < indexList.Count; i++)
			{
				BlockIndex blockIndex = indexList[i];
				if (blockIndex.nums.Contains(index))
				{
					int targetIndex = blockIndex.nums.IndexOf(index);
					MzIntensityPairs pairs = getSpectrumByIndex(blockIndex, targetIndex);
				}
			}
			return null;
		}



		/// <summary>
		/// Specific API
		/// 从Aird文件中读取,但是不要将m/z数组的从Integer改为Float
		/// <para>
		/// Read from aird, but not convert the m/z array from integer to float
		/// 
		/// </para>
		/// </summary>
		/// <param name="index">      索引序列号 block index </param>
		/// <param name="rt">         光谱产出时刻 retention time for the spectrum </param>
		/// <param name="compressor"> 压缩方式 compression method 0: ZDPD 1: S-ZDPD, default compressor = 0 </param>
		/// <returns> 该时刻产生的光谱信息, 其中mz数据以int类型返回 the target rt's spectrum with integer mz array </returns>
		public virtual MzIntensityPairs getSpectrumAsInteger(BlockIndex index, float rt, int compressor)
		{
			if (compressor == 0)
			{
				return getSpectrumAsInteger(index, rt);
			}
			else if (compressor == 1)
			{
				IList<float> rts = index.rts;
				int position = rts.IndexOf(rt);
				MzIntensityPairs mzIntensityPairs = getSpectrumByIndex(index, position);
				int[] mzArray = new int[mzIntensityPairs.getMzArray().Length];
				for (int i = 0; i < mzArray.Length; i++)
				{
					mzArray[i] = (int)(mzIntensityPairs.getMzArray()[i] * mzPrecision);
				}
				return new MzIntensityPairs(mzArray, mzIntensityPairs.getIntensityArray());
			}
			else
			{
				Console.WriteLine("No such compressor.");
			}

			return null;
		}


		public virtual MzIntensityPairs getSpectrumAsInteger(BlockIndex index, float rt)
		{
			try
			{
				IList<float> rts = index.rts;
				int position = rts.IndexOf(rt);

				long start = index.startPtr;

				for (int i = 0; i < position; i++)
				{
					start += index.mzs[i];
					start += index.ints[i];
				}
				airdFile.Seek(start, SeekOrigin.Begin);
				byte[] reader = new byte[((int)index.mzs[position])];
				airdFile.Read(reader, 0, (int)index.mzs[position]);
				int[] mzArray = getMzValuesAsInteger(reader);
				start += ((int)index.mzs[position]);
				airdFile.Seek(start, SeekOrigin.Begin);
				reader = new byte[((int)index.ints[position])];
				airdFile.Read(reader, 0, (int)index.ints[position]);

				float[] intensityArray = null;
				if (intCompressor.methods.Contains(Compressor.METHOD_LOG10))
				{
					intensityArray = getLogedIntValues(reader);
				}
				else
				{
					intensityArray = getIntValues(reader);
				}

				return new MzIntensityPairs(mzArray, intensityArray);

			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				Console.Write(e.StackTrace);
			}

			return null;
		}


	}
}