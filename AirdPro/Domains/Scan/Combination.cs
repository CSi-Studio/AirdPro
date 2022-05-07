using AirdPro.Constants;
using AirdPro.Storage.Config;
using AirdSDK.Compressor;

namespace AirdPro.Domains;

public class Combination
{
    //必须是mz_IVB_Zstd这种格式的字符串
    private string mz;

    //必须是intensity_VB_Zstd这种格式的字符串
    private string intensity;

    //必须是mobi_VB_Zstd这种格式的字符串
    private string mobi;

    public Combination(string mz, string intensity)
    {
        this.mz = mz;
        this.intensity = intensity;
    }

    public Combination(string mz, string intensity, string mobi)
    {
        this.mz = mz;
        this.intensity = intensity;
        this.mobi = mobi;
    }

    public ConversionConfig enable(ConversionConfig config)
    {
        if (config == null)
        {
            config = new ConversionConfig();
        }

        if (mz != null && mz.StartsWith(Tag.Key_MZ))
        {
            string[] mzCompArray = mz.Split(Const.Char_Dash);
            config.mzIntComp = SortedIntComp.getType(mzCompArray[1]);
            config.mzByteComp = ByteComp.getType(mzCompArray[2]);
        }

        if (intensity != null && intensity.StartsWith(Tag.Key_Intensity))
        {
            string[] intCompArray = intensity.Split(Const.Char_Dash);
            config.intIntComp = IntComp.getType(intCompArray[1]);
            config.intByteComp = ByteComp.getType(intCompArray[2]);
        }

        if (mobi != null && mobi.StartsWith(Tag.Key_Mobi))
        {
            string[] mobiCompArray = mobi.Split(Const.Char_Dash);
            config.mobiIntComp = IntComp.getType(mobiCompArray[1]);
            config.mobiByteComp = ByteComp.getType(mobiCompArray[2]);
        }

        return config;
    }
}