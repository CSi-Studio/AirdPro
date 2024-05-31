namespace AirdSDK.Beans.Common;

public class ByteColumn
{
    public byte[] spectraIds;
    public byte[] intensities;

    public ByteColumn(byte[] spectraIds, byte[] intensities)
    {
        this.spectraIds = spectraIds;
        this.intensities = intensities;
    }
}