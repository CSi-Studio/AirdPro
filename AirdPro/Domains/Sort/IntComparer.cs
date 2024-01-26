using System.Collections.Generic;

namespace AirdPro.Domains;

public class IntComparer : IEqualityComparer<int>
{
    public bool Equals(int x, int y)
    {
        // 自定义相等比较逻辑
        return x == y;
    }

    public int GetHashCode(int obj)
    {
        // 自定义哈希函数
        return obj;
    }
}