using System.Collections.Generic;

namespace AirdPro.ImzMLParser.mzml
{
    public interface IHasChildren
    {
        void AddChildrenToCollection(ICollection<IMzMLTag> children);
    }
}
