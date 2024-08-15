using System;
using System.Reflection;
using System.Runtime.InteropServices;
using AirdPro.IMSRawDataCompress.datamodel.callbacks;

namespace AirdPro.IMSRawDataCompress.import_rawdata_bruker_tdf.datamodel
{
    public class TDFLibrary
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CentroidCallback(long precursorId, int numPeaks, IntPtr pMz, IntPtr pIntensities, IntPtr userData);

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
        public static extern long tims_scannum_to_oneoverk0(long handle, long frameId, double[] scannum, double[] oneOverK0, long len);

        [DllImport("timsdata_x64.dll")]
        public static extern double tims_oneoverk0_to_ccs_for_mz(double ook0, long charge, double mz);
        
        [DllImport("timsdata_x64.dll")] 
        public static extern double tims_ccs_to_oneoverk0_for_mz(double ccs, long charge, double mz);

        /**
          * Read peak-picked spectra for a tims frame.
          * <p>
          * Given a frame ID, this function reads the frame, sums up the corresponding scan-number ranges
          * into a synthetic profile spectrum, performs centroiding using an algorithm and parameters
          * suggested by Bruker, and returns the resulting spectrum (exactly one for the frame ID).
          * <p>
          * Note: Result callback identical to the tims_read_pasef_msms_v2 methods, but only returns a
          * single result and the parameter id is the frame_id
          * <p>
          * Note: different threads must not read scans from the same storage handle concurrently.
          *
          * @param handle     see {@link TDFLibrary#tims_open(String, long)}.
          * @param frame_id   Bruker: "list of PASEF precursor IDs; the returned spectra may be in
          *                   different order"
          * @param scan_begin first scan number to read (inclusive)
          * @param scan_end   last scan number (exclusive)
          * @param callback   callback accepting the spectra
          * @param user_data  will be passed to callback
          * @return 0 on error
          */
        [DllImport("timsdata_x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long tims_extract_centroided_spectrum_for_frame_v2(long handle, long frame_id, long scan_begin,long scan_end, CentroidCallback callback, IntPtr user_data);         

        [DllImport("timsdata_x64.dll")]
        public static extern long tims_extract_profile_for_frame(long handle, long frame_id, long scan_begin, long scan_end, IProfileCallback callback, IntPtr user_data);

        [DllImport("timsdata_x64.dll")] 
        public static extern void tims_set_num_threads(int numThreads);
    }
}
