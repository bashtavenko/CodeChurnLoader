using Newtonsoft.Json;

namespace CodeChurnLoader.Data.Bitbucket
{
    public class Urls
    {
        [JsonProperty("html")]
        public Href Html { get; set; }

        [JsonProperty("diff")]
        public Href Diff { get; set; }

        [JsonProperty("avatar")]
        public Href Avatar { get; set; }
    }
}
