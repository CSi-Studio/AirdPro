namespace AirdPro.IMSRawDataCompress.datamodel.msms
{
    public class ActivationMethod
    {
        public string Abbreviation { get; }
        public string Name { get; }
        public string Unit { get; }

        public ActivationMethod(string abbreviation, string unit, string name)
        {
            Abbreviation = abbreviation;
            Name = name;
            Unit = unit;            
        }

        // 枚举成员
        public enum Type
        {
            CID, 
            HCD, 
            ECD, 
            ETD,
            UNKNOWN
        }

        // 相关联的类实例
        public static readonly ActivationMethod CID = new ActivationMethod("CID", "collision induced dissociation", "eV");
        public static readonly ActivationMethod HCD = new ActivationMethod("HCD", "higher-energy C-trap dissociation", "a.u.");
        public static readonly ActivationMethod ECD = new ActivationMethod("ECD", "electron capture dissociation", "");
        public static readonly ActivationMethod ETD = new ActivationMethod("ETD", "electron transfer dissociation", "");
        public static readonly ActivationMethod UNKNOWN = new ActivationMethod("N.A.", "Unknown", "");

    }
}
