using System.Collections.Generic;
using System.Xml.Serialization;

namespace CodeChurnLoader.Data.Github
{
    public class Commit
    {
        [XmlElement("sha")]
        public string Sha { get; set; }

        [XmlElement("url")]
        public string Url { get; set; }

        [XmlElement("stats")]
        public Stats Stats { get; set; }

        [XmlElement("files")]
        public List<File> Files { get; set; }        
    }
}
