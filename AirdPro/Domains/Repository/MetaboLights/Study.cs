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

namespace AirdPro.Repository.MetaboLights
{
    public class Study
    {
        public List<NameValue> comments { get; set; }
        public string identifier { get; set; }
        public string filename { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string submissionDate { get; set; }
        public string publicReleaseDate { get; set; }
        public List<People> people { get; set; }

        public List<Annotation> studyDesignDescriptors { get; set; }
        public List<Publication> publications { get; set; }

        public List<Factor> factors { get; set; }
        public List<Protocol> protocols { get; set; }
        public List<Assay> assays { get; set; }
    }
}