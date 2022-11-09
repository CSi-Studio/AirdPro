namespace AirdPro.Repository.MetaboLights
{
    public class MtblsStudy
    {
        public string studyStatus { get; set; }
        public bool read_access { get; set; }
        public bool write_access { get; set; }
        public bool is_curator { get; set; }
    }
}