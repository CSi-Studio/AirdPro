using AirdPro.Constants;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        // TDFUtils的构造函数
        public TDFUtils()
        {
            // 初始化 TDFLibrary 实例
            tdfLib = new TDFLibrary();
            
        }
    }
   
}
