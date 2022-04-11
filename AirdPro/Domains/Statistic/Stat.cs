/*
 * Copyright (c) 2020 CSi Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
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

        public Stat(List<double> numbers)
        {
            int length = numbers.Count;
            min = numbers[0];
            max = numbers[0];
            for (int i = 0; i < length; i++)
            {
                sum += numbers[i];
                if (numbers[i] > max)
                {
                    max = numbers[i];
                }

                if (numbers[i] < min)
                {
                    min = numbers[i];
                }
            }

            mean = sum / length;
            double varianceSum = 0;
            for (int i = 0; i < length; i++)
            {
                varianceSum += Math.Pow(numbers[i] - mean, 2);
            }

            std = Math.Sqrt(varianceSum / (length - 1));
            variance = sum / length;
        }
    }
}