namespace AirdPro.IMSRawDataCompress.datamodel
{
    public class MobilityType
    {
        public string AxisLabel { get; }
        public string Unit { get; }
        public string Name { get; }

        public MobilityType(string axisLabel, string unit, string name)
        {
            AxisLabel = axisLabel;
            Unit = unit;
            Name = name;
        }

        // 枚举成员
        public enum Type
        {
            NONE,
            MIXED,
            OTHER,
            TIMS,
            DRIFT_TUBE,
            TRAVELING_WAVE,
            FAIMS
        }

        // 相关联的类实例
        public static readonly MobilityType NONE = new MobilityType("none", "none", "none");
        public static readonly MobilityType MIXED = new MobilityType("none", "none", "mixed");
        public static readonly MobilityType OTHER = new MobilityType("none", "none", "other IMS");
        public static readonly MobilityType TIMS = new MobilityType("1/k0", "Vs/cm^2", "TIMS");
        public static readonly MobilityType DRIFT_TUBE = new MobilityType("Drift time", "ms", "DTIMS");
        public static readonly MobilityType TRAVELING_WAVE = new MobilityType("Drift time", "ms", "TWIMS");
        public static readonly MobilityType FAIMS = new MobilityType("TODO", "TODO", "FAIMS");
    }

}
