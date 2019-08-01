using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Typewriter.CodeModel.Configuration;
using Typewriter.CodeModel.Implementation;
using Typewriter.Generation;
using Typewriter.Metadata.Roslyn;

namespace Typewriter.CLI
{
    public class Typewriter
    {
        private ILoggerFactory loggerFactory;
        private ILogger logger;

        public Typewriter(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;
            logger = loggerFactory.CreateLogger("");
            Log.Logger = logger;
        }

        public void Generate(string solutionPath, string projectPath, IEnumerable<string> templatePaths, BuildOptions buildOptions)
        {
            Stopwatch globalStopWatch = Stopwatch.StartNew();

            var solution = new Solution(solutionPath, buildOptions, loggerFactory);
            
            Project[] projects = !String.IsNullOrWhiteSpace(projectPath) ? solution.GetProject(projectPath).Solution.Projects.ToArray() : null;
            foreach (var templatePath in templatePaths)
            {
                var templateInfo = new TemplateInfo {
                    Path = templatePath,
                    ProjectPath = projectPath,
                    SolutionPath = solutionPath,
                };
                var template = new Template(templateInfo);
                var settings = (template.Settings as SettingsImpl);

                ICollection<string> includedProjects;
                try
                {
                    var provider = new IncludedProjectsProvider(logger);
                    includedProjects = provider.GetIncludedProjects(settings, projectPath, projects);
                }
                catch (ArgumentException e)
                {
                    logger.LogError(e.Message);
                    continue;
                }
                

                if (projects == null)
                {
                    projects = solution.GetProjects(includedProjects).ToArray();
                }

                RenderTemplateForAllProjects(template, projects.Where(p => includedProjects == null || includedProjects.Contains(p.Name)));
            }

            globalStopWatch.Stop();
            logger.LogInformation($"Done in {TimeSpan.FromMilliseconds(globalStopWatch.ElapsedMilliseconds).TotalSeconds} seconds");
        }


        private void RenderTemplateForAllProjects(Template template, IEnumerable<Project> projects)
        {
            foreach (var project in projects)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                logger.LogInformation($"Rendering template {template.TemplatePath} for project {project.Name}");

                RenderProject(template, project);

                stopwatch.Stop();
                logger.LogDebug($"Rendered in {TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds).TotalSeconds} seconds");
            }
        }

        private void RenderProject(Template template, Project project)
        {
            var metadataProvider = new RoslynMetadataProvider(project.Solution);
            foreach (var projectFile in project.Documents)
            {
                var fileMetadata = metadataProvider.GetFile(projectFile.FilePath, template.Settings, null /*todo*/);
                if (fileMetadata == null)
                {
                    // the cs-file was found, but the build-action is not set to compile.
                    continue;
                }
                var file = new FileImpl(fileMetadata);

                template.RenderFile(file);

                if (template.HasCompileException)
                {
                    break;
                }
            }
        }
    }
}
