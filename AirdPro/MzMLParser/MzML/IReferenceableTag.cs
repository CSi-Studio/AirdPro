using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.csimzMLParser.mzml
{
    public interface IReferenceableTag
    {
        string GetID();
        void SetID(string id);
    }
}
