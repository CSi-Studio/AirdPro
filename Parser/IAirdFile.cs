using AirdPro.Parser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using AirdPro.Utils;
using AirdPro.DomainsCore.Aird;

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
                    var parser = new BaseParser(path);
                    var tic = parser.getTIC();
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
                    var parser = new BaseParser(path);
                    var msn = parser.getMsN(Convert.ToInt32(para.ms), Convert.ToDouble(para.time));
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
                    var parser = new BaseParser(path);
                    var msn = parser.getMsN(Convert.ToInt32(para.ms), Convert.ToDouble(para.time));
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
    }
}
