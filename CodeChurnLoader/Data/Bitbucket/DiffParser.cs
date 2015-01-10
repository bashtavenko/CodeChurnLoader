using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CodeChurnLoader.Data.Bitbucket
{
    public class DiffParser
    {
        public ICollection<File> Parse (string input)
        {
            var files = new List<File>();

            var diffRegex = new Regex("diff --git");
            var fileNameRegex = new Regex("(?<=\\+\\+\\+ b/).*[^\\r\\n]");            
            var plusRegex = new Regex("^\\+", RegexOptions.Multiline); // Will match +++ as well
            var minusRegex = new Regex("^\\-", RegexOptions.Multiline);            
            var diffMatches  = diffRegex.Matches(input);
            for (var index = 0; index < diffMatches.Count; index++)
            {
                var diffMatch = diffMatches[index];
                var startIndex = diffMatch.Index;
                var nextMatch = diffMatch.NextMatch();
                var endIndex = nextMatch.Index != 0 ? nextMatch.Index : input.Length - 1;
                var diffText = input.Substring(startIndex, endIndex - startIndex + 1);
                
                var fileNameMatch = fileNameRegex.Match(diffText);
                var plusMatches = plusRegex.Matches(diffText);
                var minusMatches = minusRegex.Matches(diffText);
                files.Add(new File { FileName = fileNameMatch.Value, Additions = plusMatches.Count - 1, Deletions = minusMatches.Count - 1 });                
            }

            return files;
        }
    }
}
