using System.Collections.Generic;
using pwiz.CLI.cv;
using pwiz.CLI.data;
using pwiz.CLI.msdata;

namespace AirdPro.DomainsCore.Aird
{
    public class CV
    {
        public string cvid;
        public object value;
        public string units;

        public CV()
        {
        }

        public CV(CVParam param)
        {
            this.cvid = ((int) param.cvid)+":"+param.name;
            this.value = param.value.ToString();
            int unitsId = (int) param.units;
            if (unitsId != -1)
            {
                this.units = (int)param.units + ":"+ param.unitsName;
            }
        }

        public static List<CV> trans(Spectrum spectrum)
        {
            List<CV> cvList = new List<CV>();
            CVParamList cvParamList = spectrum.cvParams;
            foreach (var cvParam in cvParamList)
            {
                cvList.Add(new CV(cvParam));
            }

            return cvList;
        }

        public static List<CV> trans(CVParamList paramList)
        {
            List<CV> cvList = new List<CV>();
            foreach (var cvParam in paramList)
            {
                cvList.Add(new CV(cvParam));
            }

            return cvList;
        }
    }
}