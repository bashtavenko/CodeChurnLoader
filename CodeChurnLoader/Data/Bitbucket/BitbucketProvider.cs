using System;
using System.Linq;
using System.Collections.Generic;

using AutoMapper;
using Newtonsoft.Json;

namespace CodeChurnLoader.Data.Bitbucket
{
    public class BitbucketProvider : IGitProvider
    {   
        private readonly IBitbucketService _service;
        private readonly DiffParser _diffParser;

        public BitbucketProvider(IBitbucketService service)
        {
            _service = service;
            _diffParser = new DiffParser();
            AutoMapperConfig.CreateMaps();
        }

        public List<Data.Commit> GetCommits(string repo, DateTime from, DateTime to)
        {            
            var commitsJson = _service.GetCommits(repo, "master");
            var pageCommits = JsonConvert.DeserializeObject<CommitHeader>(commitsJson);
            List<Commit> bitbucketCommits = pageCommits.Commits;

            while (!string.IsNullOrEmpty(pageCommits.NextPageUrl))
            {
                commitsJson = _service.GetString(pageCommits.NextPageUrl);
                pageCommits = JsonConvert.DeserializeObject<CommitHeader>(commitsJson);
                bitbucketCommits.AddRange(pageCommits.Commits);
            }

            // Since Bitbucket doesn't provide filter by date
            bitbucketCommits = bitbucketCommits.Where(w => w.Date >= from && w.Date <= to).Select(s => s).ToList();
            
            foreach (var commit in bitbucketCommits)
            {
                var diffText = _service.GetDiff(commit.Urls.Diff.Value);
                commit.Files = _diffParser.Parse(diffText);
            }

            List<Data.Commit> commits = Mapper.Map<List<Data.Commit>>(bitbucketCommits);
            return commits;
        }        
    }
}
