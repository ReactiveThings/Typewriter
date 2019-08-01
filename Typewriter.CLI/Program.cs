using CommandLine;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Typewriter.CLI
{
    public class Options
    {
        [Option('s', "solution", Required = true, HelpText = "Path to solution")]
        public string SolutionPath { get; set; }

        [Option('p', "project", Required = false, HelpText = "Path to project")]
        public string ProjectPath { get; set; }

        [Option('t', "template", Required = true, HelpText = "Paths to templates")]
        public IEnumerable<string> TemplatePaths { get; set; }

        [Option('d', "design-time", HelpText = "Indicates that a design-time build should be performed")]
        public bool DesignTime { get; set; }

        [Option('c', "clean", Default = false, HelpText = "Indicates that a clean build target should be run before compilation")]
        public bool CleanBeforeCompile { get; set; }

        [Option('v', "verbose", Default = false, HelpText = "Log all events to console")]
        public bool Verbose { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                  .WithParsed(options => Run(options));
        }

        private static void Run(Options options)
        {
            ILoggerFactory loggerFactory = options.Verbose ? new LoggerFactory().AddConsole((__,_) => true) : new LoggerFactory().AddConsole();
            using (loggerFactory)
            {
                var typewriter = new Typewriter(loggerFactory);
                typewriter.Generate(options.SolutionPath, options.ProjectPath, options.TemplatePaths, new BuildOptions()
                {
                    CleanBeforeCompile = options.CleanBeforeCompile,
                    DesignTime = options.DesignTime
                });
            }

            

        }
    }
}
