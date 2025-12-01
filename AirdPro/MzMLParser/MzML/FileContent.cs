using AirdPro.csimzMLParser.obo;
using System.Collections.Generic;

namespace AirdPro.csimzMLParser.mzml
{
    public class FileContent : MzMLContentWithParams
    {
        /**
        * 序列化版本ID。
        */
        private const long serialVersionUID = 1L;

        /**
         * 访问号: 数据文件内容 (MS:1000524)。必须至少提供一个子元素
         */
        public static readonly string DATA_FILE_CONTENT_ID = "MS:1000524";

        /**
         * 访问号: 谱图表示 (MS:1000525)。可选子元素，最多提供一个
         */
        public static readonly string SPECTRUM_REPRESENTATION_ID = "MS:1000525";       

        /**
         * 访问号: 质谱 (MS:1000294)。
         */
        public static readonly string MASS_SPECTRUM_ID = "MS:1000294";

        /**
         * 访问号: 二进制类型 (IMS:1000003)。
         */
        public static readonly string BINARY_TYPE_ID = "IMS:1000003";

        /**
         * 访问号: 二进制类型 (连续) (IMS:1000030)。
         */
        public static readonly string BINARY_TYPE_CONTINUOUS_ID = "IMS:1000030";

        /**
         * 访问号: 二进制类型 (处理过) (IMS:1000031)。
         */
        public static readonly string BINARY_TYPE_PROCESSED_ID = "IMS:1000031";

        /**
         * 访问号: IBD 识别 (IMS:1000008)。
         */
        public static readonly string IDB_IDENTIFICATION_ID = "IMS:1000008";

        /**
         * 访问号: UUID识别 (IMS:1000080)。
         */
        public static readonly string UUID_IDENTIFICATION_ID = "IMS:1000080";

        /**
         * 访问号: IBD校验和 (IMS:1000009)。
         */
        public static readonly string IBD_CHECKSUM_ID = "IMS:1000009";

        /**
         * 访问号: MD5校验和 (IMS:1000090)。
         */
        public static readonly string MD5_CHECKSUM_ID = "IMS:1000090";

        /**
         * 访问号: SHA-1校验和 (IMS:1000091)。
         */
        public static readonly string SHA1_CHECKSUM_ID = "IMS:1000091";

        /**
         * 访问号: IBD文件 (IMS:1000007)。
         */
        public static readonly string IBD_FILE_ID = "IMS:1000007";

        /**
         * 创建空的{@literal <fileContent>}标签。
         */
        public FileContent() : base()
        {

        }

        /**
         * 复制构造函数。
         * 
         * @param fileContent 旧的{@literal <fileContent>}标签以复制
         * @param rpgList 新的ReferenceableParamGroupList用于引用
         */
        public FileContent(FileContent fileContent, ReferenceableParamGroupList rpgList) : base(fileContent, rpgList)
        {
        }

        /**
         * 返回所有具有子术语的CV参数
         * {@link FileContent#DATA_FILE_CONTENT_ID}本体参数。
         * 
         * @return CVParam列表
         */
        public List<CVParam> GetDataFileContents()
        {
            return GetChildrenOf(DATA_FILE_CONTENT_ID, false);
        }

        public override string GetTagName()
        {
            return "fileContent";
        }

        /**
         * 创建默认的有效(根据imzML)FileContent。默认文件内容
         * CV参数是质谱 ({@link FileContent#MASS_SPECTRUM_ID})。
         * 
         * @return 默认有效的FileContent
         */
        public static FileContent Create()
        {
            FileContent fc = new FileContent();

            fc.AddCVParam(new EmptyCVParam(OBO.GetOBO().GetTerm(MASS_SPECTRUM_ID)));

            return fc;
        }
    }
}
