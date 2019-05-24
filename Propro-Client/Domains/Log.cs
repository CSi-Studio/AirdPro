using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propro.Domains
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
