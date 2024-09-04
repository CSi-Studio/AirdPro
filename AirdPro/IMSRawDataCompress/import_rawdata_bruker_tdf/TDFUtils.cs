using AirdPro.Domains;
using AirdPro.IMSRawDataCompress.datamodel;
using AirdPro.IMSRawDataCompress.datamodel.enums;
using AirdPro.IMSRawDataCompress.import_rawdata_bruker_tdf.datamodel;
using AirdPro.IMSRawDataCompress.import_rawdata_bruker_tdf.datamodel.sql;
using AirdPro.IMSRawDataCompress.preference;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AirdPro.IMSRawDataCompress.import_rawdata_bruker_tdf
{
    public class TDFUtils
    {
        // 定义常量
        //public const int SCAN_PACKAGE_SIZE = 5_000;
        //public const int BUFFER_SIZE_INCREMENT = 100_000; // 每次失败时增加100KB

        // 日志记录器
        private static readonly ILogger Logger = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        }).CreateLogger<TDFUtils>();

        // 保留RT格式的NumberFormatInfo实例（C#中用于格式化数字）
        //private readonly NumberFormatInfo rtFormat = CultureInfo.InvariantCulture.NumberFormat;

        // 用于缓存索引到m/z值的字典
        //private readonly Dictionary<int, double> indexToMzBuffer = new();

        // 用于缓存索引映射的字典
        //private readonly Dictionary<int, int> indicesToIndexMap = new();

        // 初始缓冲区大小
        //public int BUFFER_SIZE { get; } = 300000; // 初始大小300KB

        // TDF库的实例
        private TDFLibrary tdfLib = null;

        // 用于存储文件路径的字段
        //private FileInfo file;

        private long handle = 0L;

        private object tdfLibLock = new();

        public IntPtr Handle { get; set; }

        // TDFUtils的构造函数
        public TDFUtils()
        {
            // 初始化 TDFLibrary 实例
            tdfLib = new TDFLibrary();
            Logger.LogInformation("TDFUtils initialized.");
        }

        public void Close()
        {
            if (tdfLib != null && handle != 0L)
            {
                TDFLibrary.tims_close(handle);
            }
            handle = 0L;
            //file = null;
        }

        public long OpenFile(String fileName)
        {
            return TDFLibrary.tims_open_v2(fileName, 1, 0);
        }



        public double[] ConvertScanNumsToMobilities(long handle, long frameId, double[] scanNum)
        {
            double[] mobilities = new double[scanNum.Length];
            long error = TDFLibrary.tims_scannum_to_oneoverk0(handle, frameId, scanNum, mobilities, scanNum.Length);
            if (error == 0L)
            {
                Logger.LogError($"Error converting scan numbers to one over k0: {error}");
                return null;
            }
            else
            {
                return mobilities;
            }
        }

        public Frame extractProfileScanForFrame(TimsData timsData, long frameId, TDFMetaDataTable metaDataTable, TDFFrameTable frameTable, FramePrecursorTable framePrecursorTable)
        {
            throw new NotImplementedException();
        }

        public Frame extractCentroidScanForTimsFrame(TimsData timsData, long frameId, TDFMetaDataTable metaDataTable, TDFFrameTable frameTable, FramePrecursorTable framePrecursorTable)
        {
            int frameIndex = frameTable.GetFrameIdColumn().GetValueList().IndexOf(frameId);
            long numScans = frameTable.GetNumScansColumn().GetValueList()[frameIndex];
            double rt = frameTable.GetTimeColumn().GetValueList()[frameIndex] / 60; // 将秒转换为分钟
            PolarityType polarityType = PolarityType.parseFromString(frameTable.GetPolarityColumn().GetValueList()[frameIndex]);
            int msLevel = getMsLevleFromBrukerMsMsType(frameTable.GetMsMsTypeColumn().GetValueList()[frameIndex]);
            string scanDefinition = metaDataTable.GetInstrumentType()
                + "-" + BrukerScanMode.FromScanMode(frameTable.GetScanModeColumn().GetValueList()[frameIndex])
                + "Frame #" + frameId + "RT: " + rt.ToString(TimsPreferences.rtFormat.FormatProvider);
            double accumulationTime = frameTable.GetAccumulationTimeColumn().GetValueList()[frameIndex];
            Range<double> mzRange = metaDataTable.GetMzRange();

            Frame frame = new Frame(frameId, msLevel, rt, null, null, MassSpectrumType.CENTROIDED,
                polarityType, scanDefinition, mzRange, MobilityType.TIMS, null, accumulationTime);
            // filters do not contain this frame  ###########

            // load data after filters applied?????????
            //SimpleSpectralArrays data = extractCentroidsForFrame(frameId, 0, numScans);

            // process data?

            // finally set data and mobilities
            //frame.SetDataPoints(data.Mzs, data.Intensities);
            //int frameIndex = frameTable.GetFrameIdColumn().GetValueList().IndexOf(frame.FrameId);
            //long numScans = frameTable.GetNumScansColumn().GetValueList()[frameIndex];
            //mobility
            double[] scanNum = CreatePopulatedArrayFrom1(numScans);
            double[] mobilities = ConvertScanNumsToOneOverK0(handle, frameId, scanNum);
            return frame;
        }

        private double[] CreatePopulatedArrayFrom1(long numScans)
        {
            double[] scanNum = new double[numScans];
            for (long i = 0; i < numScans; i++)
            {
                scanNum[i] = i + 1;
            }
            return scanNum;
        }

        public double[] ConvertScanNumsToOneOverK0(long handle, long frameId, double[] scanNum)
        {
            double[] mobilities = new double[scanNum.Length];
            // 将int数组转换为double数组
            //double[] scanNumsAsDoubles = scanNum.Select(x => (double)x).ToArray();
            long error = TDFLibrary.tims_scannum_to_oneoverk0(handle, frameId, scanNum, mobilities, scanNum.Length);
            if (error == 0L)
            {
                Logger.LogError($"Error converting scan numbers to one over k0: {error}");
                return null;
            }
            else
            {
                return mobilities;
            }
        }

        public static int getMsLevleFromBrukerMsMsType(long msMsType)
        {
            switch (msMsType)
            {
                case 0:
                    return 1;
                case 2:
                case 9:
                case 10:
                case 8:
                    return 2;
                default:
                    return 0;
            }
        }

    }
}
