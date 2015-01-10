using System;
using System.Collections.Generic;

using AutoMapper;
using Newtonsoft.Json;

namespace CodeChurnLoader.Data.Github
{
    public class GithubProvider : IGitProvider
    {
        private readonly GithubService _service;
        
        public GithubProvider(GithubService service)
        {
            _service = service;            
            AutoMapperConfig.CreateMaps();
        }

        public List<Data.Commit> GetCommits(string repo, DateTime from, DateTime to)
        {   
            var commits = new List<Data.Commit>();
            string json = _service.GetCommits(repo, from, to);
            var commitSummaries = JsonConvert.DeserializeObject<List<CodeChurnLoader.Data.Github.Commit>>(json);

            foreach (var commitSummary in commitSummaries)
            {
                var commit = GetOneCommit(repo, commitSummary.Sha);
                if (commit != null)
                {
                    commits.Add(commit);
                }
            }
            
            return commits;
        }        

        private Data.Commit GetOneCommit(string repo, string sha)
        {   
            string json = _service.GetOneCommit(repo, sha);
            var repoCommit = JsonConvert.DeserializeObject<CodeChurnLoader.Data.Github.Commit>(json);
            Data.Commit commit = Mapper.Map<Data.Commit>(repoCommit);
            return commit;
        }
    }
}
