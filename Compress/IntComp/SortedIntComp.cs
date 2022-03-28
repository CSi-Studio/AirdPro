using System;
using Compress.Enums;

namespace Compress;

public abstract class SortedIntComp
{
    public abstract string getName();
    public abstract int[] encode(int[] uncompressed);
    public abstract int[] decode(int[] compressed);

    public static SortedIntComp build(SortedIntCompType type)
    {
        switch (type)
        {
            case SortedIntCompType.IBP:
                return new IntegratedBinPackingWrapper();
            case SortedIntCompType.IVB:
                return new IntegratedVarByteWrapper();
            default: throw new Exception("No Implementation for " + type);
        }
    }
}