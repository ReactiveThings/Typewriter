namespace Typewriter.CLI
{
    public class BuildOptions
    {
        /// <summary>
        ///  Indicates that a design-time build should be performed.
        ///  See https://github.com/dotnet/project-system/blob/master/docs/design-time-builds.md.
        /// </summary>
        public bool DesignTime { get; set; }
        /// <summary>
        /// Indicates that a clean target should be run before compilation
        /// </summary>
        public bool CleanBeforeCompile { get; set; }
    }
}
