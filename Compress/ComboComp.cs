using AirdPro.Algorithms;

namespace Compress;

public class ComboComp
{
    public static byte[] encode(ByteComp byteComp, int[] target)
    {
        return byteComp.encode(ByteTrans.intToByte(target));
    }

    public static int[] decode(ByteComp byteComp, byte[] target)
    {
        return ByteTrans.byteToInt(byteComp.decode(target));
    }

    public static byte[] encode(IntComp intComp, ByteComp byteComp, int[] target)
    {
        return byteComp.encode(ByteTrans.intToByte(intComp.encode(target)));
    }

    public static int[] decode(IntComp intComp, ByteComp byteComp, byte[] target)
    {
        return intComp.decode(ByteTrans.byteToInt(byteComp.decode(target)));
    }
}