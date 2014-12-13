namespace CodeChurnLoader.Data
{
    public class File
    {
        public string Sha { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
        public int Additions { get; set; }
        public int Deletions { get; set; }
        public int Changes { get; set; }
    }
}
