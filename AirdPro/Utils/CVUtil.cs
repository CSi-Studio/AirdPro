using pwiz.CLI.data;
using System.Collections.Generic;
using pwiz.CLI.cv;
using CV = AirdSDK.Beans.CV;

namespace AirdPro.Utils;

public class CVUtil
{
    //由于直接对这些指定的cv字段进行存储,因此不需要再转存一遍
    private static HashSet<CVID> skipList = new HashSet<CVID>()
    {
        CVID.MS_scan_start_time,
        CVID.MS_ms_level,
        CVID.MS_MSn_spectrum,
        CVID.MS_MS1_spectrum,
        CVID.MS_inverse_reduced_ion_mobility,
        CVID.MS_TIC,
        CVID.MS_negative_scan,
        CVID.MS_positive_scan,
        CVID.MS_profile_spectrum,
        CVID.MS_centroid_spectrum,
        CVID.MS_HCD,
        CVID.MS_CID,
        CVID.MS_ECD,
        CVID.MS_ETD,
        CVID.MS_collision_energy,
        CVID.MS_base_peak_intensity,
        CVID.MS_base_peak_m_z
    };

    public static List<CV> trans(CVParamList paramList)
    {
        if (paramList == null)
        {
            return null;
        }

        List<CV> cvList = new List<CV>();
        foreach (var cvParam in paramList)
        {
            if (skipList.Contains(cvParam.cvid))
            {
                continue;
            }

            cvList.Add(build(cvParam));
        }

        return cvList;
    }

    public static CV build(CVParam param)
    {
        CV cv = new CV();
        cv.cvid = ((int) param.cvid) + ":" + param.name;
        cv.value = param.value.ToString();
        int unitsId = (int) param.units;
        if (unitsId != -1)
        {
            cv.units = (int) param.units + ":" + param.unitsName;
        }

        return cv;
    }
}