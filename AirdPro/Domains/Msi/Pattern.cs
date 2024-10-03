namespace AirdPro.Domains.Msi
{
    public class Pattern(string name)
    {
        public string Name { get; } = name;

        // 枚举成员
        public enum Type
        {
            MEANDER,
            FLY_BACK,
            RANDOM
        }

        // 相关联的类实例
        public static readonly Pattern MEANDER = new("Meander");
        public static readonly Pattern FLY_BACK = new("Fly Back");
        public static readonly Pattern RANDOM = new("Random");
    }
}
