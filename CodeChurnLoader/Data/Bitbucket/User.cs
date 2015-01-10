using Newtonsoft.Json;

namespace CodeChurnLoader.Data.Bitbucket
{
    public class User
    {
        [JsonProperty("display_name")]
        public string UserName { get; set; }

        [JsonProperty("links")]
        public Urls Urls { get; set; }
    }
}
