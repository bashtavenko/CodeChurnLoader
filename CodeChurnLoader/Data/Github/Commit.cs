using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CodeChurnLoader.Data.Github
{
    public class Commit
    {
        [XmlElement("sha")]
        public string Sha { get; set; }
    }
}
