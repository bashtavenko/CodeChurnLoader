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
    }
}
