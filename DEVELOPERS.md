# Developer Documentation

This folder is for storing information regarding the development of libplctag.NET.

## Project Goals

* Package the libplctag functionality in a way that is convenient to use in .NET applications.
* Be cross-platform: It should support any platform that libplctag can be built for, and supports .NET Standard 2.0


## Glossary

|  |  |
|--|--|
| libplctag | An umbrella term used to refer to the libplctag collective and/or capability. |
| [libplctag core](https://github.com/libplctag/libplctag) | The libplctag library developed in C |
| [core binaries](https://github.com/libplctag/libplctag) | The library files (e.g. plctag.dll or libplctag.so) from libplctag core |
| [libplctag.NET](https://github.com/libplctag/libplctag.NET) | The sub-project of the libplctag organization that provides libplctag functionality for .NET projects. |
| [libplctag.NativeImport](https://www.nuget.org/packages/libplctag.NativeImport/) | The libplctag.NET package providing low-level (raw) access to libplctag core. |
| [libplctag.NET primary](https://www.nuget.org/packages/libplctag/) | The libplctag.NET package providing an API naturalised for .NET developers. |



## Build Automation

GitHub Actions have been created to automate some common activities such as updating the core binaries and release nuget packages.

These workflows delegate the actual automation logic to the [`_build`](../build) [NUKE](https://nuke.build/) project.

## Testing libplctag.NativeImport

libplctag.NativeImport uses features of Nuget and MSBuild to distribute the libplctag core binaries.
What actually needs to be tested is that logic, which xUnit and other unit-testing frameworks are not well equipped to do.
The build project, however, is capable - and does this by creating orchestrating a package creation, and using it in the restore process for those test suites.

Different versions of .NET use packages differently (see [PackageReference vs packags.config](https://learn.microsoft.com/en-us/nuget/consume-packages/migrate-packages-config-to-package-reference)), so there are different test projects for each variation.


## How to release Nuget packages

1. Update the version number in the csproj files of the libplctag or libplctag.NativeImport projects.
2. Trigger the "Build and publish libplctag.NET nuget package" workflow.
4. Create a GitHub "Release" with details of the new release.

## How to update the libplctag core binaries

When a new version of libplctag core is released, the library binaries need to be updated in libplctag.NativeImport.
The build script can be used to copy these libraries in the project without error.

1. Trigger the "Update libplctag core" workflow.
2. Verify that the files have been correctly copied.
3. Make relevant modifications to libplctag.NativeImport such as modifying the method signatures (if required).
4. Release the updated project as a new Nuget package.
5. Finally, update the libplctag.NativeImport `<PackageReference>` version in the libplctag package, and release that.
