namespace AirdPro.csimzMLParser.data
{
    public interface IDataTransform
    {
        byte[] ForwardTransform(byte[] data);
        byte[] ReverseTransform(byte[] data);
    }
}
