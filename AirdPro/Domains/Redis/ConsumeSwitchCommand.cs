using System.Collections.Generic;

namespace AirdPro.Domains.Redis;

public class ConsumeSwitchCommand
{ 
    public List<string> serverIps = new List<string>(); 
    public bool switcher;
}