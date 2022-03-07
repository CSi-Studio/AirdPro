using System.Collections.Generic;
using AirdPro.Domains.Convert;
using AirdPro.Domains.Job;

namespace AirdPro.Storage;

public interface Observer
{
    public void update(Dictionary<string, ConversionConfig> configMap);
}