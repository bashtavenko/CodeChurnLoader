namespace CodeChurnLoader.Data.Bitbucket
{
    public interface IBitbucketService
    {
        string GetCommits(string repo, string branch);
        string GetString(string url);
        string GetDiff(string url);
    }
}
