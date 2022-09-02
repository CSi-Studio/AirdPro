namespace AirdSDK.Compressor;

public class Xor
{
    /**
     * Apply differential coding (in-place).
     *
     * @param data data to be modified
     */
    public static int[] xor(int[] data)
    {
        int[] res = new int[data.Length];
        for (int i = data.Length - 1; i > 0; --i)
        {
            res[i] = data[i] ^ data[i - 1];
        }

        res[0] = data[0];
        return res;
    }

    /**
     * Undo differential coding (in-place). Effectively computes a prefix sum.
     *
     * @param data to be modified.
     */
    public static int[] recover(int[] data)
    {
        int[] res = new int[data.Length];
        res[0] = data[0];
        for (int i = 1; i < data.Length; ++i)
        {
            res[i] = data[i] - data[i - 1];
        }

        return res;
    }
}