/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using System.Collections.Generic;

public class CentroidUtil
{
    private static int CalculateExactMass(int[] mzs, float[] intensities, int topIndex, List<int> rangeDataPoints)
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