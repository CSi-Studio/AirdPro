using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.Domains.Parser
{
	public class SpectrumDetail
	{
		public float rt { get; set; }
		public float[] mzs { get; set; }
		public float[] intensities { get; set; }
		public byte[] mzBytes { get; set; } //压缩前的数mz据流
		public byte[] intensityBytes { get; set; } //压缩前的数intensity据流
    }
}
