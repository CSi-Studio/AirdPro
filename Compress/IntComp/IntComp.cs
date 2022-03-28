using System;
using Compress.Enums;

namespace Compress;

public abstract class IntComp
{
    public abstract string getName();
    public abstract int[] encode(int[] uncompressed);
    public abstract int[] decode(int[] compressed);

    public static IntComp build(IntCompType type)
    {
        switch (type)
        {
            case IntCompType.VB:
                return new VarByteWrapper();
            case IntCompType.BP:
                return new BinPackingWrapper();
            case IntCompType.Simple:
                return new Simple16Wrapper();
            case IntCompType.NewPFD:
                return new NewPFDS16Wrapper();
            case IntCompType.OptPFD:
                return new OptPFDS16Wrapper();
            case IntCompType.Empty:
                return new Empty();
            default: throw new Exception("No Implementation for " + type);
        }
    }
}