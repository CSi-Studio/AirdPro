namespace AirdPro.Domains.Aird
{
    /**
     * Path to all the ancestor files (up to the native acquisition file) used to generate the current Aird instance document.
     */
    public class ParentFile
    {
        /**
         * Name of the parent file
         */
        public string name;

        /**
         * Location of the parent file
         */
        public string location;

        /**
         * Was the parent file a native acquisition file? Or was it processed data?
         * enumeration: RAWData, processedData
         */
        public string fileType;

        /**
         * sha-1 sum of the parent file, the length must less than 40
         */
        public string fileSha1;

        /**
         * 文件格式
         */
        public string formatType;

    }
}
