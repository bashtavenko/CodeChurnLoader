using System;
using System.Collections.Generic;

namespace CodeChurnLoader.Data
{
    public interface IGitProvider
    {
        List<Commit> GetCommits(string repo, DateTime from, DateTime to);        
    }
}
