namespace AirdPro.Domains.Msi
{   
    public class VerticalStart(string name)
    {
        public string Name { get; } = name;

        // 枚举成员
        public enum Type
        {
            TOP,
            BOTTOM
        }

        // 相关联的类实例
        public static readonly VerticalStart TOP = new("Top");
        public static readonly VerticalStart BOTTOM = new("Bottom");
    }
}
