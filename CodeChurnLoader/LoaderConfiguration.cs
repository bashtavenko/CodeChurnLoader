using System;

using CommandLine;
using CommandLine.Text;

namespace CodeChurnLoader
{
    public class LoaderConfiguration
    {
        [Option('r', "repo", Required = true, HelpText = "Name of the repository")]
        public string Repo { get; set; }
        
        [Option('f', "from", Required = true, HelpText = "Date from")]
        public DateTime From { get; set; }

        [Option('t', "to", Required = true, HelpText = "Date to")]
        public DateTime To { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }        
    }
}
