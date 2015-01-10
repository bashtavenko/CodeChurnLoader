using System.Collections.Generic;
using Newtonsoft.Json;

namespace CodeChurnLoader.Data.Bitbucket
{
    public class CommitHeader
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("next")]
        public string NextPageUrl { get; set; }

        [JsonProperty("values")]
        public List<Commit> Commits { get; set; }
    }
}
