namespace CodeChurnLoader.Data.Bitbucket
{
    public class File
    {        
        public string FileName { get; set; }        
        public int Additions { get; set; }
        public int Deletions { get; set; }
        public int Changes { get { return Additions + Deletions; }}
    }
}
