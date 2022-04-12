/*
 * Copyright (c) 2020 CSi Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using System.Collections.Generic;
using AirdSDK.Domains;

namespace AirdSDK.Parser
{
	public class MsCycle
	{

		private const long serialVersionUID = -123L;

		/// <summary>
		/// Retention Time
		/// </summary>
		public double rt { get; set; }

		/// <summary>
		/// Retention Time Index
		/// </summary>
		public double ri { get; set; }

		/// <summary>
		/// the ms1 spectrum data pairs
		/// </summary>
		public MzIntensityPairs ms1Spectrum { get; set; }

		/// <summary>
		/// MS2的RT沿用MS1
		/// </summary>
		public IList<WindowRange> rangeList { get; set; }

		//MS2的RT时间列表
		public IList<float> rts { get; set; }

		public IList<MzIntensityPairs> ms2Spectrums;
	}

}
