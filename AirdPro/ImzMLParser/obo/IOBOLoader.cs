using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.ImzMLParser.obo
{
    public interface IOBOLoader
    {
        Stream GetInputStream(string location);
    }
}
