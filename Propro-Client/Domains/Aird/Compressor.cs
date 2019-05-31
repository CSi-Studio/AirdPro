namespace Propro_Client.Domains.Aird
{
    public class Compressor
    {
        //压缩对象,支持mz和intensity两种
        public string target;

        //压缩方法,使用分号隔开,目前支持PFor和Zlib两种
        public string method;
    }
}
