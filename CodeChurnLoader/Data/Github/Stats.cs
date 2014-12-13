using Newtonsoft.Json;
using System.Xml.Serialization;

namespace CodeChurnLoader.Data.Github
{
    public class Stats
    {   
        [JsonProperty("additions")]
        public int Additions { get; set; }

        [JsonProperty("deletions")]
        public int Deletions { get; set; }

        [JsonProperty("total")]
        public int Changes { get; set; }
    }
}
