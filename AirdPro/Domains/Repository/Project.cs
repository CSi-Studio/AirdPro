namespace AirdPro.Repository.ProteomeXchange
{
    public class Project
    {
        //仓库识别号
        public string Identifier { set; get; }
        //仓库标题
        public string Title { set; get; }
        public string Repos { set; get; }
        public string Species { set; get; }
        public string Instrument { set; get; }
        public string Publication { set; get; }
        public string LabHead { set; get; }
        public string Announce { set; get; }
        public string Keywords { set; get; }
    }
}