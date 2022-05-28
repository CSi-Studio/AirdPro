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
using pwiz.CLI.cv;
using pwiz.CLI.data;

namespace AirdSDK.Domains
{
    public class CV
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

        public string cvid;
        public object value;
        public string units;

        public CV()
        {
        }

        public CV(CVParam param)
        {
            this.cvid = ((int) param.cvid) + ":" + param.name;
            this.value = param.value.ToString();
            int unitsId = (int) param.units;
            if (unitsId != -1)
            {
                this.units = (int) param.units + ":" + param.unitsName;
            }
        }

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

                cvList.Add(new CV(cvParam));
            }

            return cvList;
        }
    }
}