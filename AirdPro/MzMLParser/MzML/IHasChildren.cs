using System.Collections.Generic;

namespace AirdPro.csimzMLParser.mzml
{
    public interface IHasChildren
    {
        void AddChildrenToCollection(ICollection<IMzMLTag> children);
    }
}
