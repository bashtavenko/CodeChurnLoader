using System.Collections.Generic;
using Newtonsoft.Json;

namespace CodeChurnLoader.Data.Github
{
    public class CommitProperties
    {
        [JsonProperty("committer")]
        public Committer Committer { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }        
    }
}
