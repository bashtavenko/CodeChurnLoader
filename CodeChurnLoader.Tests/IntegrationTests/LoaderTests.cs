using CodeChurnLoader.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    NumberOfAdditions = 10,
                    NumberOfChanges = 2,
                    NumberOfDeletions = 3,
                    Files = new List<File>
                    {
                        new File
                        {
                            Sha = "000000fdeb4000000000000d8000000005efc51e",
                            Url = "https://api.github.com/repos/file1",
                            FileName = "file1.cs",
                            NumberOfAdditions = 6,
                            NumberOfChanges = 1,
                            NumberOfDeletions = 3
                        },
                        new File
                        {
                            Sha = "111111fdeb4111111111111d8111111115efc51e",
                            Url = "https://api.github.com/repos/file2",
                            FileName = "file2.cs",
                            NumberOfAdditions = 4,
                            NumberOfChanges = 1,
                            NumberOfDeletions = 0
                        },
                    }
                }
            };

            loader.SaveCommits(DateTime.Now, "CodeMetricsLoader", commits);
        }

        [Test]
        public void Loader_Load_CanLoad()
        {
            LoaderContext context = ContextTests.CreateTestContext();
            Loader loader = new Loader(context, new TestLogger());
            RepoCredentials repoCredentials = ConfigurationManager.GetSection("RepoCredentials") as RepoCredentials;
            var provider = new Data.Github.GithubProvider(repoCredentials);
            loader.Load(provider, "CodeMetricsLoader", new DateTime(2013, 1, 1), new DateTime(2015, 1, 1));
        }
    }
}
