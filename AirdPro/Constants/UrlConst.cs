namespace AirdPro.Constants
{
    public class UrlConst
    {
        public static string pxListUrl = "http://proteomecentral.proteomexchange.org/cgi/GetDataset?action=search&filterstr=";

        public static string pxDetailUrl = "http://proteomecentral.proteomexchange.org/cgi/GetDataset?ID=";

        public static string pxDetailJsonUrl = "http://proteomecentral.proteomexchange.org/cgi/GetDataset?outputMode=JSON&test=no&ID=";

        public static string mlListUrl = "https://www.ebi.ac.uk/metabolights/ws/studies";

        public static string mlDetailUrl = "https://www.ebi.ac.uk/metabolights/";

        public static string mlFtpUrl = "ftp://ftp.ebi.ac.uk/pub/databases/metabolights/studies/public/";
    }
}