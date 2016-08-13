using System.IO;
using CommandLine;
using CommandLine.Text;

namespace FileComparer
{
    public class CommandLineOptions : IProcessorData
    {
        [OptionArray('i', "items", Required = true, HelpText = "Input items to be processed. Directories or files.")]
        public string[] InputItems { get; set; }

        [Option('m', "file-mask", DefaultValue = "*.*", HelpText = "File mask that will be applied as filter to the input items.")]
        public string FileMask { get; set; }

        [Option('s', "search-option", DefaultValue = SearchOption.AllDirectories, HelpText = "TopDirectoryOnly or AllDirectories.")]
        public SearchOption SearchOption { get; set; }

        [Option('r', "result-limit", DefaultValue = 10, HelpText = "Limit for results to be displayed on screen.")]
        public int ResultLimit { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
