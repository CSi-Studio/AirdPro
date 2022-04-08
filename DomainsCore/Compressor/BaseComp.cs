namespace AirdSDK.Compressor;

public abstract class BaseComp<T>
{
    public abstract string getName();
    public abstract T[] encode(T[] uncompressed);
    public abstract T[] decode(T[] compressed);
}