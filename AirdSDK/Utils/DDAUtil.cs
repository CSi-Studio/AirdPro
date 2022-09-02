using AirdSDK.Beans;
using CSharpFastPFOR.Port;

namespace AirdPro.Utils;

public class DDAUtil
{
    public static void initFromIndex(DDAMs ms, BlockIndex index, int loc)
    {
        if (index.nums != null && index.nums.Count > 0)
        {
            ms.num = index.nums[loc];
        }

        if (index.cvList != null && index.cvList.Count > 0)
        {
            ms.cvList = index.cvList[loc];
        }

        if (index.tics != null && index.tics.Count > 0)
        {
            ms.tic = index.tics[loc];
        }

        if (index.rangeList != null && index.rangeList.Count > 0)
        {
            ms.range = index.rangeList[loc];
        }
    }

    public static void initFromIndex(DDAPasefMs ms, BlockIndex index, int loc)
    {
        if (index.nums != null && index.nums.Count > 0)
        {
            ms.num = index.nums[loc];
        }

        if (index.cvList != null && index.cvList.Count > 0)
        {
            ms.cvList = index.cvList[loc];
        }

        if (index.tics != null && index.tics.Count > 0)
        {
            ms.tic = index.tics[loc];
        }

        if (index.rangeList != null && index.rangeList.Count > 0)
        {
            ms.range = index.rangeList[loc];
        }
    }
}