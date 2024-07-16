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
using Nuke.Common.Tools.NuGet;
using Nuke.Common.Tools.VSTest;
using Nuke.Common.Utilities.Collections;
using Serilog;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.HttpTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Git.GitTasks;

class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.TestLibplctag);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [Parameter] readonly string NugetApiUrl = "https://api.nuget.org/v3/index.json"; //default;
    [Parameter] readonly string NugetApiKey;
    [Parameter] readonly string LibplctagCoreVersion;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    AbsolutePath PackageRestoreDirectory => SourceDirectory / "packages";
    Project libplctag => Solution.GetProject("libplctag");
    Project libplctag_NativeImport => Solution.GetProject("libplctag.NativeImport");


    Target Clean => _ => _
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj")
            .Concat(ArtifactsDirectory)
            .Concat(PackageRestoreDirectory)
            .ForEach(dir =>
            {
                Log.Debug("Deleting {0}", dir);
                dir.DeleteDirectory();
            });
        });

    Target Compile => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(libplctag)
                .SetConfiguration(Configuration)
                );

            DotNetBuild(s => s
                .SetProjectFile(libplctag_NativeImport)
                .SetConfiguration(Configuration)
                );
        });

    Target TestLibplctag => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(libplctag)
                .SetConfiguration(Configuration)
                );
        });


    Target TestLibplctagNativeImport => _ => _
        .DependsOn(PackLibplctagNativeImport)
        .DependsOn(PackLibplctag)
        .Executes(() =>
        {
            var testsNetCore = Solution.GetAllProjects("libplctag.NativeImport.Tests.NetCore.*");
            var testsNetFramework = Solution.GetAllProjects("libplctag.NativeImport.Tests.NetFramework.*");

            var nuget_config = SourceDirectory / "libplctag.NativeImport.Tests.nuget.config";

            foreach (var proj in testsNetFramework)
            {
                var outDir = proj.GetMSBuildProject(Configuration).GetPropertyValue("OutputPath");
                var assembly = proj.Directory / outDir / proj.Name + ".dll";

                NuGetTasks.NuGetRestore(s => s
                    .SetTargetPath(proj)
                    .SetPackagesDirectory(PackageRestoreDirectory)
                    .SetConfigFile(nuget_config)
                    );

                DotNetMSBuild(s => s
                    .SetTargetPath(proj)
                    .SetConfiguration(Configuration)
                    .SetBinaryLog(assembly + ".binlog")     // VIew with https://msbuildlog.com/
                    );

                VSTestTasks.VSTest(assembly.ToString());
            }

            foreach (var proj in testsNetCore)
            {
                DotNetRestore(s => s
                    .SetProjectFile(proj)
                    .SetPackageDirectory(PackageRestoreDirectory)
                    .SetConfigFile(nuget_config)
                    );

                DotNetBuild(s => s
                    .SetProjectFile(proj)
                    .SetConfiguration(Configuration)
                    .SetNoRestore(true)
                    );

                DotNetTest(s => s
                    .SetProjectFile(proj)
                    .SetNoRestore(true)
                );
            }
        });


    Target PackLibplctag => _ => _
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
        .DependsOn(Compile)
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

    Target ReleaseAll => _ => _
      .DependsOn(TestLibplctag)
      .DependsOn(TestLibplctagNativeImport)
      .Requires(() => NugetApiUrl)
      .Requires(() => NugetApiKey)
      .Requires(() => Configuration.Equals(Configuration.Release))
      .Executes(() =>
      {
          var libplctagNativeImportVersion = libplctag_NativeImport.GetProperty("Version");
          PushAndTag($"libplctag.NativeImport.{libplctagNativeImportVersion}.nupkg", $"libplctag.NativeImport-v{libplctagNativeImportVersion}");

          var libplctagVersion = libplctag.GetProperty("Version");
          PushAndTag($"libplctag.{libplctagVersion}.nupkg", $"libplctag-v{libplctagVersion}");
      });

    Target UpdateCoreBinaries => _ => _
        .Requires(() => LibplctagCoreVersion)
        .Executes(() =>
        {

            var runtimesFolder = RootDirectory / "src" / "libplctag.NativeImport" / "runtimes";

            (string zipFileName, string[] unzipPath, AbsolutePath destination)[] releases =
            {
                ($"libplctag_{LibplctagCoreVersion}_macos_x64.zip",      ["libplctag.dylib"],        runtimesFolder/"osx-x64"/"native"/"libplctag.dylib" ),
                ($"libplctag_{LibplctagCoreVersion}_macos_aarch64.zip",  ["libplctag.dylib"],        runtimesFolder/"osx-arm64"/"native"/"libplctag.dylib" ),
                ($"libplctag_{LibplctagCoreVersion}_ubuntu_x64.zip",     ["libplctag.so"],           runtimesFolder/"linux-x64"/"native"/"libplctag.so" ),
                ($"libplctag_{LibplctagCoreVersion}_ubuntu_x86.zip",     ["libplctag.so"],           runtimesFolder/"linux-x86"/"native"/"libplctag.so" ),
                ($"libplctag_{LibplctagCoreVersion}_linux_arm7l.zip",    ["libplctag.so"],           runtimesFolder/"linux-arm"/"native"/"libplctag.so" ),
                ($"libplctag_{LibplctagCoreVersion}_linux_aarch64.zip",  ["libplctag.so"],           runtimesFolder/"linux-arm64"/"native"/"libplctag.so" ),
                ($"libplctag_{LibplctagCoreVersion}_windows_x64.zip",    ["Release", "plctag.dll"],  runtimesFolder/"win-x64"/"native"/"plctag.dll" ),
                ($"libplctag_{LibplctagCoreVersion}_windows_x86.zip",    ["Release", "plctag.dll"],  runtimesFolder/"win-x86"/"native"/"plctag.dll" ),
                ($"libplctag_{LibplctagCoreVersion}_windows_Arm.zip",    ["Release", "plctag.dll"],  runtimesFolder/"win-arm"/"native"/"plctag.dll" ),
                ($"libplctag_{LibplctagCoreVersion}_windows_Arm64.zip",  ["Release", "plctag.dll"],  runtimesFolder/"win-arm64"/"native"/"plctag.dll" ),
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

    private void PushAndTag(string packageFilename, string tag)
    {
        var output = DotNetNuGetPush(s => s
            .SetTargetPath(ArtifactsDirectory / packageFilename)
            .SetSource(NugetApiUrl)
            .SetApiKey(NugetApiKey)
            .EnableSkipDuplicate()
        );

        if (output.Any(line => line.Text.Contains("Conflict", StringComparison.InvariantCultureIgnoreCase)))
        {
            Log.Information("{0} already exists in package repository", packageFilename);
            return;
        }

        Git($"tag {tag}");
        Git($"push origin {tag}");
    }

}
