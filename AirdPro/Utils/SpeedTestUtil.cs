using System.Collections.Generic;
using System;
using pwiz.CLI.analysis;
using pwiz.CLI.msdata;
using System.Diagnostics;
using AirdSDK;
using AirdSDK.Parser;

namespace AirdPro.Utils;

public class SpeedTestUtil
{
    public static void Test(String filePath, Dictionary<int, List<int>> dict)
    {
        ReaderList readerList = ReaderList.FullReaderList;
        var readerConfig = new ReaderConfig
        {
            allowMsMsWithoutPrecursor = false,
            combineIonMobilitySpectra = true,
            ignoreZeroIntensityPoints = true
        };

        MSDataList msdList = new MSDataList();
        readerList.read(filePath, msdList, readerConfig);

        MSData msd = msdList[0];
        List<string> filter = new List<string>();
        SpectrumListFactory.wrap(msd, filter); //这一步操作可以帮助加快Wiff文件的初始化速度
        SpectrumList spectrumList = msd.run.spectrumList;

        Stopwatch sw = new Stopwatch();

        foreach (var keyValuePair in dict)
        {
            List<int> nums = keyValuePair.Value;
            long length = 0;
            sw.Restart();
            nums.ForEach(i =>
            {
                Spectrum spectrum = spectrumList.spectrum(i, true);
                double[] mzData = spectrum.getMZArray().data.Storage();
                double[] intData = spectrum.getIntensityArray().data.Storage();
                length += mzData.Length;
            });
            sw.Stop();
            long delta = sw.ElapsedMilliseconds;
            Console.Write(length/delta+",");
        }
    }

    public static void TestAird(String filePath, Dictionary<int, List<int>> dict)
    {
        DDAParser parser = new DDAParser(filePath);
        Stopwatch sw = new Stopwatch();

        foreach (var keyValuePair in dict)
        {
            int[] nums = keyValuePair.Value.ToArray();
            sw.Restart();
            var spectra = parser.GetSpectraByNums(nums);
            sw.Stop();
            Console.Write(sw.ElapsedMilliseconds + ",");
        }
    }
    public static List<int> GenerateUniqueRandomNumbers(int minValue, int maxValue, int count)
    {
        // 确保范围内有足够的唯一整数
        if (maxValue - minValue + 1 < count)
        {
            throw new ArgumentException("Range must contain at least 'count' unique numbers.");
        }

        List<int> numbers = new List<int>();
        Random random = new Random();

        // 先填充列表，然后进行 Fisher-Yates 洗牌算法以确保随机性
        for (int i = minValue; i <= maxValue; i++)
        {
            numbers.Add(i);
        }

        // Fisher-Yates 洗牌算法
        for (int i = numbers.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            int temp = numbers[i];
            numbers[i] = numbers[j];
            numbers[j] = temp;
        }

        // 如果需要的计数小于范围内总数，则截取前count个
        return numbers.GetRange(0, count);
    }
}