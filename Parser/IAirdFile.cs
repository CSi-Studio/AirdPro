using AirdPro.Parser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Parser
{
    public class IAirdFile
    {
        public async Task<object> ReadFile(object input)
        {
            var para = JsonConvert.DeserializeObject<Input>(input.ToString());
            foreach (var item in para.filesPath)
            {
                var parser = new BaseParser(item);
                var tic = parser.getTIC();
                return JsonConvert.SerializeObject(tic);
            }
            return null;
        }

        public object qq(object input)
        {
            var parser = new BaseParser(input.ToString());
            return JsonConvert.SerializeObject(parser.airdInfo);
        }


        class Input
        {
            public List<string> filesPath { get; set; }

            public string time { get; set; }

            public string ms { get; set; }
        }

    }
}
