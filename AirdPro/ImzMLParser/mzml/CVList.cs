using AirdPro.ImzMLParser.obo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.ImzMLParser.mzml
{
    public class CVList : MzMLIDContentList<CV>
    {
        /**
     * 创建一个具有指定初始容量的空CVList。
     *
     * @param count 初始容量
     */
        public CVList(int count) : base(count)
        {
        }

        /**
         * 列表的复制构造函数，浅拷贝。
         *
         * @param cvList 要复制的CVList
         */
        public CVList(CVList cvList) : base(cvList)
        {
        }

        /**
         * 添加CV。辅助方法，用于保持API，调用
         * {@link MzMLIDContentList<T>.Add(T item)}.
         * 
         * @param cv 要添加到列表的CV
         */
        public void AddCV(CV cv)
        {
            base.Add(cv);
        }

        /**
         * 返回具有特定唯一ID的CV。辅助方法，用于保持API，调用
         * {@link MzMLIDContentList<T>.Get(String id)}.
         * 
         * @param id CV的唯一ID属性
         * @return 如果列表中存在具有给定id的CV，则返回CV，否则返回null。
         */
        public CV GetCV(string id)
        {
            return base.Get(id);
        }

        public override string GetTagName()
        {
            return "cvList";
        }

        /**
         * 生成CVList的默认内容。这将包括描述MSI、MS和单位本体的CV元素。
         * 
         * @return 默认CVList
         */
        public static CVList Create()
        {
            CVList cvList = new CVList(3);

            OBO obo = OBO.GetOBO();
            List<OBO> fullOBOList = obo.GetFullImportHierarchy();

            foreach (OBO currentOBO in fullOBOList)
            {
                cvList.AddCV(new CV(currentOBO));
            }

            return cvList;
        }
    }
}
