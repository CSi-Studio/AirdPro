using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.ImzMLParser.mzml
{
    public interface IReferenceableTag
    {
        string GetID();
        void SetID(string id);
    }
}
