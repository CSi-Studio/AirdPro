/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

namespace AirdPro.Repository.ProteomeXchange
{
    public class Project
    {
        //仓库识别号
        public string Identifier { set; get; }

        //仓库标题
        public string Title { set; get; }
        public string Repos { set; get; }
        public string Species { set; get; }
        public string Instrument { set; get; }
        public string Publication { set; get; }
        public string LabHead { set; get; }
        public string Announce { set; get; }
        public string Keywords { set; get; }
    }
}