using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Typewriter.CodeModel.Configuration
{
    internal static class ProjectHelpers
    {
        internal static void AddProject(Document projectItem, ICollection<string> projectList, string projectName)
        {
            foreach (var project in projectItem.Project.Solution.Projects)
            {
                try
                {
                    if (project.Name == projectName)
                    {
                        AddProject(projectList, project);
                        return;
                    }
                }
                catch (Exception exception)
                {
                    Log.Error($"Cannot add project named '{projectName}' ({exception.Message})");
                }
            }

            string message = $"Cannot find project named '{projectName}'";

            //ErrorList.AddWarning(projectItem, message);
            Log.Warn(message);
        }

        internal static void AddCurrentProject(ICollection<string> projectList, Document projectItem)
        {
            AddProject(projectList, projectItem.Project);
        }

        internal static void AddReferencedProjects(ICollection<string> projectList, Document projectItem)
        {

            foreach (ProjectReference reference in projectItem.Project.ProjectReferences)
            {
                var referencedProject = projectItem.Project.Solution.Projects.Where(p => p.Id == reference.ProjectId).Single();
                AddProject(projectList, referencedProject);
            }
        }

        internal static void AddAllProjects(Document projectItem, ICollection<string> projectList)
        {
            foreach (var project in projectItem.Project.Solution.Projects)
            {
                AddProject(projectList, project);
            }
        }

        //internal static IEnumerable<string> GetProjectItems(DTE dte, ICollection<string> projectList, string filter)
        //{
        //    var directories = projectList.Select(p => new FileInfo(p).Directory);
        //    var files = directories.SelectMany(d => d.GetFiles(filter, SearchOption.AllDirectories));

        //    foreach (var file in files)
        //    {
        //        try
        //        {
        //            if (dte.Solution.FindProjectItem(file.FullName) == null)
        //                continue;
        //        }
        //        catch (Exception exception)
        //        {
        //            Log.Debug($"Cannot find project item '{file.FullName}' ({exception.Message})");
        //        }

        //        yield return file.FullName;
        //    }
        //}

        //internal static bool ProjectListContainsItem(DTE dte, string filename, ICollection<string> projectList)
        //{
        //    try
        //    {
        //        var projectItem = dte.Solution.FindProjectItem(filename);
        //        if (projectItem == null)
        //            return false;

        //        return projectList.Contains(projectItem.ContainingProject.FullName);
        //    }
        //    catch (Exception exception)
        //    {
        //        Log.Debug($"Cannot find project item '{filename}' ({exception.Message})");
        //        return false;
        //    }
        //}

        private static void AddProject(ICollection<string> projectList, Project project)
        {
            try
            {
                var filename = project?.Name;
                if (filename != null && projectList.Contains(filename) == false)
                {
                    projectList.Add(filename);
                }
            }
            catch (Exception exception)
            {
                Log.Error($"Cannot add project ({exception.Message})");
            }
        }
    }
}
