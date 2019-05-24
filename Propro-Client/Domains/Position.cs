using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propro.Domains
{
    class Position
    {
        //start
        public long s;
        //delta
        public long d;
        
        public Position(long start, long delta)
        {
            this.s = start;
            this.d = delta;
        }
    }
}
