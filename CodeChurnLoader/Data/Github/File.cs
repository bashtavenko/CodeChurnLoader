using System.Xml.Serialization;

namespace CodeChurnLoader.Data.Github
{
    public class File
    {
        [XmlElement("sha")]
        public string Sha { get; set; }

        [XmlElement("filename")]
        public string Filename { get; set; }

        [XmlElement("status")]
        public string Status { get; set; }

        [XmlElement("additions")]
        public int Additions { get; set; }
        
        [XmlElement("additions")]
        public int Deletions { get; set; }

        [XmlElement("changes")]
        public int Changes { get; set; }

        [XmlElement("raw_url")]
        public int Url { get; set; }
    }
}
