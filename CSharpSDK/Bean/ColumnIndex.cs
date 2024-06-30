using System;
using System.Linq;

namespace AirdSDK.Beans;

public class ColumnIndex
{
    public int level;

    /**
     * the column start position in the file
     * 在文件中的开始位置
     */
    public long startPtr;

    /**
      * the column end position in the file
      * 在文件中的结束位置
      */
    public long endPtr;

    /**
     * 对应的前体范围，在DDA模式中本字段为空，因为DDA文件的二级数据块没有逻辑连续性，在面向列存储的场景下无意义
     * 在DIA模式下，如果level==1则本字段为空，如果level==2则本字段为对应的前体范围
     */
    public WindowRange range;

    public long startMzListPtr;
    public long endMzListPtr;

    public long startRtListPtr;
    public long endRtListPtr;

    public long startSpectraIdListPtr;
    public long endSpectraIdListPtr;

    public long startIntensityListPtr;
    public long endIntensityListPtr;

    public int[] mzs; //矩阵横坐标

    public int[] rts; //矩阵纵坐标

    public int[] spectraIds; //spectraId的数组文件坐标delta值

    public int[] intensities; //强度数组坐标delta值

    public long[] anchors; //坐标插值,用于减少mzIndex定位时的耗时
    public string toString()
    {
        if (level == 1)
        {
            return "MS1-Col:";
        }
        else
        {
            return "MS2-" + range.mz + ":";
        }
    }

    public ColumnIndexProto ToProto()
    {
        ColumnIndexProto proto = new ColumnIndexProto()
        {
            Level = this.level,
            StartPtr = this.startPtr,
            EndPtr = this.endPtr,
            Range = this.range?.ToProto(),
            StartMzListPtr = this.startMzListPtr,
            EndMzListPtr = this.endMzListPtr,
            StartRtListPtr = this.startRtListPtr,
            EndRtListPtr = this.endRtListPtr,
            StartSpectraIdListPtr = this.startSpectraIdListPtr,
            EndSpectraIdListPtr = this.endSpectraIdListPtr,
            StartIntensityListPtr = this.startIntensityListPtr,
            EndIntensityListPtr = this.endIntensityListPtr
        };
        if (mzs != null && mzs.Length > 0)
        {
            proto.Mzs.AddRange(this.mzs);
        }

        if (rts != null && rts.Length > 0)
        {
            proto.Rts.AddRange(this.rts);
        }

        if (spectraIds != null && spectraIds.Length > 0)
        {
            proto.SpectraIds.AddRange(this.spectraIds);
        }

        if (intensities != null && intensities.Length > 0)
        {
            proto.Intensities.AddRange(this.intensities);
        }

        if (anchors != null && anchors.Length > 0)
        {
            proto.Anchors.AddRange(this.anchors);
        }
       
        return proto;
    }
    
    public static ColumnIndex FromProto(ColumnIndexProto proto)
    {
        if (proto == null)
            throw new ArgumentNullException(nameof(proto));

        return new ColumnIndex
        {
            level = proto.Level,
            startPtr = proto.StartPtr,
            endPtr = proto.EndPtr,
            range = WindowRange.FromProto(proto.Range),
            startMzListPtr = proto.StartMzListPtr,
            endMzListPtr = proto.EndMzListPtr,
            startRtListPtr = proto.StartRtListPtr,
            endRtListPtr = proto.EndRtListPtr,
            startSpectraIdListPtr = proto.StartSpectraIdListPtr,
            endSpectraIdListPtr = proto.EndSpectraIdListPtr,
            startIntensityListPtr = proto.StartIntensityListPtr,
            endIntensityListPtr = proto.EndIntensityListPtr,
            mzs = proto.Mzs.ToArray(),
            rts = proto.Rts.ToArray(),
            spectraIds = proto.SpectraIds.ToArray(),
            intensities = proto.Intensities.ToArray(),
            anchors = proto.Anchors.ToArray()
        };
    }

}