/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using System;
using System.Collections.Generic;

namespace AirdPro.Domains
{
    public class Stat
    {
        public double min;
        public double max;
        public double sum = 0;
        public double mean = 0;
        public double variance = 0;
        public double std;
        public List<double> dataList;
        public int size;

        public Stat(List<double> dataList)
        {
            if (dataList.Count == 0)
            {
                throw new Exception("Data list cannot be Empty");
            }

            this.dataList = dataList;
            size = dataList.Count;
            min = dataList[0];
            max = dataList[0];
            for (int i = 0; i < size; i++)
            {
                sum += dataList[i];
                if (dataList[i] > max)
                {
                    max = dataList[i];
                }

                if (dataList[i] < min)
                {
                    min = dataList[i];
                }
            }

            //平均值
            mean = sum / size;
            double varianceSum = 0;
            for (int i = 0; i < size; i++)
            {
                varianceSum += Math.Pow(dataList[i] - mean, 2);
            }

            //方差
            variance = varianceSum / size;
            //标准差
            std = Math.Sqrt(varianceSum / (size - 1));
        }
    }
}