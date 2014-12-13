using System;
using System.Collections.Generic;

namespace CodeChurnLoader.Data
{
    public class Commit
    {
        public string Sha { get; set; }
        public string Url { get; set; }
        public int Additions { get; set; }
        public int Deletions { get; set; }
        public int Changes { get; set; }
        public string Message { get; set; }
        public string Committer { get; set; }
        public string CommitterAvatarUrl { get; set; }
        public DateTime Date { get; set; }

        public List<File> Files { get; set; }
    }
}
