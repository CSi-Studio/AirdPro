using System;
using System.Collections.Generic;
using AirdSDK.Beans.Common;
using System.Linq;
using System.Collections.Concurrent;
using System.Numerics;
using AirdSDK.Enums;
using AirdSDK.Utils;
using MathNet.Numerics.LinearAlgebra.Complex;
using MathNet.Numerics.LinearAlgebra.Storage;

public class CentroidUtil
{
    public static IntSpectrum centroid(int[] mzs, double[] intensities, double noiseLevel)
    {
        if (mzs == null)
        {
            return null;
        }

        // use number of centroid signals as base array list capacity
        int pointNum = mzs.Length;

        // lists of primitive doubles
        List<int> pickedMzs = new List<int>();
        List<double> pickedInts = new List<double>();

        //计算局部最大值，作为峰的候选点
        int localMaximumIndex = 0;
        List<int> rangeDataPoints = new List<int>();
        bool ascending = true; //递增

        // Find possible mzPeaks
        for (int i = 0; i < pointNum - 1; i++)
        {
            bool nextIsBigger = intensities[i + 1] > intensities[i];
            bool nextIsZero = !(intensities[i + 1] > 0d);
            bool currentIsZero = !(intensities[i] > 0d);

            //忽略强度为0的点
            if (currentIsZero)
            {
                continue;
            }

            //将强度非0的点加到当前峰
            rangeDataPoints.Add(i);

            //检查局部最大值
            if (ascending && (!nextIsBigger))
            {
                localMaximumIndex = i;
                ascending = false;
                continue;
            }

            //寻找峰的下边界
            if ((!ascending) && (nextIsBigger || nextIsZero))
            {
                //计算exact mass
                int exactMz = calculateExactMass(mzs, intensities, localMaximumIndex, rangeDataPoints);
                //添加强度高于噪声阈值的点
                if (intensities[localMaximumIndex] > noiseLevel)
                {
                    pickedMzs.Add(exactMz);
                    pickedInts.Add(intensities[localMaximumIndex]);
                }

                //重置参数，寻找下一个峰
                ascending = true;
                rangeDataPoints.Clear();
            }
        }

        return new IntSpectrum(pickedMzs.ToArray(), pickedInts.ToArray());
    }

    private static int calculateExactMass(int[] mzs, double[] intensities, int topIndex, List<int> rangeDataPoints)
    {
        double xRight = -1, xLeft = -1;
        double halfInt = intensities[topIndex] / 2;
        for (int i = 0; i < rangeDataPoints.Count - 1; i++)
        {
            //左侧
            if ((intensities[rangeDataPoints[i]] <= halfInt) && (mzs[rangeDataPoints[i]] < mzs[topIndex]) &&
                (intensities[rangeDataPoints[i + 1]] >= halfInt))
            {
                double leftY1 = intensities[rangeDataPoints[i]];
                double leftX1 = mzs[rangeDataPoints[i]];
                double leftY2 = intensities[rangeDataPoints[i + 1]];
                double leftX2 = mzs[rangeDataPoints[i + 1]];
                //计算斜率
                double mLeft = (leftY1 - leftY2) / (leftX1 - leftX2);
                if (mLeft == 0.0)
                {
                    xLeft = (leftX1 + leftX2) / 2;
                }
                else
                {
                    xLeft = leftX1 + (((halfInt) - leftY1) / mLeft);
                }

                continue;
            }

            //右侧
            if ((intensities[rangeDataPoints[i]] >= halfInt) && (mzs[rangeDataPoints[i]] > mzs[topIndex]) &&
                (intensities[rangeDataPoints[i + 1]] <= halfInt))
            {
                double rightY1 = intensities[rangeDataPoints[i]];
                double rightX1 = mzs[rangeDataPoints[i]];
                double rightY2 = intensities[rangeDataPoints[i + 1]];
                double rightX2 = mzs[rangeDataPoints[i + 1]];
                //计算斜率
                double mRight = (rightY1 - rightY2) / (rightX1 - rightX2);
                if (mRight == 0.0)
                {
                    xRight = (rightX1 + rightX2) / 2;
                }
                else
                {
                    xRight = rightX1 + (((halfInt) - rightY1) / mRight);
                }

                break;
            }
        }

        if ((xRight == -1) || (xLeft == -1)) return mzs[topIndex];

        // The center of left and right points is the exact mass of our peak.
        return (int)((xLeft + xRight) / 2);
    }
}