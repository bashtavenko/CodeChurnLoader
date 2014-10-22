using System.Data.Entity;
using System.Linq;

using NUnit.Framework;

using CodeChurnLoader.Data;


namespace CodeChurnLoader.Tests.IntegrationTests
{
    [TestFixture]
    public class ContextTests
    {
        private LoaderContext _context;
        private static string _databaseName = "CodeChurnLoaderWarehouseTEST";

        public static LoaderContext CreateTestContext()
        {
            return new LoaderContext(_databaseName, new DropCreateDatabaseAlways<LoaderContext>());            
        }

        [TestFixtureSetUp]
        public void Setup()
        {
            _context = CreateTestContext();
        }

        [Test]
        public void LoaderContext_SmokeTest_CanInitDatabase()
        {
            var dates = _context.Dates.ToList();
        }

        [Test]
        public void LoaderContext_Save_CanSave()
        {
            var repo = new DimRepo
            {
                Name = "CodeMetricsLoader"                
            };

            var commit = new DimCommit
            {
                Sha = "9c4800fdeb47aa8f990105fd894ab1f125efc51e",
                Url = "https://api.github.com/repos/StanBPublic/CodeMetricsLoader/commits/9c4800fdeb47aa8f990105fd894ab1f125efc51e"

            };
            repo.Commits.Add(commit);
            _context.Repos.Add(repo);
                        
            var date = new DimDate();
            var churn = new FactCodeChurn
            {                
                Commit = commit,
                Date = date,
                LinesAdded = 10,
                LinesDeleted = 2,
                LinesModified = 1
            };               
            _context.Churn.Add(churn);
            _context.SaveChanges();            
        }
    }
}
