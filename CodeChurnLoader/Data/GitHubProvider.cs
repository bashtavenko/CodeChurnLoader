using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

using AutoMapper;
using Newtonsoft.Json;

namespace CodeChurnLoader.Data
{
    public class GithubProvider : IGitProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _owner;

        public GithubProvider(RepoCredentials repoCredentials)
        {
            _owner = repoCredentials.Owner;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(_owner);
            if (!string.IsNullOrEmpty(repoCredentials.UserName))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", repoCredentials.UserName, repoCredentials.Password))));
            }

            _httpClient.BaseAddress = new Uri("https://api.github.com/");
            
            AutoMapperConfig.CreateMaps();
        }

        public List<Commit> GetCommits(string repo, DateTime from, DateTime to)
        {
            HttpResponseMessage response = _httpClient.GetAsync(string.Format("repos/{0}/{1}/commits", _owner, repo)).Result;
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ApplicationException(string.Format("Cannot get data from remote repo - {0}", response.StatusCode));
            }
            string json = response.Content.ReadAsStringAsync().Result;
            var commitSummaries = JsonConvert.DeserializeObject<List<CodeChurnLoader.Data.Github.CommitSummary>>(json);

            List<Commit> commits = new List<Commit>();
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

        private Commit GetOneCommit(string repo, string sha)
        {
            HttpResponseMessage response = _httpClient.GetAsync(string.Format("repos/{0}/{1}/commits/{2}", _owner, repo, sha)).Result;
            string json = response.Content.ReadAsStringAsync().Result;
            var repoCommit = JsonConvert.DeserializeObject<CodeChurnLoader.Data.Github.Commit>(json);
            Commit commit = Mapper.Map<Commit>(repoCommit);
            return commit;
        }
    }
}
