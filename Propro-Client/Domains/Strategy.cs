using System;

namespace Propro.Domains
{
    class Strategy
    {
        public String methods;

        public int precision;

        public Strategy()
        {
        }

        public Strategy(String methods, int precision)
        {
            this.methods = methods;
            this.precision = precision;
        }
    }
}
