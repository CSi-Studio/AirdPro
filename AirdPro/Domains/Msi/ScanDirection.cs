namespace AirdPro.Domains.Msi
{   
    public class ScanDirection(string name)
    {
        public string Name { get; } = name;

        // 枚举成员
        public enum Type
        {
            HORIZONTAL,
            VERTICAL
        }

        // 相关联的类实例
        public static readonly ScanDirection HORIZONTAL = new("Horizontal");
        public static readonly ScanDirection VERTICAL = new("Vertical");
    }
}
