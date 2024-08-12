using AirdPro.IMSRawDataCompress.datamodel.callbacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.IMSRawDataCompress.datamodel
{
    public class TDFLibrary
    {
        [DllImport("timsdata_x64.dll")]
        public static extern long tims_open(string analysis_dir, long use_recalib);

        [DllImport("timsdata_x64.dll")]
        public static extern long tims_open_v2(String analysis_dir, long use_recalib, int pressureCompensation);

        [DllImport("timsdata_x64.dll")]
        public static extern void tims_close(long handle);

        [DllImport("timsdata_x64.dll")] 
        public static extern long tims_get_last_error_string(byte[] error, long len);

        [DllImport("timsdata_x64.dll")] 
        public static extern long tims_has_recalibrated_state(long handle);

        [DllImport("timsdata_x64.dll")]
        public static extern long tims_read_scans_v2(long handle, long frameId, long scanBegin, long scanEnd,
      byte[] scanBuffer, long len);

        [DllImport("timsdata_x64.dll")]
        public static extern long tims_read_pasef_msms(long handle, long[] precursors, long num_precursors,
      ICentroidCallback callback);

        [DllImport("timsdata_x64.dll")]
        public static extern long tims_read_pasef_msms_v2(long handle, long[] precursors, long num_precursors,
            CentroidData callback, Pointer user_data);

        [DllImport("timsdata_x64.dll")]
        public static extern long tims_read_pasef_profile_msms_v2(long handle, long[] precursors, long num_precursors,
      ProfileData callback, Pointer user_data);

        [DllImport("timsdata_x64.dll")]
        public static extern long tims_read_pasef_profile_msms_for_frame_v2(long handle, long frame_id,
      MultipleProfileData callback, Pointer user_data);

        [DllImport("timsdata_x64.dll")] 
        public static extern long tims_index_to_mz(long handle, long frameId, double[] index, double[] mz, long len);

        [DllImport("timsdata_x64.dll")]
        public static extern long tims_scannum_to_oneoverk0(long handle, long frameId, double[] scannum, double[] oneOverK0,
      long len);

        [DllImport("timsdata_x64.dll")]
        public static extern double tims_oneoverk0_to_ccs_for_mz(double ook0, long charge, double mz);
        
        [DllImport("timsdata_x64.dll")] 
        public static extern double tims_ccs_to_oneoverk0_for_mz(double ccs, long charge, double mz);
        
        [DllImport("timsdata_x64.dll")]
        public static extern long tims_extract_centroided_spectrum_for_frame_v2(long handle, long frame_id, long scan_begin,long scan_end, ICentroidCallback callback, Pointer user_data);
        //public static extern long tims_extract_centroided_spectrum_for_frame_v2(long handle, long frame_id, long scan_begin,
      //long scan_end, ICentroidCallback callback, IntPtr user_data);

        [DllImport("timsdata_x64.dll")]
        public static extern long tims_extract_profile_for_frame(long handle, long frame_id, long scan_begin, long scan_end,
      IProfileCallback callback, Pointer userData);

        [DllImport("timsdata_x64.dll")] 
        public static extern void tims_set_num_threads(int numThreads);
    }
}
