using System;
using System.Reflection;

namespace AirdPro.IMSRawDataCompress.import_rawdata_bruker_tdf.datamodel
{
    internal class EnumUtils
    {
        // 通过反射获取枚举值的描述
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description;
        }
    }

    // 自定义特性，用于存储枚举值的描述信息
    [AttributeUsage(AttributeTargets.Field)]
    public class DescriptionAttribute : Attribute
    {
        public string Description { get; }

        public DescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}
