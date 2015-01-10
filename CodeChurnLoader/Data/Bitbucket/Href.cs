using Newtonsoft.Json;

namespace CodeChurnLoader.Data.Bitbucket
{
    public class Href
    {
        [JsonProperty("href")]
        public string Value { get; set; }
    }
}
