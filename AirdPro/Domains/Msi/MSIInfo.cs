using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.Domains.Msi
{
    public class MSIInfo
    {
        //ROW_PER_FILE = 0,IMAGE_PER_FILE = 1,SPECTRUM_PER_FILE =2
        public int MSIFileOrganisation;

        //MSI 
        public int lineScanDirection;

        //MSI 
        public int scanPattern;

        public int pixelX;

        public int pixelY;

        public int pixelZ;
    }
}
