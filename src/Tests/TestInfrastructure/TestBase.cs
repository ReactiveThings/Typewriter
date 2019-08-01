using System.IO;
using Typewriter.CodeModel.Implementation;
using Typewriter.Metadata.Providers;
using Xunit;
using File = Typewriter.CodeModel.File;
using Typewriter.Configuration;
using Typewriter.CodeModel.Configuration;
using System;
using Typewriter.Generation;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Typewriter.Tests.TestInfrastructure
{
    public abstract class TestBase
    {
        protected readonly IMetadataProvider metadataProvider;

        protected readonly bool isRoslyn;
        protected readonly bool isCodeDom;

        protected TestBase(ITestFixture fixture)
        {
            this.solutionPath = fixture.SolutionPath;
            this.SolutionDirectory = Path.Combine(new FileInfo(fixture.SolutionPath).Directory?.FullName, "src");
            this.metadataProvider = fixture.Provider;

            this.isRoslyn = fixture is RoslynFixture;
            this.isCodeDom = false;
        }

        private string solutionPath;
        protected string SolutionDirectory;

        protected TemplateInfo GetProjectItem(string path)
        {
            return new TemplateInfo
            {
                Path = path,
                SolutionPath = solutionPath,
                //ProjectPath = ""
            };
        }

        protected string GetFileContents(string path)
        {
            return System.IO.File.ReadAllText(Path.Combine(SolutionDirectory, path));
        }

        protected File GetFile(string path, Settings settings = null, Action<string[]> requestRender = null)
        {
            if (settings == null)
                settings = new SettingsImpl();

            var metadata = metadataProvider.GetFile(Path.Combine(SolutionDirectory, path), settings, requestRender);
            return new FileImpl(metadata);
        }
    }
}