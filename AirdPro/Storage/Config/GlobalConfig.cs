namespace AirdPro.Domains.Convert;

public class GlobalConfig
{
    public string defaultOpenPath;
    public string redisHost;
    public string redisPort;

    public GlobalConfig()
    {
        this.redisHost = "127.0.0.1";
        this.redisPort = "6379";
    }

    public GlobalConfig(string defaultOpenPath, string redisHost, string redisPort)
    {
        this.defaultOpenPath = defaultOpenPath;
        this.redisHost = redisHost;
        this.redisPort = redisPort;
    }
}