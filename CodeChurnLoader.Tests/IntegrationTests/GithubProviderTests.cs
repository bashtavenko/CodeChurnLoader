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
            _IGitProvider = new Data.Github.GithubProvider(repoCredentials);
        }

        [Test]
        public void GitHubProvider_GetCommits_ShouldGetList()
        {            
            List<Commit> commits = _IGitProvider.GetCommits("CodeMetricsLoader", DateTime.Now.AddYears(-2), DateTime.Now);
            Assert.IsNotNull(commits);
            CollectionAssert.IsNotEmpty(commits);
        }

        [Test]
        public void GitHubProvider_GetCommits_ShouldNotGetList()
        {
            List<Commit> commits = _IGitProvider.GetCommits("CodeMetricsLoader", DateTime.Now.AddYears(-2), DateTime.Now.AddYears(-1));
            Assert.IsNotNull(commits);
            CollectionAssert.IsEmpty(commits);
        }
    }
}
