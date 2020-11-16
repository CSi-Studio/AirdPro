/*
 * Copyright (c) 2020 CSi Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using System.Collections.Generic;

namespace AirdPro.Domains.Aird
{
    public class BlockIndex
    {
        //1:MS1;2:MS2
        public int level;
        //在文件中的开始位置
        public long startPtr;
        //在文件中的结束位置
        public long endPtr;
        //每一个MS2对应的MS1序号;如果是MS1块,则本字段为空
        public int parentNum;
        //每一个MS2对应的前体窗口;MS1中为空,在SWATH/DIA和PRM的采集模式下,本数组的长度恒为1
        public List<WindowRange> rangeList;
        //当msLevel=1时,本字段为每一个MS1谱图的序号,当msLevel=2时本字段为每一个MS2谱图序列号
        public List<int> nums = new List<int>();
        //所有该块中的rt时间列表
        public List<float> rts = new List<float>();
        //一个块中所有子谱图的mz的压缩后的大小列表,当为Common类型时,每一个存储的不是块大小,而是起始位置
        public List<long> mzs = new List<long>();
        //一个块中所有子谱图的intenisty的压缩后的大小列表,当为Common类型时,每一个存储的不是块大小,而是起始位置
        public List<long> ints = new List<long>();
        //用于存储KV键值对
        public string features;

        // 用于兼容SWATH模式下,此
        public WindowRange getWindowRange()
        {
            if (rangeList == null || rangeList.Count == 0)
            {
                return null;
            }
            else
            {
                return rangeList[0];
            }
        }

        public void setWindowRange(WindowRange wr)
        {
            if (rangeList == null)
            {
                rangeList = new List<WindowRange>();
            }
            rangeList.Add(wr);
        }

        public BlockIndex()
        {
        }

        public BlockIndex(int level)
        {
            this.level = level;
        }
    }
}
