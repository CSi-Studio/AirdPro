using AirdPro.ImzMLParser.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.ImzMLParser.mzml
{
    public class SourceFile : MzMLContentWithParams, IReferenceableTag
    {
        private const long serialVersionUID = 1L;

        /**
         * 访问号: 本地光谱标识符格式 (MS:1000767)。
         */
        public static readonly string NATIVE_SPECTRUM_IDENTIFIER_FORMAT_ID = "MS:1000767";  // 必需子元素 (1)

        /**
         * 访问号: 数据文件校验和类型 (MS:1000561)。
         */
        public static readonly string DATA_FILE_CHECKSUM_TYPE_ID = "MS:1000561";    // 必需子元素 (1+)
        /**
         * 访问号: 质谱仪文件格式 (MS:1000560)。
         */
        public static readonly string MASS_SPECTROMETER_FILE_FORMAT_ID = "MS:1000560"; // 必需子元素 (1)

        /**
         * 访问号: mzML格式 (MS:1000584)。
         */
        public static readonly string MZML_FILE_FORMAT_ID = "MS:1000584";

        /**
         * 访问号: SHA-1文件校验和 (MS:1000569)。
         */
        public static readonly string SHA1_FILE_CHECKSUM_ID = "MS:1000569";

        /**
         * 源文件的唯一标识符。
         */
        public string id;          // 必需

        /**
         * 源文件的位置。
         */
        public string location;    // 必需

        /**
         * 源文件的名称。
         */
        public string name;        // 必需

        /**
         * 使用指定的唯一ID、位置和文件名创建源文件。
         * 
         * @param id 源文件的唯一标识符
         * @param location 源文件的位置
         * @param name 源文件的名称
         */
        public SourceFile(string id, string location, string name)
        {
            this.id = id;
            this.location = location;
            this.name = name;
        }

        /**
         * 复制构造函数。
         *
         * @param sourceFile 要复制的旧SourceFile
         * @param rpgList 新的ReferenceableParamGroupList以匹配引用
         */
        public SourceFile(SourceFile sourceFile, ReferenceableParamGroupList rpgList) : base(sourceFile, rpgList)
        {
            this.id = sourceFile.id;
            this.location = sourceFile.location;
            this.name = sourceFile.name;
        }

        public string GetID()
        {
            return id;
        }

        public void SetID(string id)
        {
            this.id = id;
        }

        /**
         * 返回源文件的位置。
         * 
         * @return 源文件位置
         */
        public string GetLocation()
        {
            return location;
        }

        /**
         * 返回源文件的名称。
         * 
         * @return 源文件名称
         */
        public string GetName()
        {
            return name;
        }

        public override string GetXMLAttributeText()
        {
            string attributeText = $"id=\"{XMLHelper.EnsureSafeXML(id)}\"";
            attributeText += $"location=\"{XMLHelper.EnsureSafeXML(location)}\"";
            attributeText += $"name=\"{XMLHelper.EnsureSafeXML(name)}\"";
            return attributeText;
        }
        public override string ToString()
        {
            return $"sourceFile: id=\"{id}\" location=\"{location}\" name=\"{name}\"";
        }

        public override string GetTagName()
        {
            return "sourceFile";
        }
    }
}
