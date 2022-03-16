using System.Net.Http.Headers;

namespace AirdPro.DomainsCore.Aird;

public class MobiInfo
{ 
    public long dictStart; // start position in the aird for mobi array
    public long dictEnd; //end position in the aird for mobi array
    public string unit; //ion mobility unit
    public string type; //ion mobility type
}