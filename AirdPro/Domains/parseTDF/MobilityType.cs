using AirdPro.Constants;
using Google.Protobuf.WellKnownTypes;
using pwiz.CLI.tradata;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Protobuf.Compiler.CodeGeneratorResponse.Types;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Web.UI.WebControls;
using static pwiz.CLI.chemistry.MZTolerance;

namespace AirdPro.Domains.parseTDF
{
    public enum MobilityType
    {
        None, // 等同于 NONE
        Mixed, // 等同于 MIXED
        Other, // 等同于 OTHER
        Tims, // 等同于 TIMS
        DriftTube, // 等同于 DRIFT_TUBE
        TravelingWave, // 等同于 TRAVELING_WAVE
        Faims // 等同于 FAIMS
    }

    // 为枚举类型添加扩展方法
    public static class MobilityTypeExtensions
    {
        // 存储每个枚举值的额外信息
        private static readonly (string axisLabel, string unit, string name)[] MobilityTypeInfos =
        {
            ("none", "none", "none"),
            ("none", "none", "mixed"),
            ("none", "none", "other IMS"),
            ("1/k0", "Vs/cm^2", "TIMS"),
            ("Drift time", "ms", "DTIMS"),
            ("Drift time", "ms", "TWIMS"),
            ("TODO", "TODO", "FAIMS")
        };

        // 获取枚举值的轴标签
        public static string GetAxisLabel(this MobilityType type)
        {
            var info = MobilityTypeInfos[(int)type];
            // 这里可以添加获取单位格式的逻辑
            return $"Mobility ({info.axisLabel})";
        }

        // 获取枚举值的单位
        public static string GetUnit(this MobilityType type)
        {
            return MobilityTypeInfos[(int)type].unit;
        }

        // 重写ToString方法
        public static string ToStringCustom(this MobilityType type)
        {
            return MobilityTypeInfos[(int)type].name;
        }

        // 检查特性是否匹配此类型
        // 这里需要Feature类的定义，假设它有一个名为MobilityType的属性
        public static bool IsTypeOf(this MobilityType type, Feature f)
        {
            return f.MobilityType == type;
        }

        // 检查原始数据文件是否匹配此类型
        // 这里需要RawDataFile类的定义，假设它有一个名为MobilityType的属性
        public static bool IsTypeOf(this MobilityType type, RawDataFile raw)
        {
            return raw is IMSRawDataFile imsfile && imsfile.MobilityType == type;
        }

        // 检查特性或其原始数据文件的类型是否匹配
        public static bool IsTypeOfBackingRawData(this MobilityType type, Feature f)
        {
            return IsTypeOf(type, f) || IsTypeOf(type, f.RawDataFile);
        }

        // 根据枚举值获取特定的字符串
        public static string GetCcsBaseEntryString(this MobilityType type)
        {
            return type switch
            {
                MobilityType.None or MobilityType.Faims or MobilityType.Other or MobilityType.Mixed => null,
                MobilityType.Tims => "TIMS",
                MobilityType.DriftTube => "DT",
                MobilityType.TravelingWave => "TW",
                _ => throw new ArgumentException($"Unhandled MobilityType: {type}")
            };
        }
    }
}