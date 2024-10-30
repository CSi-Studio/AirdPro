using AirdSDK.Bean;
using AirdSDK.Bean.Msi;
using AirdSDK.Enums.Msi;
using System;
using System.Collections.Generic;

namespace AirdPro.Utils
{
    public class MsiUtil
    {
        public static MsiInfo GetMsiInfo(MsiConfig msiConfig, int spectraCount, double minMZ, double maxMZ)
        {
            MsiInfo msiInfo = new();
            //fileOrganisation: row per file, image per file，spectrum per file
            msiInfo.fileOrganisation = GetFileOrganisation(msiConfig);
            //image info
            msiInfo.imageInfo = GetImageInfo(msiConfig, minMZ, maxMZ);
            //spectrum position
            msiInfo.spectraPosition = GetSpectraPosition(msiConfig, spectraCount);
            //public ScanInfo scanInfo
            msiInfo.scanInfo = GetScanInfo(msiConfig);

            return msiInfo;
        }

        public static string GetFileOrganisation(MsiConfig msiConfig)
        {
            return msiConfig.fileOrganisation switch
            {
                FileOrganisation.SPECTRUM_PER_FILE => FileOrganisation.SPECTRUM_PER_FILE,
                FileOrganisation.IMAGE_PER_FILE => FileOrganisation.IMAGE_PER_FILE,
                FileOrganisation.ROW_PER_FILE => FileOrganisation.ROW_PER_FILE,
                _ => throw new ArgumentException("Invalid file organisation")
            };
        }

        public static ImageInfo GetImageInfo(MsiConfig msiConfig, double minMZ, double maxMZ)
        {
            ImageInfo imageInfo = new()
            {
                maxPixelX = msiConfig.maxPixelX,
                maxPixelY = msiConfig.maxPixelY,
                maxPixelZ = msiConfig.maxPixelZ,
                pixelSizeX = msiConfig.pixelSizeX,
                pixelSizeY = msiConfig.pixelSizeY,

                minMZ = minMZ,
                maxMZ = maxMZ
            };
            return imageInfo;
        }

        public static ScanInfo GetScanInfo(MsiConfig msiConfig)
        {
            ScanInfo scanInfo = new();
            //scanDirection
            switch (msiConfig.scanDirection)
            {
                case ScanDirection.LINESCAN_TOP_DOWN:
                    scanInfo.scanDirection = ScanDirection.LINESCAN_TOP_DOWN;
                    break;
                case ScanDirection.LINESCAN_BOTTOM_UP:
                    scanInfo.scanDirection = ScanDirection.LINESCAN_BOTTOM_UP;
                    break;
                case ScanDirection.LINESCAN_LEFT_RIGHT:
                    scanInfo.scanDirection = ScanDirection.LINESCAN_LEFT_RIGHT;
                    break;
                case ScanDirection.LINESCAN_RIGHT_LEFT:
                    scanInfo.scanDirection = ScanDirection.LINESCAN_RIGHT_LEFT;
                    break;
            }
            //scan pattern
            switch (msiConfig.scanPattern)
            {
                case ScanPattern.MEANDERING:
                    scanInfo.scanPattern = ScanPattern.MEANDERING;
                    break;
                case ScanPattern.FLY_BACK:
                    scanInfo.scanPattern = ScanPattern.FLY_BACK;
                    break;
                case ScanPattern.RANDOM_ACCESS:
                    scanInfo.scanPattern = ScanPattern.RANDOM_ACCESS;
                    break;
            }
            //scan sequence
            switch (msiConfig.scanSequence)
            {
                case ScanSequence.TOP_DOWN:
                    scanInfo.scanSequence = ScanSequence.TOP_DOWN;
                    break;
                case ScanSequence.BOTTOM_UP:
                    scanInfo.scanSequence = ScanSequence.BOTTOM_UP;
                    break;
                case ScanSequence.LEFT_RIGHT:
                    scanInfo.scanSequence = ScanSequence.LEFT_RIGHT;
                    break;
                case ScanSequence.RIGHT_LEFT:
                    scanInfo.scanSequence = ScanSequence.RIGHT_LEFT;
                    break;
            }
            //scanType
            if(msiConfig.scanDirection == ScanDirection.LINESCAN_LEFT_RIGHT || msiConfig.scanDirection == ScanDirection.LINESCAN_RIGHT_LEFT)
            {
                scanInfo.scanType = ScanType.HORIZONTAL;
            }
            else
            {
                scanInfo.scanType = ScanType.VERTICAL;
            }            

            return scanInfo;
        }

