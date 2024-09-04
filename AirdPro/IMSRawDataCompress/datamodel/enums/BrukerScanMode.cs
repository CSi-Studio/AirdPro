using System.Linq;
using System.Reflection;

namespace AirdPro.IMSRawDataCompress.datamodel.enums 
{
    public class BrukerScanMode
    {
        public long Num { get; }
        public string Description { get; }

        public BrukerScanMode(long num, string description)
        {
            Num = num;
            Description = description;
        }

        // 枚举成员
        public enum Type
        {
            MS,
            AUTO_MSMS,
            MRM,
            IN_SOURCE_CID,
            BROADBAND_CID,
            PASEF,
            DIA,
            PRM,
            MALDI,
            UNKNOWN
        }

        // 相关联的类实例
        public static readonly BrukerScanMode MS = new BrukerScanMode(0, "MS");
        public static readonly BrukerScanMode AUTO_MSMS = new BrukerScanMode(1, "Auto MS/MS");
        public static readonly BrukerScanMode MRM = new BrukerScanMode(2, "MRM");
        public static readonly BrukerScanMode IN_SOURCE_CID = new BrukerScanMode(3, "in-source CID");
        public static readonly BrukerScanMode BROADBAND_CID = new BrukerScanMode(4, "broadband CID");
        public static readonly BrukerScanMode PASEF = new BrukerScanMode(8, "PASEF");
        public static readonly BrukerScanMode DIA = new BrukerScanMode(9, "DIA");
        public static readonly BrukerScanMode PRM = new BrukerScanMode(10, "PRM");
        public static readonly BrukerScanMode MALDI = new BrukerScanMode(20, "MALDI");
        public static readonly BrukerScanMode UNKNOWN = new BrukerScanMode(-1, "Unknown");

        public static BrukerScanMode FromScanMode(long num)
        {
            // 使用 LINQ 查找匹配的模式
            var mode = typeof(BrukerScanMode)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(f => f.FieldType == typeof(BrukerScanMode))
                .Select(f => (BrukerScanMode)f.GetValue(null))
                .FirstOrDefault(m => m.Num == num);
            return mode ?? UNKNOWN; // 如果没有找到，返回UNKNOWN
        }

    }
}