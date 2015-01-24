using System.IO;
using System.Linq;

using NUnit.Framework;

using CodeChurnLoader.Data.Bitbucket;
using System.Collections.Generic;

namespace CodeChurnLoader.Tests
{
    public class DiffParserTests
    {
        private DiffParser _diffParser;

        [TestFixtureSetUp]
        public void Setup()
        {
            _diffParser = new DiffParser();
        }

        [Test]
        public void DiffParser_Parse()
        {
            // Arrange
            string diff = GetJsonFromFile("diff1.txt");

            // Act
            var files = _diffParser.Parse(diff);

            // Assert
            Assert.AreEqual(14, files.Count);
            AssertFile(files, 0, "CodeQualityPortal.IntegrationTests/Data/ChurnRepositoryTests.cs", 17, 9);
            AssertFile(files, 1, "CodeQualityPortal.IntegrationTests/Data/MetricsRepositoryTests.cs", 8, 1);
            AssertFile(files, 2, "CodeQualityPortal/App_Start/AutoMapperConfig.cs", 13, 0);
            AssertFile(files, 3, "CodeQualityPortal/CodeQualityPortal.csproj", 3, 0);
            AssertFile(files, 4, "CodeQualityPortal/Common/HtmlHelpers.cs", 23, 1);
            AssertFile(files, 5, "CodeQualityPortal/Controllers/HomeController.cs", 10, 2);
            AssertFile(files, 6, "CodeQualityPortal/Data/CodeChurnRepository.cs", 20, 3);
            AssertFile(files, 7, "CodeQualityPortal/Data/ICodeChurnRepository.cs", 1, 0);
            AssertFile(files, 8, "CodeQualityPortal/Data/IMetricsRepository.cs", 2, 1);
            AssertFile(files, 9, "CodeQualityPortal/Data/MetricsRepository.cs", 15, 0);
            AssertFile(files, 10, "CodeQualityPortal/ViewModels/FileChurnSummary.cs", 9, 0);
            AssertFile(files, 11, "CodeQualityPortal/ViewModels/MemberSummary.cs", 10, 0);
            AssertFile(files, 12, "CodeQualityPortal/ViewModels/TopWorst.cs", 10, 0);
            AssertFile(files, 13, "CodeQualityPortal/Views/Home/Index.cshtml", 61, 45);
        }
                
        [TestCase("diff2.txt", "CodeQualityPortal/Data/IMetricsRepository.cs", 2, 1)]
        [TestCase("diff3.txt", "CodeQualityPortal/Controllers/HomeController.cs", 10, 2)]
        [TestCase("diff4.txt", "CodeQualityPortal/ViewModels/FileChurnSummary.cs", 9, 0)]
        public void DiffParser_ParseSingleDiff(string fileName, string name, int additions, int deletions)
        {
            // Arrange
            string diff = GetJsonFromFile(fileName);

            // Act
            var files = _diffParser.Parse(diff);

            // Assert
            Assert.AreEqual(1, files.Count);
            AssertFile(files, 0, name, additions, deletions);            
        }

        [Test]
        public void DiffParser_ParseRidiculouslyLongDiff()
        {
            // Arrange
            string diff = GetJsonFromFile("ridiculously-long-diff.txt");

            // Act
            var files = _diffParser.Parse(diff);

            // Assert
            Assert.AreEqual(59, files.Count);
            var blanFileNames = files.Where(f => f.FileName == string.Empty).ToList();
            Assert.AreEqual(0, blanFileNames.Count);
        }

        [Test]
        public void DiffParser_ParseNewBinaryFileDiff()
        {
            // Arrange
            string diff = GetJsonFromFile("new-binary-file-diff.txt");

            // Act
            var files = _diffParser.Parse(diff);

            // Assert
            Assert.AreEqual(1, files.Count);
            AssertFile(files, 0, "fonts/glyphicons-halflings-regular.woff", 0, 0);
        }
        
        private void AssertFile (ICollection<Data.Bitbucket.File> files, int index, string fileName, int additions, int deletions)
        {
            var file = files.ElementAt(index);
            Assert.AreEqual(fileName, file.FileName, "FileName is different");
            Assert.AreEqual(additions, file.Additions, "Additions is different");
            Assert.AreEqual(deletions, file.Deletions, "Deletions is different");
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
