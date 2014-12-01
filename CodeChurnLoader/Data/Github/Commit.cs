using System.Collections.Generic;
using Newtonsoft.Json;

namespace CodeChurnLoader.Data.Github
{
    public class Commit
    {
        [JsonProperty("sha")]
        public string Sha { get; set; }

        [JsonProperty("html_url")]
        public string Url { get; set; }

        [JsonProperty("stats")]
        public Stats Stats { get; set; }

        [JsonProperty("commit")]
        public CommitProperties CommitProperties { get; set; }

        [JsonProperty("committer")]
        public Committer Committer { get; set; }

        [JsonProperty("files")]
        public List<File> Files { get; set; }        
    }
}
