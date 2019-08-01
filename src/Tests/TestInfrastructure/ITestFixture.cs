using Microsoft.CodeAnalysis;
using System;
using Typewriter.Metadata.Providers;

namespace Typewriter.Tests.TestInfrastructure
{
    public interface ITestFixture : IDisposable
    {
        string SolutionPath { get; set; }
        Solution Dte { get; }
        IMetadataProvider Provider { get; }
    }
}