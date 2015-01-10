using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

namespace CodeChurnLoader.Data.Bitbucket
{
    public class Commit
    {
        [JsonProperty("hash")]
        public string Sha { get; set; }

        [JsonProperty("links")]
        public Urls Urls { get; set; }
        
        [JsonProperty("author")]
        public Author Author { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        public int Additions { get { return Files.Sum(s => s.Additions); } }

        public int Deletions { get { return Files.Sum(s => s.Deletions); } }

        public int Changes { get { return Additions + Deletions; } }

        public ICollection<File> Files { get; set; }

        public Commit()
        {
            Files = new List<File>();
        }
    }
}
