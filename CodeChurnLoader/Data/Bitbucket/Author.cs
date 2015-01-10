using Newtonsoft.Json;

namespace CodeChurnLoader.Data.Bitbucket
{
    public class Author
    {
        [JsonProperty("raw")]
        public string Raw { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }
    }
}
