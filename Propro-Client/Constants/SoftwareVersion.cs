namespace AirdPro.Constants
{
    class SoftwareInfo
    {
        public static string VERSION = "1.0.0";
        public static int VERSION_CODE = 1;
        public static string CLIENT_VERSION_DESCRIPTION = "正式命名为AirdPro,合并SWATHIndex与BlockIndex为BlockIndex";
        public static string NAME = "AirdPro";

        public static string getVersion()
        {
            return "AirdPro V" + SoftwareInfo.VERSION + " (Aird Version Code:" + SoftwareInfo.VERSION_CODE + ")";
        }

        public static string getDescription()
        {
            return CLIENT_VERSION_DESCRIPTION;
        }
    }
}
