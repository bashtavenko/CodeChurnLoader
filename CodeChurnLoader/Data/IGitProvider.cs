using System;
using System.Collections.Generic;

namespace CodeChurnLoader.Data
{
    public interface IGitProvider
    {
        List<string> GetCommits(string repo, DateTime from, DateTime to);
        Commit GetOneCommit(string repo, string sha);
    }
}
