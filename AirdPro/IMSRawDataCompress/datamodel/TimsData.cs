using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.IMSRawDataCompress.datamodel
{
    public class TimsData
    {
        // 记录日志
        public static readonly ILogger Logger = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        }).CreateLogger<TimsData>();

        private ObservableCollection<Frame> frames = new ObservableCollection<Frame>();
        private Dictionary<int, List<Scan>> frameNumbersCache = new Dictionary<int, List<Scan>>();
        private Dictionary<int, Range<double>> dataMobilityRangeCache = new Dictionary<int, Range<double>>();
        private Dictionary<int, List<Frame>> frameMsLevelCache = new Dictionary<int, List<Frame>>();
        IReadOnlyList<int> mobilitySegments = new List<int>().AsReadOnly();

        protected Range<double> mobilityRange;
        protected MobilityType mobilityType;
        public DateTime AcquisitionDateTime { get; set; }


    }
}
