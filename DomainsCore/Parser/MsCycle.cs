using AirdPro.DomainsCore.Aird;
using System.Collections.Generic;

namespace AirdPro.DomainsCore.Parser
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
