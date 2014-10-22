using System.Xml.Serialization;

namespace CodeChurnLoader.Data.Github
{
    public class Stats
    {
        [XmlElement("total")]
        public int Total { get; set; }
        
        [XmlElement("additions")]
        public int Additions { get; set; }

        [XmlElement("deletions")]
        public int Deletions { get; set; }
    }
}
