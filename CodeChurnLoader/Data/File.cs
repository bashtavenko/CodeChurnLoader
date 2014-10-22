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
