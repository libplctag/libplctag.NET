using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using Serilog;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.HttpTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Git.GitTasks;

class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Test);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [Parameter] readonly string NugetApiUrl = "https://api.nuget.org/v3/index.json"; //default;
    [Parameter] readonly string NugetApiKey;
    [Parameter] readonly string LibplctagCoreVersion;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    Project libplctag => Solution.GetProject("libplctag");
    Project libplctag_NativeImport => Solution.GetProject("libplctag.NativeImport");


    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            DotNetClean();
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(dir => dir.DeleteDirectory());
            ArtifactsDirectory.CreateOrCleanDirectory();
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                );
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
            .SetProjectFile(Solution)
            .SetConfiguration(Configuration)
            .EnableNoRestore()
            );
        });


    Target PackLibplctag => _ => _
        .DependsOn(Test)
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetProject(libplctag)
                .SetConfiguration(Configuration)
                .EnableNoBuild()
                .EnableNoRestore()
                .SetOutputDirectory(ArtifactsDirectory)
            );

        });

    Target PackLibplctagNativeImport => _ => _
        .DependsOn(Test)
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetProject(libplctag_NativeImport)
                .SetConfiguration(Configuration)
                .EnableNoBuild()
                .EnableNoRestore()
                .SetOutputDirectory(ArtifactsDirectory)
            );

        });

    Target ReleaseLibplctag => _ => _
      .DependsOn(PackLibplctag)
      .Requires(() => NugetApiUrl)
      .Requires(() => NugetApiKey)
      .Requires(() => Configuration.Equals(Configuration.Release))
      .Executes(() =>
      {
          PushAndTag(libplctag);
      });

    Target ReleaseLibplctagNativeImport => _ => _
      .DependsOn(PackLibplctagNativeImport)
      .Requires(() => NugetApiUrl)
      .Requires(() => NugetApiKey)
      .Requires(() => Configuration.Equals(Configuration.Release))
      .Executes(() =>
      {
          PushAndTag(libplctag_NativeImport);
      });

    Target ReleaseAll => _ => _
      .DependsOn(PackLibplctag)
      .DependsOn(PackLibplctagNativeImport)
      .Requires(() => NugetApiUrl)
      .Requires(() => NugetApiKey)
      .Requires(() => Configuration.Equals(Configuration.Release))
      .Executes(() =>
      {
          PushAndTag(libplctag_NativeImport);
          PushAndTag(libplctag);
      });

    Target UpdateCoreBinaries => _ => _
        .Requires(() => LibplctagCoreVersion)
        .Executes(() =>
        {

            var runtimesFolder = RootDirectory / "src" / "libplctag.NativeImport" / "runtime";

            (string zipFileName, string[] unzipPath, AbsolutePath destination)[] releases =
            {
                ($"libplctag_{LibplctagCoreVersion}_macos_x64.zip",      ["libplctag.dylib"],        runtimesFolder/"osx_x64"/"libplctag.dylib" ),
                ($"libplctag_{LibplctagCoreVersion}_macos_aarch64.zip",  ["libplctag.dylib"],        runtimesFolder/"osx_ARM64"/"libplctag.dylib" ),
                ($"libplctag_{LibplctagCoreVersion}_ubuntu_x64.zip",     ["libplctag.so"],           runtimesFolder/"linux_x64"/"libplctag.so" ),
                ($"libplctag_{LibplctagCoreVersion}_ubuntu_x86.zip",     ["libplctag.so"],           runtimesFolder/"linux_x86"/"libplctag.so" ),
                ($"libplctag_{LibplctagCoreVersion}_linux_arm7l.zip",    ["libplctag.so"],           runtimesFolder/"linux_ARM"/"libplctag.so" ),
                ($"libplctag_{LibplctagCoreVersion}_linux_aarch64.zip",  ["libplctag.so"],           runtimesFolder/"linux_ARM64"/"libplctag.so" ),
                ($"libplctag_{LibplctagCoreVersion}_windows_x64.zip",    ["Release", "plctag.dll"],  runtimesFolder/"win_x64"/"plctag.dll" ),
                ($"libplctag_{LibplctagCoreVersion}_windows_x86.zip",    ["Release", "plctag.dll"],  runtimesFolder/"win_x86"/"plctag.dll" ),
                ($"libplctag_{LibplctagCoreVersion}_windows_Arm.zip",    ["Release", "plctag.dll"],  runtimesFolder/"win_ARM"/"plctag.dll" ),
                ($"libplctag_{LibplctagCoreVersion}_windows_Arm64.zip",  ["Release", "plctag.dll"],  runtimesFolder/"win_ARM64"/"plctag.dll" ),
            };

            var downloadFolder = RootDirectory / "downloads";
            downloadFolder.CreateOrCleanDirectory();

            releases.ForEach(release =>
            {
                var localZipFile = downloadFolder / release.zipFileName;
                HttpDownloadFile($@"https://github.com/libplctag/libplctag/releases/download/v{LibplctagCoreVersion}/{release.zipFileName}", localZipFile);

                var unzipFolder = downloadFolder / release.zipFileName + ".unzip_folder";
                unzipFolder.CreateOrCleanDirectory();
                localZipFile.UnZipTo(unzipFolder);

                var buildFile = unzipFolder / Path.Combine(release.unzipPath);
                CopyFile(buildFile, release.destination, FileExistsPolicy.Overwrite, createDirectories: true);

                // Help the user running the build to confirm that each file has been copied into the correct folder
                Log.Information("{0}    Hash at source = {1}    Hash at {2} = {3}", release.zipFileName, buildFile.GetFileHash(), release.destination, release.destination.GetFileHash());
            });

            downloadFolder.DeleteDirectory();

            // Record the bundled core version so it appears in the package version as build metadata (+core.x.y.z)
            var nativeImportProjectFile = libplctag_NativeImport.Path;
            var projectXml = nativeImportProjectFile.ReadAllText();
            var updatedXml = Regex.Replace(projectXml, @"<LibplctagCoreVersion>[^<]*</LibplctagCoreVersion>", $"<LibplctagCoreVersion>{LibplctagCoreVersion}</LibplctagCoreVersion>");
            nativeImportProjectFile.WriteAllText(updatedXml, Encoding.UTF8); // Encoding.UTF8 keeps the csproj's existing BOM
            Log.Information("Set <LibplctagCoreVersion> to {0} in {1}", LibplctagCoreVersion, nativeImportProjectFile);


            var message = $"Updating to libplctag core v{LibplctagCoreVersion}";

            Git($"add {runtimesFolder} {nativeImportProjectFile}");
            Git($"commit -m {message}");
            Git($"push origin");

        });

    private void PushAndTag(Project project)
    {
        var packageId = project.GetProperty("PackageId");
        var version = project.GetProperty("Version");

        // NuGet omits SemVer build metadata (e.g. "+core.2.6.0") from the package filename, but we keep it in the tag
        var packageFilename = $"{packageId}.{version.Split('+')[0]}.nupkg";
        var tag = $"{packageId}-v{version}";

        var output = DotNetNuGetPush(s => s
            .SetTargetPath(ArtifactsDirectory / packageFilename)
            .SetSource(NugetApiUrl)
            .SetApiKey(NugetApiKey)
            .EnableSkipDuplicate()
        );

        if (output.Any(line => line.Text.Contains("Conflict", StringComparison.InvariantCultureIgnoreCase))) {
            Log.Information("{0} already exists in package repository", packageFilename);
            return;
        }

        Git($"tag {tag}");
        Git($"push origin {tag}");
    }

}
