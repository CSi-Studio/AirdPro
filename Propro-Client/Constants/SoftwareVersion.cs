using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace Propro_Client.Constants
{
    class SoftwareVersion
    {
        public static int AIRD_VERSION = 2;
        public static string CLIENT_VERSION = "1.7.0";

        public static string getVersion()
        {
            return "Propro-Client V" + SoftwareVersion.CLIENT_VERSION + " (Aird Version Code:" + SoftwareVersion.AIRD_VERSION + ")";
        }
    }
}
