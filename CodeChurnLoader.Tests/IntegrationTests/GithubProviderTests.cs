using CodeChurnLoader.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeChurnLoader.Tests.IntegrationTests
{
    [TestFixture]
    public class GithubProviderTests
    {
        private IGitProvider _IGitProvider;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _IGitProvider = new GithubProvider("stanbpublic");
        }


        [Test]
        public void GitHubProvider_GetCommits_ShouldGetList()
        {
            List<Commit> commits = _IGitProvider.GetCommits("codemetricsloader", DateTime.Now.AddMonths(-1), DateTime.Now);
            Assert.IsNotNull(commits);
            CollectionAssert.IsNotEmpty(commits);
        }
    }
}
