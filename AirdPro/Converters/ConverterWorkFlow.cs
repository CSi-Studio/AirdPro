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
    public abstract class ConverterWorkFlow
    {
        public static void DDA(PwizConverter converter)
        {
            converter.PredictForIntensityPrecision(); //预测intensity需要保留的精度
            converter.PredictForBestCombination(); //预测最佳压缩组合
            converter.PretreatmentDda(); //MS1和MS2分开建立索引
            converter.CompressMs1Block(); //处理MS1,并将索引写入文件流中
            converter.compressMS2BlockForDDA(); //处理MS2,并将索引写入文件流中
            converter.WriteToAirdInfoFile(); //将Info数据写入文件
        }

        public static void DDA(ImzMLConverter converter)
        {
            converter.PredictForIntensityPrecision(); //预测intensity需要保留的精度
            converter.PredictForBestCombination(); //预测最佳压缩组合
            converter.PretreatmentDda(); //MS1和MS2分开建立索引
            converter.CompressMs1Block(); //处理MS1,并将索引写入文件流中
            converter.WriteToAirdInfoFile(); //将Info数据写入文件
        }

        public static void DIA(PwizConverter converter)
        {
            converter.PredictForIntensityPrecision(); //预测intensity需要保留的精度
            converter.PredictForBestCombination(); //预测最佳压缩组合
            converter.PretreatmentDia(); //预处理谱图,将MS1和MS2谱图分开存储
            converter.CompressMs1Block();
            converter.CompressMs2BlockForDia();
            converter.WriteToAirdInfoFile(); //将Info数据写入文件
        }        

        public static void DDAPasef(PwizConverter converter)
        {
            converter.InitBrukerMobi();
            converter.PredictForIntensityPrecision(); //预测intensity需要保留的精度
            converter.PredictForBestCombination(); //预测最佳压缩组合
            converter.PretreatmentDdaPasef(); //MS1和MS2分开建立索引
            converter.CompressMobiDict();
            converter.CompressMs1Block(); //处理MS1,并将索引写入文件流中
            converter.compressMS2BlockForDDA(); //处理MS2,并将索引写入文件流中
            converter.WriteToAirdInfoFile(); //将Info数据写入文件
        }        

        public static void DIAPasef(PwizConverter converter)
        {
            converter.InitBrukerMobi();
            converter.PredictForIntensityPrecision(); //预测intensity需要保留的精度
            converter.PredictForBestCombination(); //预测最佳压缩组合
            converter.PretreatmentDiaPasef(); //预处理谱图,将MS1和MS2谱图分开存储
            converter.CompressMobiDict();
            converter.CompressMs1Block();
            converter.CompressMs2BlockForDia();
            converter.WriteToAirdInfoFile(); //将Info数据写入文件
        }       

        public static void PRM(PwizConverter converter)
        {
            converter.PredictForIntensityPrecision(); //预测intensity需要保留的精度
            converter.PredictForBestCombination(); //预测最佳压缩组合
            converter.PretreatmentPrm(); //预处理谱图,将MS1和MS2谱图分开存储
            converter.CompressMs1Block(); //处理MS1,并将索引写入文件流中
            converter.CompressMs2BlockForPrm(); //处理MS2,并将索引写入文件流中
            converter.WriteToAirdInfoFile(); //将Info数据写入文件
        }        

        public static void MRM(PwizConverter converter)
        {
            if (converter.SpectrumList.size() > 0)
            {
                converter.PredictForIntensityPrecision(); //预测intensity需要保留的精度
                converter.PredictForBestCombination(); //预测最佳压缩组合
                converter.PretreatmentDda(); //MS1和MS2分开建立索引
                converter.CompressMs1Block(); //处理MS1,并将索引写入文件流中
                converter.compressMS2BlockForDDA(); //处理MS2,并将索引写入文件流中
            }
            
            converter.compressChromatograms();
            converter.WriteToAirdInfoFile(); //将Info数据写入文件
        }        

        public static void DDAMSI(MSIConvert converter)
        {
            converter.PredictForIntensityPrecision(); //预测intensity需要保留的精度
            converter.PredictForBestCombination(); //预测最佳压缩组合
            converter.PretreatmentDda(); //MS1和MS2分开建立索引
            converter.CompressMs1Block(); //处理MS1,并将索引写入文件流中
            converter.compressMS2BlockForDDA(); //处理MS2,并将索引写入文件流中
            converter.StoreAirdInfo();
        }       

    }
}