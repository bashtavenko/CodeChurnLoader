using NUnit.Framework;

using CodeChurnLoader.Data.Bitbucket;

namespace CodeChurnLoader.Tests.IntegrationTests
{
    [TestFixture]
    public class BitbucketServiceTests
    {   
        [Test]
        public void BitbucketService_GetPublicCommits()
        {
            var service = new BitbucketService(new ProviderConfigurationElement { Owner = "stanbpublic" });
            string json = service.GetCommits("testrepo", "master");
        }

        //[Test]
        public void BitbucketService_GetPrivateCommits()
        {
            var service = new BitbucketService(new ProviderConfigurationElement { Owner = "secret", UserName="secret", Password="secret" });
            string json = service.GetCommits("secret", "master");
        }
    }
}
