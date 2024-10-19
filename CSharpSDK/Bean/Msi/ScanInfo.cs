namespace AirdSDK.Bean.Msi
{
    public class ScanInfo
    {        
        public string linescanSequence; //bottom up, top down, left right, right left, no direction 
        public string scanPattern; //meandering, flyback, random access
        public string scanType; //horizontal linescan, vertical linescan
        public string linescanDirection; //linescan bottom up, linescan left right,  linescan right left, linescan right left
    }
}
