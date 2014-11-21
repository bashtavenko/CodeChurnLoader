using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeChurnLoader.Data
{
    public class Commit
    {
        public string Sha { get; set; }
        public string Url { get; set; }
        public int NumberOfAdditions { get; set; }
        public int NumberOfDeletions { get; set; }
        public int NumberOfChanges { get; set; }
        public int TotalChurn { get { return NumberOfAdditions + NumberOfChanges + NumberOfDeletions; } }
        public string Message { get; set; }
        public string Committer { get; set; }
        public string CommitterAvatarUrl { get; set; }
        public DateTime Date { get; set; }

        public List<File> Files { get; set; }
    }
}
