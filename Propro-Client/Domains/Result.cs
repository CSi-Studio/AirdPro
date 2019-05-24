using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propro.Domains
{
    class Result
    {
        Boolean success;
        string code;
        string msg;

        public Result()
        {
        }

        public Result(String code, String msg)
        {
            this.code = code;
            this.msg = msg;
        }
    }
}
