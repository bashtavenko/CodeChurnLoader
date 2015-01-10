using System;

namespace CodeChurnLoader.Data.Github
{
    public interface IGithubService
    {
        string GetCommits(string repo, DateTime from, DateTime to);
        string GetOneCommit(string repo, string sha);
    }
}
