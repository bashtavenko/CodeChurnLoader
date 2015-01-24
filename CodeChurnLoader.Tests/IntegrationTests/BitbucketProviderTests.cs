using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using NUnit.Framework;

using CodeChurnLoader.Data;
using CodeChurnLoader.Data.Bitbucket;

namespace CodeChurnLoader.Tests.IntegrationTests
{
    [TestFixture]
    public class BtbbucketProviderTests
    {
        private IGitProvider _IGitProvider;
        private IBitbucketService _service;

        [TestFixtureSetUp]
        public void SetUp()
        {            
            var loaderConfigurationSection = (ConfigurationManager.GetSection("LoaderConfiguration") as LoaderConfigurationSection);            
            var providerConfig = loaderConfigurationSection.Providers.Where(w => w.Type == ProviderType.Bitbucket).FirstOrDefault();
            if (providerConfig == null)
            {
                Assert.Inconclusive("No Bitibucket provider");
            } 
            _service = new BitbucketService(providerConfig);
            _IGitProvider = new Data.Bitbucket.BitbucketProvider(_service);
        }

        //[TestCase("somerepo", "2014-12-03", "85a615bf5876b81d0ca025b2e7ac20f6c67cedbf")]
        public void BitbucketProvider_GetRealCommit(string repo, string dateFromString, string sha)
        {
            // Arrange
            var dateFrom = DateTime.Parse(dateFromString);
            var dateTo = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 23, 59, 59);

            // Act
            List<CodeChurnLoader.Data.Commit> commits = _IGitProvider.GetCommits(repo, dateFrom, dateTo);

            // Assert
            Assert.IsNotNull(commits);          
        }            
    }
}
