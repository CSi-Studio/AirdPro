using AirdPro.csimzMLParser.obo;
using System.Collections.Generic;

namespace AirdPro.csimzMLParser.mzml
{
    public class CVList : MzMLIDContentList<CV>
    {
        public CVList(int count) : base(count)
        {

        }

        public CVList(CVList cvList) : base(cvList)
        {

        }

        public void AddCV(CV cv)
        {
            base.Add(cv);
        }

        public CV GetCV(string id)
        {
            return base.Get(id);
        }

        public override string GetTagName()
        {
            return "cvList";
        }

        public static CVList Create()
        {
            CVList cvList = new CVList(3);

            OBO obo = OBO.GetOBO();
            List<OBO> fullOBOList = obo.GetFullImportHierarchy();

            foreach (OBO currentOBO in fullOBOList)
            {
                cvList.AddCV(new CV(currentOBO));
            }
            return cvList;
        }
    }
}
