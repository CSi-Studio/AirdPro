using System.Collections.Generic;
using AirdPro.Domains.Convert;

namespace AirdPro.Storage;

public interface Subject
{
    public void attach(Observer<Dictionary<string, ConversionConfig>> observer);
    public void detach(Observer<Dictionary<string, ConversionConfig>> observer);

    public void notify();
}