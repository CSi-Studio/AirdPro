/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

namespace AirdPro.Converters
{
    public class ConverterWorkFlow
    {
        public static void DDA(Converter converter)
        {
            converter.predictForIntensityPrecision(); //预测intensity需要保留的精度
            converter.predictForBestCombination(); //预测最佳压缩组合
            converter.pretreatmentDDA(); //MS1和MS2分开建立索引
            converter.compressMS1Block(); //处理MS1,并将索引写入文件流中
            converter.compressMS2BlockForDDA(); //处理MS2,并将索引写入文件流中
            converter.writeToAirdInfoFile(); //将Info数据写入文件
        }

        public static void DIA(Converter converter)
        {
            converter.predictForIntensityPrecision(); //预测intensity需要保留的精度
            converter.predictForBestCombination(); //预测最佳压缩组合
            converter.pretreatmentDIA(); //预处理谱图,将MS1和MS2谱图分开存储
            converter.compressMS1Block();
            converter.compressMS2BlockForDIA();
            converter.writeToAirdInfoFile(); //将Info数据写入文件
        }

        public static void DDAPasef(Converter converter)
        {
            converter.initBrukerMobi();
            converter.predictForIntensityPrecision(); //预测intensity需要保留的精度
            converter.predictForBestCombination(); //预测最佳压缩组合
            converter.pretreatmentDDAPasef(); //MS1和MS2分开建立索引
            converter.compressMobiDict();
            converter.compressMS1Block(); //处理MS1,并将索引写入文件流中
            converter.compressMS2BlockForDDA(); //处理MS2,并将索引写入文件流中
            converter.writeToAirdInfoFile(); //将Info数据写入文件
        }

        public static void DIAPasef(Converter converter)
        {
            converter.initBrukerMobi();
            converter.predictForIntensityPrecision(); //预测intensity需要保留的精度
            converter.predictForBestCombination(); //预测最佳压缩组合
            converter.pretreatmentDIAPasef(); //预处理谱图,将MS1和MS2谱图分开存储
            converter.compressMobiDict();
            converter.compressMS1Block();
            converter.compressMS2BlockForDIA();
            converter.writeToAirdInfoFile(); //将Info数据写入文件
        }

        public static void PRM(Converter converter)
        {
            converter.predictForIntensityPrecision(); //预测intensity需要保留的精度
            converter.predictForBestCombination(); //预测最佳压缩组合
            converter.pretreatmentPRM(); //预处理谱图,将MS1和MS2谱图分开存储
            converter.compressMS1Block(); //处理MS1,并将索引写入文件流中
            converter.compressMS2BlockForPRM(); //处理MS2,并将索引写入文件流中
            converter.writeToAirdInfoFile(); //将Info数据写入文件
        }

        public static void MRM(Converter converter)
        {
            if (converter.spectrumList.size() > 0)
            {
                converter.predictForIntensityPrecision(); //预测intensity需要保留的精度
                converter.predictForBestCombination(); //预测最佳压缩组合
                converter.pretreatmentDDA(); //MS1和MS2分开建立索引
                converter.compressMS1Block(); //处理MS1,并将索引写入文件流中
                converter.compressMS2BlockForDDA(); //处理MS2,并将索引写入文件流中
            }
            
            converter.compressChromatograms();
            converter.writeToAirdInfoFile(); //将Info数据写入文件
        }
    }
}