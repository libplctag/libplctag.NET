using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
          var version = libplctag.GetProperty("Version");
          PushAndTag($"libplctag.{version}.nupkg", $"libplctag-v{version}");
      });

    Target ReleaseLibplctagNativeImport => _ => _
      .DependsOn(PackLibplctagNativeImport)
      .Requires(() => NugetApiUrl)
      .Requires(() => NugetApiKey)
      .Requires(() => Configuration.Equals(Configuration.Release))
      .Executes(() =>
      {
          var version = libplctag_NativeImport.GetProperty("Version");
          PushAndTag($"libplctag.NativeImport.{version}.nupkg", $"libplctag.NativeImport-v{version}");
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


            var message = $"Updating to libplctag core v{LibplctagCoreVersion}";

            Git($"add {runtimesFolder}");
            Git($"commit -m {message}");
            Git($"push origin");

        });


    private bool PackageExistsInNugetOrg(string packageName)
    {
        var nugetSource = "https://api.nuget.org";
        var uri = $"{nugetSource}/v3-flatcontainer/{packageName.ToLowerInvariant()}/index.json";

        try
        {
            HttpDownloadString(uri);
            return true;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            // https://github.com/alirezanet/publish-nuget/blob/master/index.js#L118
            return false;
        }
    }

    private void PushAndTag(string packageFilename, string tag)
    {
        // If the tag already exists, go no further
        if (Git($"tag -l {tag}").Any())
        {
            return;
        }

        // Push the nuget package
        DotNetNuGetPush(s => s
            .SetTargetPath(ArtifactsDirectory / packageFilename)
            .SetSource(NugetApiUrl)
            .SetApiKey(NugetApiKey)
        );

        // Add the git tag and push
        Git($"tag {tag}");
        Git($"push origin {tag}");
    }

}
