using System;
using System.Collections.Generic;
using System.Text;

namespace AirdSDK.Enums
{
    public enum MSIFileOrganisation
    {
        ROW_PER_FILE = 0,
        IMAGE_PER_FILE = 1,
        SPECTRUM_PER_FILE = 2
    }

    public enum LineScanDirection
    {
        Left_Right = 0,
        Right_Left = 1,
        Top_Down = 2,
        Bottom_Up = 3
    }

    public enum ScanSequence
    {
        Left_Right = 0,
        Right_Left = 1,
        Top_Down = 2,
        Bottom_Up = 3
    }

    public enum ScanPattern
    {
        FlyBack = 0,
        Meandering = 1,
    }
}
