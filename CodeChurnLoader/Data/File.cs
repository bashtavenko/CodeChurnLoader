using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeChurnLoader.Data
{
    public class File
    {
        public string Sha { get; set; }
        public string FileName { get; set; }
        public int NumberOfAdditions { get; set; }
        public int NumberOfDeletions { get; set; }
        public int NumberOfChanges { get; set; }
    }
}
