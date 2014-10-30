using System;

using CommandLine;
using CommandLine.Text;

namespace CodeChurnLoader
{
    public class LoaderConfiguration
    {
        [Option('r', "repo", Required = true, HelpText = "Name of the repository")]
        public string Repo { get; set; }

        [Option('d', "day", Required = false, MutuallyExclusiveSet = "day", HelpText = "Day from midnight to  midnight")]
        public DateTime? Day { get; set; }

        [Option('f', "from", Required = false, MutuallyExclusiveSet = "from-to", HelpText = "Date from")]
        public DateTime? FromParam { get; set; }

        [Option('t', "to", Required = false, MutuallyExclusiveSet = "from-to", HelpText = "Date to")]
        public DateTime? ToParam { get; set; }

        public DateTime From { get; private set; }
        public DateTime To { get; private set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }

        public void SetupDates()
        {
            if (FromParam.HasValue && ToParam.HasValue)
            {
                this.From = FromParam.Value;
                this.To = ToParam.Value;
            }
            else
            {
                if (Day.HasValue)
                {
                    From = new DateTime(Day.Value.Year, Day.Value.Month, Day.Value.Day, 0, 0, 0);
                    To = new DateTime(Day.Value.Year, Day.Value.Month, Day.Value.Day, 23, 59, 59);
                }
                else
                {
                    throw new ApplicationException("Invalid command line parameters");
                }
            }
        }
    }
}
