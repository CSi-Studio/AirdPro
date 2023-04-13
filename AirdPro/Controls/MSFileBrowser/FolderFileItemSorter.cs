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
using System.Collections;
using System.Windows.Forms;

namespace AirdPro
{
    public class FolderFileItemSorter : IComparer
    {
        private string mode;
        private SortOrder order;

        public FolderFileItemSorter(string mode, SortOrder order)
        {
            this.mode = mode;
            this.order = order;
        }

        public int Compare(object x, object y)
        {
            BaseItem a = x as BaseItem;
            BaseItem b = y as BaseItem;
            int res = 0;

            if (mode == "Date")
                res = DateTime.Compare(a.Date, b.Date);
            else if (mode == "Size")
            {
                if (a.Size < b.Size)
                    res = -1;
                else if (a.Size > b.Size)
                    res = 1;
            }
            else
                res = string.Compare(a.Name, b.Name);

            if (order == SortOrder.Ascending)
                return -res;
            else
                return res;
        }

        private string GetData(object x)
        {
            return (x as BaseItem).Name;
        }
    }
}