using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace CodeChurnLoader.Data.Github
{
    public class GithubService : IGithubService
    {
        private readonly HttpClient _httpClient;
        private readonly ProviderConfigurationElement _providerConfig;

        public GithubService(ProviderConfigurationElement providerConfig)
        {
            _providerConfig = providerConfig;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(_providerConfig.Owner);
            if (!string.IsNullOrEmpty(providerConfig.UserName))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", providerConfig.UserName, providerConfig.Password))));
            }

            _httpClient.BaseAddress = new Uri("https://api.github.com/");
        }

        public string GetCommits(string repo, DateTime from, DateTime to)
        {
            string url = string.Format("repos/{0}/{1}/commits?since={2:s}&until={3:s}", _providerConfig.Owner, repo, from, to);
            var json = GetJson(url);
            return json;
        }

        public string GetOneCommit(string repo, string sha)
        {
            string url = string.Format("repos/{0}/{1}/commits/{2}", _providerConfig.Owner, repo, sha);
            var json = GetJson(url);
            return json;
        }

        private string GetJson (string url)
        {
            HttpResponseMessage response = _httpClient.GetAsync(url).Result;
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ApplicationException(string.Format("Cannot get data from remote repo - {0}", response.StatusCode));
            }
            string json = response.Content.ReadAsStringAsync().Result;
            return json;
        }
    }
}
