namespace Propro_Client.Constants
{
    class SoftwareVersion
    {
        public static int AIRD_VERSION = 3;
        public static string CLIENT_VERSION = "2.0.2";
        public static string CLIENT_VERSION_DESCRIPTION = "New Function: Change Log10 Compression Strategy to Log2";

        public static string getVersion()
        {
            return "Propro-Client V" + SoftwareVersion.CLIENT_VERSION + " (Aird Version Code:" + SoftwareVersion.AIRD_VERSION + ")";
        }

        public static string getDescription()
        {
            return CLIENT_VERSION_DESCRIPTION;
        }
    }
}
