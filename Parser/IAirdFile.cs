using AirdPro.Parser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using AirdPro.Utils;
using AirdPro.DomainsCore.Aird;
using System.Linq;

namespace Parser
{
    public class IAirdFile
    {
        public async Task<object> ReadFile(object input)
        {
            var para = JsonConvert.DeserializeObject<Input>(input.ToString());
            Output output = new Output();
            try
            {
                foreach (var item in para.filesPath)
                {
                    string path = FileNameUtil.getIndexPath(item);
                    var tic = getTIC(path);
                    output.status = true;
                    output.content = tic;
                    return JsonConvert.SerializeObject(output);
                }
            }
            catch (Exception e)
            {
                output.status = false;
                output.content = "Something error.";
                return JsonConvert.SerializeObject(output);
            }
            return null;
        }

        public async Task<object> Test(object input)
        {
            var para = JsonConvert.DeserializeObject<Input>(input.ToString());
            foreach (var item in para.filesPath)
            {
                Output output = new Output();
                try
                {
                    string path = FileNameUtil.getIndexPath(item);
                    if (string.IsNullOrEmpty(path))
                    {
                        output.status = false;
                        output.content = "Can't find the whole aird file.";
                        return JsonConvert.SerializeObject(output);
                    }
                    var msn = getMsNByTime(path, Convert.ToInt32(para.ms), Convert.ToDouble(para.time));
                    output.status = true;
                    output.content = msn;
                    return JsonConvert.SerializeObject(output);
                }
                catch (Exception e)
                {
                    output.status = false;
                    output.content = "Something error.";
                    return JsonConvert.SerializeObject(output);
                }
            }
            return null;
        }
        public async Task<object> getMsN(object input)
        {
            var para = JsonConvert.DeserializeObject<Input>(input.ToString());
            foreach (var item in para.filesPath)
            {
                Output output = new Output();
                try
                {
                    string path = FileNameUtil.getIndexPath(item);
                    if (string.IsNullOrEmpty(path))
                    {
                        output.status = false;
                        output.content = "Can't find the whole aird file.";
                        return JsonConvert.SerializeObject(output);
                    }
                    //var parser = new BaseParser(path);
                    var msn = getMsNByTime(path, Convert.ToInt32(para.ms), Convert.ToDouble(para.time));
                    output.status = true;
                    output.content = msn;
                    return JsonConvert.SerializeObject(output);
                }
                catch (Exception e)
                {
                    output.status = false;
                    output.content = "Something error.";
                    return JsonConvert.SerializeObject(output);
                }
            }
            return null;
        }

        public async Task<object> getFileDetail(object input)
        {
            Output output = new Output();
            try 
            {
                string path = FileNameUtil.getIndexPath(input.ToString());
                if (string.IsNullOrEmpty(path))
                {
                    output.status = false;
                    output.content = "Can't find the whole aird file.";
                    return JsonConvert.SerializeObject(output);
                }
                var str = File.ReadAllText(path);
                output.status = true;
                var info = JsonConvert.DeserializeObject<AirdInfo>(str);
                info.indexList = null;
                output.content = info;
                
                return JsonConvert.SerializeObject(output);
            }
            catch (Exception e)
            {
                output.status = false;
                output.content = "Something error.";
                return JsonConvert.SerializeObject(output);
            }
        }

        private BaseParser getParser(string type, string path)
        {
            switch (type.ToLower()) {
                case "dda":
                    return new DDAParser(path);
                case "dia":
                    return new DIAParser(path);
                default:
                    return new BaseParser(path);
            }
        }

        public List<MsNResult> getMsNByTime(string path, int msN, double time)
        {
            var list = new List<MsNResult>();
            var parser = new BaseParser(path);
            var msNBlock = parser.airdInfo.indexList.Where(x => x.level == msN)
                .OrderBy(x => Math.Abs(x.rts.OrderBy(y => Math.Abs(y - time)).FirstOrDefault() - time))
                .FirstOrDefault();
            var blockValue = parser.parseBlockValue(parser.airdFile, msNBlock);
            var MzIntensitys = blockValue.OrderBy(x => Math.Abs(x.Key - time)).FirstOrDefault().Value;
            var mzArray = MzIntensitys.getMzArray();
            var intensityArray = MzIntensitys.getIntensityArray();
            for (int i = 0; i < mzArray.Length; i++)
            {
                var r = new MsNResult();
                r.mz = mzArray[i];
                r.intensity = intensityArray[i];
                list.Add(r);
            }
            parser.close();
            return list;
        }

        /// <summary>
        /// 获取TIC
        /// </summary>
        /// <returns></returns>
        public List<TICResult> getTIC(string path)
        {
            var list = new List<TICResult>();
            var parser = new BaseParser(path);
            var ms1Block = parser.airdInfo.indexList.FirstOrDefault(x => x.level == 1);
            var blockValue = parser.parseBlockValue(parser.airdFile, ms1Block);
            foreach (var item in blockValue)
            {
                var r = new TICResult();
                r.rt = item.Key;
                r.intensity = item.Value.getIntensityArray().Sum();
                list.Add(r);
            }
            parser.close();
            return list;
        }

        class Input
        {
            public List<string> filesPath { get; set; }

            public string time { get; set; }

            public string ms { get; set; }
        }

        class Output
        {
            public bool status { get; set; }
            public object content { get; set; }
        }

        public class MsNResult
        {
            public double mz { get; set; }
            public float intensity { get; set; }
        }

        public class TICResult
        {
            public double rt { get; set; }
            public float intensity { get; set; }
        }
    }
}
