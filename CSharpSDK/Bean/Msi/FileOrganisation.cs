namespace AirdSDK.Bean.Msi
{
    public class FileOrganisation(string name)
    {
        public string Name { get; } = name;

        public enum Type
        {
            ROW_PER_FILE,
            IMAGE_PER_FILE,
            SPECTRUM_PER_FILE
        }

        public static readonly FileOrganisation ROW_PER_FILE = new("row per file");
        public static readonly FileOrganisation IMAGE_PER_FILE = new("image per file");
        public static readonly FileOrganisation SPECTRUM_PER_FILE = new("spectrum per file");
    }
}
