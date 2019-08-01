using System.Collections.Generic;
using Typewriter.Configuration;

namespace Typewriter.CodeModel.Configuration
{
    public class SettingsImpl : Settings
    {
        //private readonly Document _projectItem;
        public bool ShouldIncludeReferencedProjects { get; private set; }
        public bool ShouldIncludeCurrentProject { get; private set; }

        public bool ShouldIncludeAllProjects { get; private set; }
        public SettingsImpl()
        {
            //_projectItem = projectItem;
        }

        private List<string> _includedProjects;

        public override Settings IncludeProject(string projectName)
        {
            if (_includedProjects == null)
                _includedProjects = new List<string>();

            _includedProjects.Add(projectName);
            //ProjectHelpers.AddProject(_projectItem, _includedProjects, projectName);
            return this;
        }

        public override Settings IncludeReferencedProjects()
        {
            if (_includedProjects == null)
                _includedProjects = new List<string>();

            ShouldIncludeReferencedProjects = true;
            return this;
        }

        public override Settings IncludeCurrentProject()
        {
            if (_includedProjects == null)
                _includedProjects = new List<string>();

            ShouldIncludeCurrentProject = true;
            return this;
        }

        public override Settings IncludeAllProjects()
        {
            if (_includedProjects == null)
                _includedProjects = new List<string>();

            ShouldIncludeAllProjects = true;
            return this;
        }

        public ICollection<string> IncludedProjects
        {
            get
            {
                if (_includedProjects == null)
                {
                    IncludeCurrentProject();
                    IncludeReferencedProjects();
                }

                return _includedProjects;
            }
        }
    }
}