        public static SpectraPosition GetSpectraPosition(MsiConfig msiConfig, int spectaCount)
        {
            if (spectaCount == 0)
            {
                return null;
            }
            int[] x = new int[spectaCount];
            int[] y = new int[spectaCount];
            int[] z = new int[spectaCount];
            
            List<int> yList = new();
            if (msiConfig.maxPixelZ == 1)
            {
                for(int i= 0; i < spectaCount; i++)
                {
                    z[i] = 1;
                }
                //根据msiConfig计算坐标信息，一共16种情况
                if (msiConfig.scanDirection.Equals(ScanDirection.LINESCAN_LEFT_RIGHT) && msiConfig.scanSequence.Equals(ScanSequence.TOP_DOWN))
                {
                    if (msiConfig.scanPattern.Equals(ScanPattern.MEANDERING)) //1: 1-x,x-1; 1-1,y-y
                    {
                        x = X_1toXandXto1(msiConfig.maxPixelX, msiConfig.maxPixelY);
                    }
                    else if (msiConfig.scanPattern.Equals(ScanPattern.FLY_BACK)) //2: 1-x,1-x; 1-1,y-y
                    {
                        x = X_1toXand1toX(msiConfig.maxPixelX, msiConfig.maxPixelY);
                    }
                    y = Y_1to1andYtoY(msiConfig.maxPixelX, msiConfig.maxPixelY);
                }
                else if (msiConfig.scanDirection.Equals(ScanDirection.LINESCAN_LEFT_RIGHT) && msiConfig.scanSequence.Equals(ScanSequence.BOTTOM_UP))
                {
                    if (msiConfig.scanPattern.Equals(ScanPattern.MEANDERING)) //3: 1-x,x-1; y-y,1-1 
                    {
                        x = X_1toXandXto1(msiConfig.maxPixelX, msiConfig.maxPixelY);
                    }
                    else if (msiConfig.scanPattern.Equals(ScanPattern.FLY_BACK)) //4: 1-x,1-x; y-y,1-1
                    {
                        x = X_1toXand1toX(msiConfig.maxPixelX, msiConfig.maxPixelY);
                    }
                    y = Y_YtoYand1to1(msiConfig.maxPixelX, msiConfig.maxPixelY);
                }
                else if (msiConfig.scanDirection.Equals(ScanDirection.LINESCAN_RIGHT_LEFT) && msiConfig.scanSequence.Equals(ScanSequence.TOP_DOWN))
                {
                    if (msiConfig.scanPattern.Equals(ScanPattern.MEANDERING)) //5：x-1,1-x; 1-1,y-y
                    {
                        x = X_Xto1and1toX(msiConfig.maxPixelX, msiConfig.maxPixelY);
                    }
                    else if (msiConfig.scanPattern.Equals(ScanPattern.FLY_BACK)) //6：x-1,x-1; 1-1,y-y
                    {
                        x = X_Xto1andXto1(msiConfig.maxPixelX, msiConfig.maxPixelY);
                    }
                    y = Y_1to1andYtoY(msiConfig.maxPixelX, msiConfig.maxPixelY);
                }
                else if (msiConfig.scanDirection.Equals(ScanDirection.LINESCAN_RIGHT_LEFT) && msiConfig.scanSequence.Equals(ScanSequence.BOTTOM_UP))
                {
                    if (msiConfig.scanPattern.Equals(ScanPattern.MEANDERING)) //7：x-1,1-x; y-y,1-1
                    {
                        x = X_Xto1and1toX(msiConfig.maxPixelX, msiConfig.maxPixelY);
                    }
                    else if (msiConfig.scanPattern.Equals(ScanPattern.FLY_BACK)) //8：x-1,x-1; y-y,1-1
                    {
                        x = X_Xto1andXto1(msiConfig.maxPixelX, msiConfig.maxPixelY);
                    }
                    y = Y_YtoYand1to1(msiConfig.maxPixelX, msiConfig.maxPixelY);
                }
                else if (msiConfig.scanDirection.Equals(ScanDirection.LINESCAN_TOP_DOWN) && msiConfig.scanSequence.Equals(ScanSequence.LEFT_RIGHT))
                {                    
                    if (msiConfig.scanPattern.Equals(ScanPattern.MEANDERING)) //9：1-1,x-x; 1-y,y-1
                    {
                        y = Y_1toYandYto1(msiConfig.maxPixelX, msiConfig.maxPixelY);
                    }
                    else if (msiConfig.scanPattern.Equals(ScanPattern.FLY_BACK)) //10：1-1,x-x; 1-y,1-y
                    {
                        y = Y_1toYand1toY(msiConfig.maxPixelX, msiConfig.maxPixelY);
                    }
                    x = X_1to1andXtoX(msiConfig.maxPixelX, msiConfig.maxPixelY);
                }
                else if (msiConfig.scanDirection.Equals(ScanDirection.LINESCAN_TOP_DOWN) && msiConfig.scanSequence.Equals(ScanSequence.RIGHT_LEFT)) 
                { 
                    if (msiConfig.scanPattern.Equals(ScanPattern.MEANDERING)) //11：x-x,1-1; 1-y,y-1
                    {
                        y = Y_1toYandYto1(msiConfig.maxPixelX, msiConfig.maxPixelY);
                    }
                    else if (msiConfig.scanPattern.Equals(ScanPattern.FLY_BACK)) //12：x-x,1-1; 1-y,1-y
                    {
                        y = Y_1toYand1toY(msiConfig.maxPixelX, msiConfig.maxPixelY);
                    }
                    x = X_XtoXand1to1(msiConfig.maxPixelX, msiConfig.maxPixelY);
                }
                else if (msiConfig.scanDirection.Equals(ScanDirection.LINESCAN_BOTTOM_UP) && msiConfig.scanSequence.Equals(ScanSequence.LEFT_RIGHT))
                {
                    if (msiConfig.scanPattern.Equals(ScanPattern.MEANDERING)) //13：1-1,x-x; y-1,1-y
                    {
                        y = Y_Yto1and1toY(msiConfig.maxPixelX, msiConfig.maxPixelY);
                    }
                    else if (msiConfig.scanPattern.Equals(ScanPattern.FLY_BACK)) //14：1-1,x-x; y-1,y-1
                    {
                        y = Y_Yto1andYto1(msiConfig.maxPixelX, msiConfig.maxPixelY);
                    }
                    x = X_1to1andXtoX(msiConfig.maxPixelX, msiConfig.maxPixelY);
                }
                else if (msiConfig.scanDirection.Equals(ScanDirection.LINESCAN_BOTTOM_UP) && msiConfig.scanSequence.Equals(ScanSequence.RIGHT_LEFT))
                {
                    if (msiConfig.scanPattern.Equals(ScanPattern.MEANDERING)) //15：x-x,1-1; y-1,1-y
                    {
                        y = Y_Yto1and1toY(msiConfig.maxPixelX, msiConfig.maxPixelY);
                    }
                    else if (msiConfig.scanPattern.Equals(ScanPattern.FLY_BACK)) //16：x-x,1-1; y-1,y-1
                    {
                        y = Y_Yto1andYto1(msiConfig.maxPixelX, msiConfig.maxPixelY);
                    }
                    x = X_XtoXand1to1(msiConfig.maxPixelX, msiConfig.maxPixelY);
                }
            }
            else
            {
                // 不用实现
            }
            
            return new SpectraPosition(x, y, z);
        }

