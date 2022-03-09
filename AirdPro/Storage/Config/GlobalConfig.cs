namespace AirdPro.Domains.Convert;

public class GlobalConfig
{
    public string lastOpenPath;
    public string redisHost;
    public string redisPort;

    public GlobalConfig()
    {
        this.redisHost = "127.0.0.1";
        this.redisPort = "6379";
    }

    public GlobalConfig(string lastOpenPath, string redisHost, string redisPort)
    {
        this.lastOpenPath = lastOpenPath;
        this.redisHost = redisHost;
        this.redisPort = redisPort;
    }
}