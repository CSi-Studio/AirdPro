namespace AirdPro.Domains.Aird
{
    public class Layers
    {
        /**
         * 使用fastPfor算法压缩以后的mz数组
         * compressed mz array with fastPfor
        */
        public byte[] mzArray;

        /**
         * mz对应的层索引
         * layer index of mz
         */
        public byte[] tagArray;

        /**
         * 存储单个索引所需的位数
         * storage digits occupied by an index
         */
        public int digit;

        public Layers() { }

        public Layers(byte[] mzArray, byte[] tagArray, int digit)
        {
            this.mzArray = mzArray;
            this.tagArray = tagArray;
            this.digit = digit;
        }
    }
}