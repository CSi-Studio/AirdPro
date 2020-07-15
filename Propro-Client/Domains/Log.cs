using System;

namespace AirdPro.Domains
{
    public class Log
    {
        public DateTime dateTime;
        public string content;

        public Log(DateTime dt, string content)
        {
            this.dateTime = dt;
            this.content = content;
        }
    }
}