        private static int[] Y_Yto1andYto1(int maxPixelX, int maxPixelY)
        {
            List<int> yList = new();
            for (int j = 1; j <= maxPixelX; j++)
            {
                for (int i = maxPixelY; i >= 1; i--)
                {
                    yList.Add(i);
                }
            }
            return yList.ToArray();
        }

        private static int[] Y_Yto1and1toY(int maxPixelX, int maxPixelY)
        {
            List<int> yList = new();
            int cycle = maxPixelX / 2;
            for (int i = 1; i <= cycle; i++)
            {
                for (int j = maxPixelY; j >= 1; j--)
                {
                    yList.Add(j);
                }
                for (int j = 1; j <= maxPixelY; j++)
                {
                    yList.Add(j);
                }
                
            }
            if (maxPixelX % 2 != 0)
            {
                for (int j = maxPixelY; j >= 1; j--)
                {
                    yList.Add(j);
                }
            }
            return yList.ToArray();
        }

        private static int[] X_XtoXand1to1(int maxPixelX, int maxPixelY)
        {
            List<int> xList = new();
            for (int i = maxPixelX; i >= 1; i--)
            {
                for (int j = 1; j <= maxPixelY; j++)
                {
                    xList.Add(i);
                }
            }
            return xList.ToArray();
        }

