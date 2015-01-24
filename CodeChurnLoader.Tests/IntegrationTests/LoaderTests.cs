using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using NUnit.Framework;

using CodeChurnLoader.Data;

namespace CodeChurnLoader.Tests.IntegrationTests
{
    [TestFixture]
    public class LoaderTests
    {
        [Test]
        public void Loader_Load_CanSaveCommits()
        {
            LoaderContext context = ContextTests.CreateTestContext();
            Loader loader = new Loader(context, new TestLogger());

            List<Commit> commits = new List<Commit>
            {
                new Commit
                {
                    Sha = "9c4800fdeb47aa8f990105fd894ab1f125efc51e",
                    Url = "https://api.github.com/repos/StanBPublic/CodeMetricsLoader/commits/9c4800fdeb47aa8f990105fd894ab1f125efc51e",
                    Additions = 10,                    
                    Deletions = 3, 
                    Changes = 13,
                    Message = "Test",
                    Committer = "Joe Doe",
                    CommitterAvatarUrl = "https://avatars.githubusercontent.com/u/1820912?v=3",
                    Date = DateTime.Now,
                    Files = new List<File>
                    {
                        new File
                        {
                            Sha = "000000fdeb4000000000000d8000000005efc51e",
                            Url = "https://api.github.com/repos/file1",
                            FileName = "file1.cs",
                            Additions = 6,
                            Changes = 9,
                            Deletions = 3,                            
                        },
                        new File
                        {
                            Sha = "111111fdeb4111111111111d8111111115efc51e",
                            Url = "https://api.github.com/repos/file2",
                            FileName = "file2.cs",
                            Additions = 4,
                            Changes = 4,
                            Deletions = 0
                        },
                    }
                }
            };

            loader.SaveCommits(DateTime.Now.AddDays(-1), "CodeMetricsLoader", commits);
            loader.SaveCommits(DateTime.Now, "CodeMetricsLoader", commits);
        }

        [Test]
        public void Loader_Load_CanLoad()
        {
            LoaderContext context = ContextTests.CreateTestContext();
            Loader loader = new Loader(context, new TestLogger());            
            var providerConfig = (ConfigurationManager.GetSection("LoaderConfiguration") as LoaderConfigurationSection)
                .Providers
                .Where(w => w.Type == ProviderType.Github).First();
            var provider = new Data.Github.GithubProvider(new Data.Github.GithubService(providerConfig));
            loader.Load(provider, "CodeMetricsLoader", new DateTime(2013, 1, 1), new DateTime(2015, 1, 1));
        }
    }
}
