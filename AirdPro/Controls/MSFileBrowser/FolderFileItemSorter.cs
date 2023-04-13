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