        private static int[] X_1to1andXtoX(int maxPixelX, int maxPixelY)
        {
            List<int> xList = new();
            for (int i = 1; i <= maxPixelX; i++)
            {
                for (int j = 1; j <= maxPixelY; j++)
                {
                    xList.Add(i);
                }
            }
            return xList.ToArray();
        }

        private static int[] Y_1toYand1toY(int maxPixelX, int maxPixelY)
        {
            List<int> yList = new();
            for (int j = 1; j <= maxPixelX; j++)
            {
                for (int i = 1; i <= maxPixelY; i++)
                {
                    yList.Add(i);
                }
            }
            return yList.ToArray();
        }

        private static int[] Y_1toYandYto1(int maxPixelX, int maxPixelY)
        {
            List<int> yList = new();
            int cycle = maxPixelX / 2;
            for (int i = 1; i <= cycle; i++)
            {
                for (int j = 1; j <= maxPixelY; j++)
                {
                    yList.Add(j);
                }
                for (int j = maxPixelY; j >= 1; j--)
                {
                    yList.Add(j);
                }
            }
            if (maxPixelX % 2 != 0)
            {
                for (int j = 1; j <= maxPixelY; j++)
                {
                    yList.Add(j);
                }
            }
            return yList.ToArray();
        }

        private static int[] Y_YtoYand1to1(int maxPixelX, int maxPixelY)
        {
            List<int> yList = new();
            for (int i = maxPixelY; i >= 1; i--)
            {
                for (int j = 1; j <= maxPixelX; j++)
                {
                    yList.Add(i);
                }
            }
            return yList.ToArray();
        }

        private static int[] X_1toXandXto1(int maxPixelX, int maxPixelY)
        {
            List<int> xList = new();
            int cycle = maxPixelY / 2;
            for (int i = 1; i <= cycle; i++)
            {
                for (int j = 1; j <= maxPixelX; j++)
                {
                    xList.Add(j);
                }
                for (int j = maxPixelX; j >= 1; j--)
                {
                    xList.Add(j);
                }
            }
            if (maxPixelY % 2 != 0)
            {
                for (int j = 1; j <= maxPixelX; j++)
                {
                    xList.Add(j);
                }
            }
            return xList.ToArray();
        }

        private static int[] Y_1to1andYtoY(int maxPixelX, int maxPixelY)
        {
            List<int> yList = new();
            for (int i = 1; i <= maxPixelY; i++)
            {
                for (int j = 1; j <= maxPixelX; j++)
                {
                    yList.Add(i);
                }
            }
            return yList.ToArray();
        }

        private static int[] X_Xto1and1toX(int maxPixelX, int maxPixelY) 
        {
            List<int> xList = new();
            int cycle = maxPixelY / 2;
            for (int i = 1; i <= cycle; i++)
            {
                for (int j = maxPixelX; j >= 1; j--)
                {
                    xList.Add(j);
                }
                for (int j = 1; j <= maxPixelX; j++)
                {
                    xList.Add(j);
                }                
            }
            if (maxPixelY % 2 != 0)
            {
                for (int j = maxPixelX; j >= 1; j--)
                {
                    xList.Add(j);
                }
            }
            return xList.ToArray();
        }       

        private static int[] X_1toXand1toX(int maxPixelX, int maxPixelY)  //1-maxPixelX,1-maxPixelX,...,1-maxPixelX
        {
            List<int> xList = new();
            for (int i = 1; i <= maxPixelY; i++)
            {
                for (int j = 1; j <= maxPixelX; j++)
                {
                    xList.Add(j);
                }
            }
            return xList.ToArray();
        }

        private static int[] X_Xto1andXto1(int maxPixelX, int maxPixelY)  //maxPixelX-1,maxPixelX-1,...,maxPixelX-1
        {
            List<int> xList = new();
            for (int i = 1; i <= maxPixelY; i++)
            {
                for (int j = maxPixelX; j >= 1; j--)
                {
                    xList.Add(j);
                }
            }
            return xList.ToArray();
        }        
       
    }        
}
