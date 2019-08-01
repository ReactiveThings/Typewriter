using Buildalyzer;
using Buildalyzer.Environment;
using Buildalyzer.Workspaces;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Typewriter.CLI
{
    public class Solution
    {
        private readonly AnalyzerManager manager;
        private readonly BuildOptions options;
        private ILogger logger;

        public Solution(string solutionPath, BuildOptions options, ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger("");

            AnalyzerManagerOptions analyzerOptions = new AnalyzerManagerOptions
            {
                LoggerFactory = loggerFactory,
            };

            manager = new AnalyzerManager(solutionPath, analyzerOptions);
            this.options = options;
        }

        public IEnumerable<Project> GetProjects(ICollection<string> projectNames = null)
        {
            var projects = manager.Projects
                .Where(p => projectNames == null || projectNames.Contains(p.Value.ProjectInSolution.ProjectName))
                .Select(p => p.Value);

            foreach (var projectAnalyzer in projects)
            {
                yield return BuildAndGetProject(projectAnalyzer);
            }
        }

        public Project GetProject(string projectPath)
        {
            ProjectAnalyzer projectAnalyzer = manager.GetProject(projectPath);

            return BuildAndGetProject(projectAnalyzer);
        }

        private Project BuildAndGetProject(ProjectAnalyzer projectAnalyzer)
        {
            logger.LogInformation($"Building project {projectAnalyzer.ProjectInSolution.ProjectName} and dependencies");
            AdhocWorkspace workspace = new AdhocWorkspace();
            var build = Build(projectAnalyzer).First().AddToWorkspace(workspace, true);
            logger.LogInformation($"Builded {workspace.CurrentSolution.ProjectIds.Count()} projects");
            return build;
        }

        private AnalyzerResults Build(ProjectAnalyzer projectAnalyzer)
        {
            EnvironmentOptions environmentOptions = new EnvironmentOptions();
            environmentOptions.DesignTime = options.DesignTime;
            if (!options.CleanBeforeCompile)
            {
                environmentOptions.TargetsToBuild.Remove("Clean");
            }

            return projectAnalyzer.Build(environmentOptions);
        }
    }
}
