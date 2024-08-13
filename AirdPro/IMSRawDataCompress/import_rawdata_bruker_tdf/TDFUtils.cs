using AirdPro.Constants;
using AirdPro.IMSRawDataCompress.datamodel.callbacks;
using AirdPro.IMSRawDataCompress.datamodel.sql;
using CSharpFastPFOR.Port;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThermoFisher.CommonCore.Data.Business;

namespace AirdPro.IMSRawDataCompress.datamodel
{
    public class TDFUtils
    {
        // 定义常量
        public const int SCAN_PACKAGE_SIZE = 5_000;
        public const int BUFFER_SIZE_INCREMENT = 100_000; // 每次失败时增加100KB

        // 日志记录器
        private static readonly ILogger Logger = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        }).CreateLogger<TDFUtils>();

        // 保留RT格式的NumberFormatInfo实例（C#中用于格式化数字）
        private readonly NumberFormatInfo rtFormat = CultureInfo.InvariantCulture.NumberFormat;

        // 用于缓存索引到m/z值的字典
        private readonly Dictionary<int, double> indexToMzBuffer = new Dictionary<int, double>();

        // 用于缓存索引映射的字典
        private readonly Dictionary<int, int> indicesToIndexMap = new Dictionary<int, int>();

        // 初始缓冲区大小
        public int BUFFER_SIZE { get; } = 300000; // 初始大小300KB

        // TDF库的实例（假设TDFLibrary是一个定义了与原生TDF库交互的类）
        private TDFLibrary tdfLib = null;

        // 用于存储文件路径的字段
        private FileInfo file;

        private long handle = 0L;

        private object tdfLibLock = new object();

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
            file = null;
        }

        public long openFile(String fileName)
        {
            //long handle = TDFLibrary.tims_open(fileName, 2);
            long handle = TDFLibrary.tims_open_v2(fileName, 1, 0);
            return handle;
        }

        public CentroidData extractCentroidsForFrame(long handle, long frameId, int startScanNum, int endScanNum)
        {
            if (handle == 0L)
            {
                throw new InvalidOperationException("No TDF data file opened yet.");
            }

            CentroidData data = new CentroidData();

            lock (tdfLibLock)
            {
                long error = TDFLibrary.tims_extract_centroided_spectrum_for_frame_v2(handle, 2, 1, 667, data, IntPtr.Zero);
                if (error == 0L)
                {
                    Logger.LogError($"Error extracting centroided spectrum for frame: {error}");
                    return null;
                }
                else
                {
                    return data;
                }

            }
        }

        public double[] convertScanNumsToMobilities(long handle, long frameId, int[] scanNums)
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
