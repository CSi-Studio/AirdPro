using System.Collections.Generic;
using AirdPro.Converters;
using AirdPro.DomainsCore.Aird;
using pwiz.CLI.msdata;

namespace AirdPro.Algorithms
{
    public abstract class IZDPD
    {
        protected IConverter converter;
        public IZDPD(IConverter converter)
        {
            this.converter = converter;
        }

        public abstract void compressMS1(BlockIndex index);

        public abstract void compressMS2(List<TempIndex> tempIndexList, BlockIndex index);
    }
}