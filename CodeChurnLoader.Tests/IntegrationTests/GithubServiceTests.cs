using System;

using NUnit.Framework;

using CodeChurnLoader.Data.Github;


namespace CodeChurnLoader.Tests.IntegrationTests
{
    [TestFixture]
    public class GithbuServiceTests
    {   
        private GithubService _service;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _service = new GithubService(new ProviderConfigurationElement { Owner = "stanbpublic" });
        }

        [Test]
        public void GithubService_Commits()
        {            
            string json = _service.GetCommits("codemetricsloader", DateTime.Now.AddYears(-1), DateTime.Now);
            Assert.IsNotNullOrEmpty(json);
        }

        [Test]
        public void GithubService_GetOneCommit()
        {
            string json = _service.GetOneCommit("codemetricsloader", "d362b4578f6170ae61f7a638e65e22297216853e");
            Assert.IsNotNullOrEmpty(json);            
        }     
    }
}
