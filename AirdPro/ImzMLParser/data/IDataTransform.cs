using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.ImzMLParser.data
{
    public interface IDataTransform
    {
        byte[] ForwardTransform(byte[] data);
        byte[] ReverseTransform(byte[] data);
    }
}
