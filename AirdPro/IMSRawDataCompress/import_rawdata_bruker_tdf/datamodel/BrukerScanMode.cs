namespace AirdPro.IMSRawDataCompress.import_rawdata_bruker_tdf.datamodel
{
    public enum BrukerScanMode
    {
        [Description("MS")]
        MS = 0,
        [Description("Auto MS/MS")]
        AUTO_MSMS = 1,
        [Description("MRM")]
        MRM = 2,
        [Description("in-source CID")]
        IN_SOURCE_CID = 3,
        [Description("broadband CID")]
        BROADBAND_CID = 4,
        [Description("PASEF")]
        PASEF = 8,
        [Description("DIA")]
        DIA = 9,
        [Description("PRM")]
        PRM = 10,
        [Description("MALDI")]
        MALDI = 20,
        [Description("Unknown")]
        UNKNOWN = -1         
    }
}