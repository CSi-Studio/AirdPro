using System.Collections.Generic;
using AirdPro.Domains.Convert;

namespace AirdPro.Storage;

public interface Subject<T>
{
    public void attach(Observer<T> observer);
    public void detach(Observer<T> observer);

    public void notify();
}