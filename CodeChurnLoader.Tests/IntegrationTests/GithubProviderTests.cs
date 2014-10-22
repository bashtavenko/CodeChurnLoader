using System;
using System.Collections.Generic;
using System.Configuration;

using NUnit.Framework;

using CodeChurnLoader.Data;

namespace CodeChurnLoader.Tests.IntegrationTests
{
    [TestFixture]
    public class GithubProviderTests
    {
        private IGitProvider _IGitProvider;        

        [TestFixtureSetUp]
        public void SetUp()
        {            
            RepoCredentials repoCredentials = ConfigurationManager.GetSection("RepoCredentials") as RepoCredentials;
            _IGitProvider = new GithubProvider(repoCredentials);
        }

        [Test]
        public void GitHubProvider_GetCommits_ShouldGetList()
        {            
            List<Commit> commits = _IGitProvider.GetCommits("CodeMetricsLoader", DateTime.Now.AddMonths(-1), DateTime.Now);
            Assert.IsNotNull(commits);
            CollectionAssert.IsNotEmpty(commits);
        }
    }
}
