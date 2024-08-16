using AirdPro.IMSRawDataCompress.datamodel.callbacks;
using AirdPro.IMSRawDataCompress.import_rawdata_bruker_tdf.datamodel;
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

        public void close()
        {
            if (tdfLib != null && handle != 0L)
            {
                TDFLibrary.tims_close(handle);
            }
            handle = 0L;
            //file = null;
        }

        public long openFile(String fileName)
        {
            return TDFLibrary.tims_open_v2(fileName, 1, 0);
        }



        public double[] convertScanNumsToMobilities(long handle, long frameId, long[] scanNums)
        {
            double[] mobilities = new double[scanNums.Length];
            // 将int数组转换为double数组
            double[] scanNumsAsDoubles = scanNums.Select(x => (double)x).ToArray();
            long error = TDFLibrary.tims_scannum_to_oneoverk0(handle, frameId, scanNumsAsDoubles, mobilities, scanNums.Length);
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
    }
}
