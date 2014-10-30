﻿using System.Collections.Generic;

namespace CodeChurnLoader.Data
{
    public class DimFile
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string Url { get; set; }
        public int CommitId { get; set; }
        public virtual DimCommit Commit { get; set; }
        public List<FactCodeChurn> Churn { get; set; }

        public DimFile(string fileName)
        {
            this.FileExtension = System.IO.Path.GetExtension(fileName);
            this.Churn = new List<FactCodeChurn>();
        }
    }
}