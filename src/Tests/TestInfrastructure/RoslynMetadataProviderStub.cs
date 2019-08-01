using System.Linq;
using Typewriter.Metadata.Interfaces;
using Typewriter.Metadata.Providers;
using Typewriter.Metadata.Roslyn;
using Typewriter.Configuration;
using System;
using Typewriter.CLI;
using Microsoft.Extensions.Logging.Abstractions;

namespace Typewriter.Tests.TestInfrastructure
{
    public class RoslynMetadataProviderStub : IMetadataProvider
    {
        private readonly Microsoft.CodeAnalysis.Workspace workspace;

        public RoslynMetadataProviderStub(Microsoft.CodeAnalysis.Solution dte)
        {
            workspace = dte.Workspace;
        }

        public IFileMetadata GetFile(string path, Settings settings, Action<string[]> requestRender)
        {
            var document = workspace.CurrentSolution.GetDocumentIdsWithFilePath(path).FirstOrDefault();
            if (document != null)
            {
                return new RoslynFileMetadata(workspace.CurrentSolution.GetDocument(document), settings, requestRender);
            }

            return null;
        }
    }
}
