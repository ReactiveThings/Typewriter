using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using Typewriter.Generation;

namespace Typewriter
{
    public static class PathResolver
    {
        public static string ResolveRelative(string path, TemplateInfo projectItem)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            if (Path.IsPathRooted(path) || projectItem.Path == null) return path;

            if (path.StartsWith("~\\"))
            {
                if(projectItem.ProjectPath == null)
                {
                    throw new NotSupportedException("'~~\\' relative project path for dll not supported when projectPath parameter is not provided");
                }
                Log.Warn("projectPath parameter will be used for '~~\\' relative project path for dll");
                var folder = Path.GetDirectoryName(projectItem.ProjectPath);
                return Path.Combine(folder, path.Substring(2));
            }
            else if (path.StartsWith("~~\\"))
            {
                var folder = Path.GetDirectoryName(projectItem.SolutionPath);
                return Path.Combine(folder, path.Substring(3));
            }
            else
            {
                var folder = Path.GetDirectoryName(projectItem.Path);
                return Path.Combine(folder, path);
            }
        }
    }
}
