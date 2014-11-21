using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace CodeChurnLoader.Data.Github
{
    public class Committer
    {
        [JsonProperty("login")]
        public string Login { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("date")]
        public DateTime? Date { get; set; }

        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }
    }
}
