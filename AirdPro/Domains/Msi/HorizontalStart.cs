namespace AirdPro.Domains.Msi
{
    public class HorizontalStart(string name)
    {
        public string Name { get; } = name;

        // 枚举成员
        public enum Type
        {
            LEFT,
            RIGHT
        }

        // 相关联的类实例
        public static readonly HorizontalStart LEFT = new("Left");
        public static readonly HorizontalStart RIGHT = new("Right");
    }
}
