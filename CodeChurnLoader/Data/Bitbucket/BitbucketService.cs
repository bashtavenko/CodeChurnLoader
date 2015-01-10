using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace CodeChurnLoader.Data.Bitbucket
{
    public class BitbucketService : IBitbucketService
    {
        private readonly HttpClient _httpClient;
        private readonly ProviderConfigurationElement _providerConfig;

        public BitbucketService(ProviderConfigurationElement providerConfig)
        {
            _providerConfig = providerConfig;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrEmpty(providerConfig.UserName))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", providerConfig.UserName, providerConfig.Password))));
            }

            _httpClient.BaseAddress = new Uri("https://bitbucket.org/api/2.0/");
        }

        public string GetCommits(string repo, string branch)
        {
            string url = string.Format("repositories/{0}/{1}/commits", _providerConfig.Owner, repo);
            if (!string.IsNullOrEmpty(branch))
            {
                url = url + "?branchortag=" + branch;
            }
            return GetString(url);
        }       

        public string GetDiff(string url)
        {
            return GetString(url);
        }

        public string GetString(string url)
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
