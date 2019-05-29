using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propro_Client.Domains.Aird
{
    public class Features
    {
        //原始文件中前体的窗口大小,未经过ExperimentDO.overlap参数调整,precursor mz
        public static string scan_index_original_width = "owid";
        //原始文件中前体的荷质比窗口开始位置,未经过ExperimentDO.overlap参数调整,precursor mz
        public static string scan_index_original_precursor_mz_start = "oMzStart";
        //原始文件中前体的荷质比窗口结束位置,未经过ExperimentDO.overlap参数调整,precursor mz
        public static string scan_index_original_precursor_mz_end = "oMzEnd";
        //从Vendor文件中得到的msd.id
        public static string aird_info_raw_id = "rawId";
        //是否忽略Intensity为0的键值对,默认为true
        public static string aird_info_ignore_zero_intensity = "ignoreZeroIntensity";
        //SWATH的各个窗口间的重叠部分
        public static string aird_info_overlap = "overlap";
    }
}
