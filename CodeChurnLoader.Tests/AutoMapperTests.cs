using AutoMapper;
using CodeChurnLoader.Data.Bitbucket;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeChurnLoader.Tests
{
    [TestFixture]
    public class AutoMapperTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            AutoMapperConfig.CreateMaps();
        }

        [Test]
        public void AutoMapper_Committer_NoUser()
        {
            var bitbucketCommit = new Commit { Author = new Author { Raw = "Stan <StanBPublic@live.com>" } };
            Data.Commit commit = Mapper.Map<Data.Commit>(bitbucketCommit);
            Assert.AreEqual("Stan", commit.Committer);
        }

        [Test]
        public void AutoMapper_Committer_Regular()
        {
            var bitbucketCommit = new Commit
            { 
                Author = new Author
                { 
                    Raw = "Stan <StanBPublic@live.com>",
                    User = new User {  UserName = "StanB"}
                }
            };
            Data.Commit commit = Mapper.Map<Data.Commit>(bitbucketCommit);
            Assert.AreEqual("StanB", commit.Committer);
        }
    }
}
