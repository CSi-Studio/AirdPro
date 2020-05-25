namespace Propro_Client.Constants
{
    class SoftwareVersion
    {
        public static int AIRD_VERSION = 4;
        public static string CLIENT_VERSION = "2.1.0";
        public static string CLIENT_VERSION_DESCRIPTION = "正式命名为AirdPro";

        public static string getVersion()
        {
            return "AirdPro V" + SoftwareVersion.CLIENT_VERSION + " (Aird Version Code:" + SoftwareVersion.AIRD_VERSION + ")";
        }

        public static string getDescription()
        {
            return CLIENT_VERSION_DESCRIPTION;
        }
    }
}
