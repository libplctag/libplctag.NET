# Developer Documentation

This folder is for storing information regarding the development of libplctag.NET - the .NET wrapper for libplctag.

## Glossary

|  |  |
|--|--|
| libplctag | An umbrella term used to refer to the libplctag collective and/or capability. |
| [libplctag core](https://github.com/libplctag/libplctag) | The libplctag library developed in C |
| [core binaries](https://github.com/libplctag/libplctag) | The library files (e.g. plctag.dll or libplctag.so) from libplctag core |
| [libplctag.NET](https://github.com/libplctag/libplctag.NET) | The sub-project of the libplctag organization that provides libplctag functionality for .NET projects. |
| [libplctag.NativeImport](https://www.nuget.org/packages/libplctag.NativeImport/) | The libplctag.NET package providing low-level (raw) access to libplctag core. |
| [libplctag.NET primary](https://www.nuget.org/packages/libplctag/) | The libplctag.NET package providing an API naturalised for .NET developers. |




## How to update libplctag.NativeImport

When a new version of libplctag core is released, the library binaries need to be updated in libplctag.NativeImport.
The build script can be used to copy these libraries in the project without error.

1. Run the script with the selected version:
   `> .\build.cmd UpdateCoreBinaries --libplctag-core-version 2.6.0`
2. Verify that the files have been correctly copied.
3. Make relevant modifications to libplctag.NativeImport such as modifying the method signatures (if required).
4. Increment version number of libplctag.NativeImport project.
5. Run the build script with `ReleaseLibplctagNativeImport` to upload to nuget.
   `> .\build.cmd ReleaseLibplctagNatveImport`
   Note there is a github action which automatically executes this.
