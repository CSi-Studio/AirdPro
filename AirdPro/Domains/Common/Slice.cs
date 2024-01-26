using System.Collections.Generic;

namespace AirdPro.Domains.Common;

public class Slice
{
    public List<int> indexIdList = [];
    public List<int> intensityList = [];

    public void Add(int indexId, int intensity)
    {
        indexIdList.Add(indexId);
        intensityList.Add(intensity);
    }
}