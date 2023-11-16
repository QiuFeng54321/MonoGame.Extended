using Cake.Common.IO;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.MSBuild;
using Cake.Common.Tools.DotNet.Pack;
using Cake.Core.IO;
using Cake.Frosting;

namespace BuildScripts;

[TaskName(nameof(PackageTask))]
public sealed class PackageTask : FrostingTask<BuildContext>
{
    public override async void Run(BuildContext context)
    {
        context.CleanDirectories(context.ArtifactsDirectory);
        context.CreateDirectory(context.ArtifactsDirectory);

        DotNetMSBuildSettings msBuildSettings = new DotNetMSBuildSettings();
        msBuildSettings.WithProperty("Version", context.Version);

        DotNetPackSettings packSettings = new DotNetPackSettings()
        {
            MSBuildSettings = msBuildSettings,
            Configuration = "Release",
            Verbosity = DotNetVerbosity.Minimal,
            NoLogo = true,
            OutputDirectory = context.ArtifactsDirectory,
        };

        FilePathCollection files = context.GetFiles("./src/cs/MonoGame.Extended*/**/*.csproj");
        foreach(FilePath file in files)
        {
            context.DotNetPack(file.FullPath, packSettings);
        }
    }
}
