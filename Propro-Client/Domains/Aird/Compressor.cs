namespace Propro_Client.Domains.Aird
{
    public class Compressor
    {
        public static string TARGET_MZ = "mz";
        public static string TARGET_INTENSITY = "intensity";

        public static string METHOD_ZLIB = "zlib";
        public static string METHOD_PFOR = "pFor";
        public static string METHOD_LOG10 = "log2";

        //压缩对象,支持mz和intensity两种
        public string target;

        //压缩方法,使用分号隔开,目前支持PFor和Zlib两种
        public string method;
    }
}
