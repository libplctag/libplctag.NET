# libplctag.NativeImport

NOTE: Most application developers will not need to directly reference the Native Import library, it is primarily for use by other wrapper libraries.

This library automatically extracts platform-specific version of the libplctag core library needed for the libplctag .NET wrapper. 

If you wish to override this behavior you can do so: [Using a non packaged version of the native libplctag library](docs/libplctag.NativeImport.md#Using-a-non-packaged-version-of-the-native-libplctag-library.md)

Documentation for the core library can be found [here](https://github.com/libplctag/libplctag/wiki/API).
Further examples of its usage can be found [here](src/Examples/CSharp%20DotNetCore/NativeImportExample.cs).

The libplctag core library can be compiled for [many platforms](https://github.com/libplctag/libplctag#platform-support), and not all supported platforms are shipped with this wrapper. If you get a `TypeLoadException`, chances are that you can still use this wrapper but you will need to [supply the runtime yourself](https://github.com/libplctag/libplctag/blob/master/BUILD.md).

## Using a non-packaged version of the core libplctag library

## Developing for systems with immutable application directories
