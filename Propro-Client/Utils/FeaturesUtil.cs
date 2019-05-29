using System.Collections;
using System.Text;

namespace Propro_Client.Utils
{
    class FeaturesUtil
    {

        public static char SP = ';';
        public static char SSP = ':';

        /**
         * 通过Map转换成String
         *
         * @param attrs
         * @return
         */
        public static string toString(Hashtable attrs)
        {
            StringBuilder sb = new StringBuilder();
            if (null != attrs && attrs.Count != 0)
            {
                sb.Append(SP);
                foreach (DictionaryEntry entry in attrs)
                {
                    string key = entry.Key.ToString();
                    string val = entry.Value.ToString();
                    sb.Append(key).Append(SSP).Append(val).Append(SP);
                }
            }
            return sb.ToString();
        }

        /**
         * 通过字符串解析成attributes
         *
         * @param str (格式比如为: "k:v;k:v;k:v")
         * @return
         */
        public static Hashtable toMap(string str) {
            Hashtable attrs = new Hashtable();
            if (str != null)
            {
                string[] arr = str.Split(SP);
                foreach (string kv in arr)
                {
                    if (null != kv && "".Equals(kv))
                    {
                        string[] ar = kv.Split(SSP);
                        if (ar.Length == 2)
                        {
                            attrs.Add(ar[0], ar[1]);
                        }
                    }  
                }
            }
            return attrs;
        }
    }
}
