using System.Xml.Serialization;

namespace CodeChurnLoader.Data.Github
{
    public class CommitSummary
    {
        [XmlElement("sha")]
        public string Sha { get; set; }
    }
}
