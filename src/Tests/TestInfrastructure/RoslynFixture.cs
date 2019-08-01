using Microsoft.Extensions.Logging.Abstractions;
using System.IO;
using Typewriter.CLI;
using Typewriter.Metadata.Providers;
using Xunit;

namespace Typewriter.Tests.TestInfrastructure
{
    public class RoslynFixture : ITestFixture
    {
        public string SolutionPath { get; set; }

        public RoslynFixture()
        {
            SolutionPath = new FileInfo(@"..\..\..\..\..\Typewriter.sln").FullName;
            var o = new BuildOptions() {
                CleanBeforeCompile = true,
                DesignTime = false,
            };
            var solution = new CLI.Solution(SolutionPath, o, NullLoggerFactory.Instance);


            Dte = solution.GetProject(@"..\..\..\Typewriter.Tests.csproj").Solution;
            Provider = new RoslynMetadataProviderStub(Dte);

            // Handle threading errors when calling into Visual Studio.
            MessageFilter.Register();
        }

        public Microsoft.CodeAnalysis.Solution Dte { get; }
        public IMetadataProvider Provider { get; }

        public void Dispose()
        {
            MessageFilter.Revoke();
        }
    }

    [CollectionDefinition(nameof(RoslynFixture))]
    public class RoslynCollection : ICollectionFixture<RoslynFixture>
    {
    }
}
