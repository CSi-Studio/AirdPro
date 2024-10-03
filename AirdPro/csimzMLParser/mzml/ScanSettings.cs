using System;
using System.Collections.Generic;

namespace AirdPro.csimzMLParser.mzml
{
    [Serializable] // 标记类为可序列化
    public class ScanSettings : MzMLContentWithParams, IReferenceableTag
    {
        private static readonly long serialVersionUID = 1L;

        // 定义常量
        public const string LINE_SCAN_DIRECTION_ID = "IMS:1000049";
        public const string LINE_SCAN_DIRECTION_LEFT_RIGHT_ID = "IMS:1000491";
        public const string LINE_SCAN_DIRECTION_RIGHT_LEFT_ID = "IMS:1000490";
        public const string LINE_SCAN_DIRECTION_TOP_DOWN_ID = "IMS:1000493";
        public const string LINE_SCAN_DIRECTION_BOTTOM_UP_ID = "IMS:1000492";
        public const string SCAN_DIRECTION_ID = "IMS:1000040";
        public const string SCAN_DIRECTION_TOP_DOWN_ID = "IMS:1000401";
        public const string SCAN_DIRECTION_BOTTOM_UP_ID = "IMS:1000400";
        public const string SCAN_DIRECTION_LEFT_RIGHT_ID = "IMS:1000402";
        public const string SCAN_DIRECTION_RIGHT_LEFT_ID = "IMS:1000403";
        public const string SCAN_PATTERN_ID = "IMS:1000041";
        public const string SCAN_PATTERN_FLYBACK_ID = "IMS:1000413";
        public const string SCAN_PATTERN_MEANDERING_ID = "IMS:1000410";
        public const string SCAN_PATTERN_RANDOM_ACCESS_ID = "IMS:1000412";
        public const string SCAN_TYPE_ID = "IMS:1000048";
        public const string SCAN_TYPE_HORIZONTAL_ID = "IMS:1000480";
        public const string SCAN_TYPE_VERTICAL_ID = "IMS:1000481";
        public const string IMAGE_ID = "IMS:1000004";
        public const string MAX_COUNT_PIXEL_X_ID = "IMS:1000042";
        public const string MAX_COUNT_PIXEL_Y_ID = "IMS:1000043";
        public const string MAX_DIMENSION_X_ID = "IMS:1000044";
        public const string MAX_DIMENSION_Y_ID = "IMS:1000045";
        public const string PIXEL_AREA_ID = "IMS:1000046";

        public string id;
        public SourceFileRefList sourceFileRefList;
        public TargetList targetList;

        public ScanSettings(string id)
        {
            this.id = id;
        }

        public ScanSettings(ScanSettings scanSettings, ReferenceableParamGroupList rpgList, SourceFileList sourceFileList)
            : base(scanSettings, rpgList)
        {
            this.id = scanSettings.id;

            if (scanSettings.sourceFileRefList != null && sourceFileList != null)
            {
                sourceFileRefList = new SourceFileRefList(scanSettings.sourceFileRefList, sourceFileList);
            }

            if (scanSettings.targetList != null)
            {
                targetList = new TargetList(scanSettings.targetList, rpgList);
            }
        }

        public string GetID()
        {
            return id;
        }

        public void SetSourceFileRefList(SourceFileRefList sourceFileRefList)
        {
            sourceFileRefList.SetParent(this);
            this.sourceFileRefList = sourceFileRefList;
        }

        public void SetTargetList(TargetList targetList)
        {
            targetList.SetParent(this);
            this.targetList = targetList;
        }

        public CVParam GetLineScanDirection()
        {
            return GetCVParamOrChild(LINE_SCAN_DIRECTION_ID);
        }

        public CVParam GetScanDirection()
        {
            return GetCVParamOrChild(SCAN_DIRECTION_ID);
        }

        public CVParam GetScanPattern()
        {
            return GetCVParamOrChild(SCAN_PATTERN_ID);
        }

        public CVParam GetScanType()
        {
            return GetCVParamOrChild(SCAN_TYPE_ID);
        }

        public override string ToString()
        {
            return $"scanSettings: {id}";
        }

        public override string GetTagName()
        {
            return "scanSettings";
        }

        public virtual void AddChildrenToCollection(List<IMzMLTag> children)
        {
            if (sourceFileRefList != null)
                children.Add(sourceFileRefList);
            if (targetList != null)
                children.Add(targetList);

            base.AddChildrenToCollection(children);
        }

        public void SetID(string id)
        {
            this.id = id;
        }
    }
}
