using System;
using System.Collections;
using System.Windows.Forms;

namespace AirdPro.Domains;

public class FileListSorter: IComparer
{
    public static bool isAscending = true;
    public int Compare(object x, object y)
    {
        ListViewItem itemX = (ListViewItem)x;
        ListViewItem itemY = (ListViewItem)y;
        
        // 将比较结果反转以实现倒序排列
        try
        {
            int result = int.Parse(itemY.SubItems[0].Text) - int.Parse(itemX.SubItems[0].Text);
            return isAscending?result:-result;
        }
        catch (Exception ee)
        {
            Console.WriteLine(ee.Message);
        }
        return String.Compare(itemY.Text, itemX.Text);
    }
}