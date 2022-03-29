using System;
using System.Net;

namespace AirdPro.Utils;

public class NetworkUtil
{
    //获取本地IP的方法
    public static string getHostIP()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return Convert.ToString(ip);
            }
        }
        return "Unknown IP Address";
    }
}