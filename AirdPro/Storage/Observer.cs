using System.Collections.Generic;
using AirdPro.Domains.Convert;
using AirdPro.Domains.Job;

namespace AirdPro.Storage;

public interface Observer<T>
{
    public void update(T configs);
}