using Propro.Constants;
using Propro.Domains;
using Propro.Structs;
using pwiz.CLI.cv;
using pwiz.CLI.msdata;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Propro_Client.Domains.Aird;

namespace Propro.Logics
{
    internal class DDAConverter : IConverter
    {
        private int totalSize;//总计谱图数目
        private int progress;//进度计数器
        List<TempIndex> ms1List = new List<TempIndex>();//用于存放MS1索引及基础信息
        

        public DDAConverter(ConvertJobInfo jobInfo) : base(jobInfo) { }

        public override void doConvert()
        {
            start();
            initDirectory();//创建文件夹
            using (airdStream = new FileStream(jobInfo.airdFilePath, FileMode.Create))
            {
                using (airdJsonStream = new FileStream(jobInfo.airdJsonFilePath, FileMode.Create))
                {
                    readVendorFile();//准备读取Vendor文件
                    initGlobalVar();//初始化全局变量
                    preProgress();//预处理谱图,将MS1和MS2谱图分开存储
                    writeToAirdInfoFile();//将Info数据写入文件
                }
            }
            finish();
        }

        private void initGlobalVar()
        {
            totalSize = spectrumList.size();
            progress = 0;
            jobInfo.log("Total Size:" + totalSize);
            startPosition  =  0;
        }

        //解析谱图
        private void preProgress()
        {
            int parentNum = 0;
            jobInfo.log("Preprocessing:" + totalSize, "Preprocessing");
            for (var i = 0; i < totalSize; i++)
            {
                
            }
        }

        //获取MsLevel
        private string getMsLevel(int index)
        {
            return spectrumList.spectrum(index).cvParamChild(CVID.MS_ms_level).value.ToString();
        }

        //添加到MS2谱图中
        private void addToMS2Map(TempIndex ms2Index)
        {
            
        }

        //添加到MS1谱图中
        private void addToMS1Map(TempIndex ms1Index)
        {
            
        }

        private void buildWindowsRanges()
        {
            jobInfo.log("Start getting windows", "Getting Windows");
            int i = 0;
            Spectrum spectrum = spectrumList.spectrum(0);
            while (!spectrum.cvParamChild(CVID.MS_ms_level).value.ToString().Equals(MsLevel.MS1))
            {
                i++;
                spectrum = spectrumList.spectrum(i);
                if (i > spectrumList.size() / 2 || i > 500)
                {
                    //如果找了一半的spectrum或者找了超过500个spectrum仍然没有找到ms1,那么数据格式有问题,返回空;
                    jobInfo.logError("Search for more than 500 spectrums and no ms1 was found. File Format Error");
                    throw new Exception("Search for more than 500 spectrums and no ms1 was found. File Format Error");
                }
            }

            i++;
            spectrum = spectrumList.spectrum(i);
            
        }



        private void computeOverlap()
        {

        }


        private void adjustOverlap()
        {

        }
            
        private void coreLoop()
        {

        }

        private void buildSwathBlock()
        {

        }

    }
}
