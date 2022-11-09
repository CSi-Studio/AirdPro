using System.Collections.Generic;

namespace AirdPro.Repository.ProteomeXchange
{
    public class RootProject
    {
        public List<TermNode> contacts { set; get; }
        public List<Node> datasetFiles { set; get; }
        public List<Node> fullDatasetLinks { set; get; }
        public List<TermNode> species { set; get; }
        public string title { set; get; }
        public string description { set; get; }
        public List<Node> keywords { set; get; }
        public List<Node> identifiers { set; get; }
        public List<Node> modifications { set; get; }
        public List<TermNode> publications { set; get; }
    }
}