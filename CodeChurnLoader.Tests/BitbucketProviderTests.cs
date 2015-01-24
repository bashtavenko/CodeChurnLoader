using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

using NUnit.Framework;
using Moq;

using CodeChurnLoader.Data;

using CodeChurnLoader.Data.Bitbucket;
using Newtonsoft.Json;


namespace CodeChurnLoader.Tests
{
    [TestFixture]
    public class BtbbucketProviderTests
    {
        private IGitProvider _IGitProvider;
        private Mock<IBitbucketService> _serviceMock;

        [TestFixtureSetUp]
        public void SetUp()
        {            
            _serviceMock = new Mock<IBitbucketService>();
            _IGitProvider = new Data.Bitbucket.BitbucketProvider(_serviceMock.Object);
        }

        [Test]
        public void BitbucketProvider_GetCommits_Single()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetCommits(It.IsAny<string>(), It.IsAny<string>())).Returns(GetJsonFromFile("singlecommit.json"));
            _serviceMock.Setup(s => s.GetDiff(It.IsAny<string>())).Returns(GetJsonFromFile("diff2.txt"));
            
            // Act
            List<CodeChurnLoader.Data.Commit> commits = _IGitProvider.GetCommits("CodeMetricsLoader", DateTime.Now.AddYears(-2), DateTime.Now);

            // Assert
            Assert.IsNotNull(commits);
            Assert.AreEqual(1, commits.Count);
            var commit = commits[0];
            Assert.AreEqual("https://bitbucket.org/stanbpublic/codequalityportal/commits/aec313a2bfda169922dc409acff15bb17b2172f7", commit.Url);
            Assert.AreEqual("aec313a2bfda169922dc409acff15bb17b2172f7", commit.Sha);
            Assert.AreEqual("2015-01-02T16:14:32", commit.Date.ToUniversalTime().ToString("s"));
            Assert.AreEqual("Refactored churn pages.\n", commit.Message);
            Assert.AreEqual("https://bitbucket-assetroot.s3.amazonaws.com/c/photos/2014/Oct/12/stanbpublic-avatar-1580570491-7_avatar.png", commit.CommitterAvatarUrl);
            Assert.AreEqual("Stan B", commit.Committer);
            Assert.AreEqual(1, commit.Files.Count);
            var file = commit.Files[0];
            Assert.AreEqual("CodeQualityPortal/Data/IMetricsRepository.cs", file.FileName);
            Assert.AreEqual(2, file.Additions);
            Assert.AreEqual(1, file.Deletions);
            Assert.AreEqual(3, commit.Changes);
            Assert.AreEqual(1, commit.Deletions);
            Assert.AreEqual(2, commit.Additions);
        }

        [Test]
        public void BitbucketProvider_GetCommits()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetCommits(It.IsAny<string>(), It.IsAny<string>())).Returns(GetJsonFromFile("commits.json"));
            _serviceMock.Setup(s => s.GetString(It.IsAny<string>())).Returns(GetJsonFromFile("commits-lastpage.json"));
            _serviceMock.Setup(s => s.GetDiff(It.IsAny<string>())).Returns(GetJsonFromFile("diff2.txt"));

            // Act
            List<CodeChurnLoader.Data.Commit> commits = _IGitProvider.GetCommits("CodeMetricsLoader", DateTime.Now.AddYears(-2), DateTime.Now);

            // Assert
            Assert.AreEqual(60, commits.Count);
        }

        [Test]
        public void BitbucketProvider_ParseCommitWithoutUser()
        {
            var json = GetJsonFromFile("singlecommit-no-user.json");
            var header = JsonConvert.DeserializeObject<CommitHeader>(json);
            Assert.AreEqual(1, header.Commits.Count);
        }

        [Test]
        public void BitbucketProvider_GetSingleCommit()
        {
            var json = GetJsonFromFile("commit-dbf.json");
            var commit = JsonConvert.DeserializeObject<CodeChurnLoader.Data.Bitbucket.Commit>(json);            
        }
        
        private string GetJsonFromFile(string fileName)
        {
            using (var reader = new StreamReader(Path.Combine(@"..\..\SampleFiles", fileName)))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
