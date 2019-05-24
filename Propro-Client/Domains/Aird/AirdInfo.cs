using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Propro.Structs;

namespace Propro.Domains
{
    class AirdInfo
    {
        //仪器设备信息
        public InstrumentInfo instrumentInfo;
        //父文件信息
        public ParentFileInfo parentFileInfo;
        //DIA_SWATH,PRM
        public string type;
        //mz数组采用了pfor+zlib的压缩,精确到小数点后2位,intensity数组采用了zlib压缩,精确到小数点后1位
        public Hashtable strategies = new Hashtable();
        //是否忽略Intensity为0的键值对
        public Boolean ignoreZeroIntensity = true;

        //枚举值,LITTLE_ENDIAN和BIG_ENDIAN两种
        public string byteOrder = "LITTLE_ENDIAN";
        //转换压缩后的aird的文件路径
        public string airdPath;
        //实验的描述
        public string description;
        //从raw文件中得到的msd.id
        public string rawId;
        //实验的创建者
        public string creator;
        //实验的创建日期
        public DateTime createDate;
        //Swaht的各个窗口间的重叠部分
        public float overlap;
        //Store the window rangs which have been adjusted with experiment overlap
        public List<WindowRange> rangeList;
        //the whole new scan index for new format file
        public List<ScanIndex> scanIndexList;
        //the swath window location(start and and) for new format file
        public List<ScanIndex> swathIndexList;
    }
}
