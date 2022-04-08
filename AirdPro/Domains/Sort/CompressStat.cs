namespace AirdPro.Domains;

public class CompressStat
{
    public string key;
    public long size;
    public long compressTime;
    public long decompressTime;

    public CompressStat(string key, long size, long compressTime, long decompressTime)
    {
        this.key = key;
        this.size = size;
        this.compressTime = compressTime;
        this.decompressTime = decompressTime;
    }
}