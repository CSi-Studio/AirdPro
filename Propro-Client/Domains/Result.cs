using System;

namespace AirdPro.Domains
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
