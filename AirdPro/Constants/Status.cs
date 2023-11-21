/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

namespace AirdPro.Constants
{
    public class Status
    {
        public static string Prepare = "Prepare";
        public static string Init = "Init";
        public static string Waiting = "Waiting";
        public static string Starting = "Starting";
        public static string Adapting = "Adapting";
        public static string Finished = "Finished";
        public static string Predicting = "Predicting";
        public static string Error = "Error";
        public static string Preprocessing = "Preprocessing";
        public static string Pretreatment = "Pretreatment";
        public static string Writing_Index_File = "Writing Index File";

        public static string Redis_Connected = "Connected";
        public static string Redis_Not_Connected = "Unconnect";

        public static string empty = "";
        public static string tag_ms1 = "MS1:";
        public static string tag_ms2 = "MS2:";
        public static string tag_preprocessing = "Preprocessing:";


        public static string progress(string tag, int progress, int total)
        {
            return tag + progress + "/" + total;
        }
    }
}