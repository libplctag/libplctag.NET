# Developer Readme

This readme is for developers of libplctag.NET - the .NET wrapper for libplctag.

The term "libplctag" is ambiguous, so for the purposes of this document we will use the following interpretation:

|  |  |
|--|--|
| libplctag | An umbrella term used to refer to the libplctag collective or capability in high-level terms. |
| [libplctag native](https://github.com/libplctag/libplctag) | The primary libplctag library developed in C |
| [libplctag.NET](https://github.com/libplctag/libplctag.NET) | The sub-project of the libplctag organization that provides libplctag for .NET projects. |
| [libplctag.NativeImport](https://www.nuget.org/packages/libplctag.NativeImport/) | The package providing low-level (raw) access to libplctag native |
| [libplctag ???](https://www.nuget.org/packages/libplctag/) | The package providing an API for libplctag that should feel natural to .NET developers. |




## Updating libplctag.NativeImport

When a new version of libplctag native is released, the binaries need to be packed into libplctag.NativeImport.
The build script can be used to copy these libraries in the project without error.

1. Run the script with the selected version:

   `> build.cmd CopyNativeFiles --native-file-version 2.6.0`

2. Verify that the files have been correctly copied
3. Make relevant modifications to libplctag.NativeImport such as modifying the method signatures (if required)
4. Increment version number of libplctag.NativeImport project
5. Upload to nuget
   Note there is a github action which automatically builds libplctag.NativeImport and uploads to nuget
