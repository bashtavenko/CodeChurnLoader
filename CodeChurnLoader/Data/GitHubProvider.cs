using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CodeChurnLoader.Data
{
    public class GithubProvider : IGitProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _owner;

        public GithubProvider(string owner)
        {
            _owner = owner;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(owner);
            _httpClient.BaseAddress = new Uri("https://api.github.com/");
        }

        public List<string> GetCommits(string repo, DateTime from, DateTime to)
        {
            HttpResponseMessage response = _httpClient.GetAsync(string.Format("repos/{0}/{1}/commits", _owner, repo)).Result;
            string json = response.Content.ReadAsStringAsync().Result;
            var commits = JsonConvert.DeserializeObject<List<CodeChurnLoader.Data.Github.Commit>>(json);
            return commits.Select(s => s.Sha).ToList();
        }

        public Commit GetOneCommit(string repo, string sha)
        {
            throw new NotImplementedException();
        }
    }
}
