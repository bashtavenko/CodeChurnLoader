using System;
using System.Collections.Generic;
using System.Configuration;

using NUnit.Framework;

using CodeChurnLoader.Data;
using System.IO;
using Newtonsoft.Json;

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

        [Test]
        public void GitHubProvider_Json_CanDeserializeCommits()
        {
            var json = GetJsonFromFile("Commits.json");
            var commits = JsonConvert.DeserializeObject<List<CodeChurnLoader.Data.Github.Commit>>(json);
            Assert.AreEqual(2, commits.Count);
            var commit = commits[0];
            Assert.AreEqual("21bbb99934a36899c3c65347ea886569823c9a54", commit.Sha);
            Assert.AreEqual("https://api.github.com/repos/StanBPublic/CodeMetricsLoader/commits/21bbb99934a36899c3c65347ea886569823c9a54", commit.Url);
            Assert.IsNotNull(commit.CommitProperties);
            var commitProperties = commit.CommitProperties;
            Assert.AreEqual("Added FileName field to DimTarget", commitProperties.Message);
            Assert.IsNotNull(commitProperties.Committer);
            var committer = commitProperties.Committer;
            Assert.AreEqual("Stan", committer.Name);
            Assert.IsNotNull(committer.Date);
            Assert.AreEqual("2014-11-20T14:24:59", committer.Date.Value.ToString("s"));
            Assert.IsNotNull(commit.Committer);
            var yetAnotherCommitter = commit.Committer;
            Assert.AreEqual("StanBPublic", yetAnotherCommitter.Login);
            Assert.AreEqual("https://avatars.githubusercontent.com/u/1820912?v=3", yetAnotherCommitter.AvatarUrl);
            Assert.IsNull(yetAnotherCommitter.Date);
            Assert.IsNull(yetAnotherCommitter.Name);
        }

        [Test]
        public void GitHubProvider_Json_CanDeserializeOneCommit()
        {
            var json = GetJsonFromFile("OneCommit.json");
            var commit = JsonConvert.DeserializeObject<CodeChurnLoader.Data.Github.Commit>(json);
            Assert.IsNotNull(commit);
            Assert.AreEqual("21bbb99934a36899c3c65347ea886569823c9a54", commit.Sha);
            Assert.AreEqual("https://api.github.com/repos/StanBPublic/CodeMetricsLoader/commits/21bbb99934a36899c3c65347ea886569823c9a54", commit.Url);
            Assert.IsNotNull(commit.CommitProperties);
            var commitProperties = commit.CommitProperties;
            Assert.AreEqual("Added FileName field to DimTarget", commitProperties.Message);
            Assert.IsNotNull(commitProperties.Committer);
            var committer = commitProperties.Committer;
            Assert.AreEqual("Stan", committer.Name);
            Assert.IsNotNull(committer.Date);
            Assert.AreEqual("2014-11-20T14:24:59", committer.Date.Value.ToString("s"));
            Assert.IsNotNull(commit.Committer);
            var yetAnotherCommitter = commit.Committer;
            Assert.AreEqual("StanBPublic", yetAnotherCommitter.Login);
            Assert.AreEqual("https://avatars.githubusercontent.com/u/1820912?v=3", yetAnotherCommitter.AvatarUrl);
            Assert.IsNull(yetAnotherCommitter.Date);
            Assert.IsNull(yetAnotherCommitter.Name);

            Assert.IsNotNull(commit.Stats);            
            Assert.AreEqual(90, commit.Stats.Total);
            Assert.AreEqual(49, commit.Stats.Additions);
            Assert.AreEqual(41, commit.Stats.Deletions);            
            Assert.AreEqual(2, commit.Files.Count);
            var file = commit.Files[0];
            Assert.AreEqual("e97d008f40d4294ac98b70a1dac5fb79cf864572", file.Sha);
            Assert.AreEqual("CodeMetricsLoader.Tests/IntegrationTests/ContextTests.cs", file.Filename);
            Assert.AreEqual("modified", file.Status);
            Assert.AreEqual(4, file.Additions);
            Assert.AreEqual(3, file.Deletions);
            Assert.AreEqual(7, file.Changes);
            Assert.AreEqual("https://github.com/StanBPublic/CodeMetricsLoader/blob/21bbb99934a36899c3c65347ea886569823c9a54/CodeMetricsLoader.Tests/IntegrationTests/ContextTests.cs", file.Url); 
        }

        private string GetJsonFromFile(string fileName)
        {
            using (var reader = new StreamReader(Path.Combine(@"..\..\IntegrationTests\SampleJson", fileName)))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
