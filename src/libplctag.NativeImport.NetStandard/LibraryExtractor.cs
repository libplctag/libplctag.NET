// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace libplctag.NativeImport.NetStandard
{
    class LibraryExtractor
    {

        public static void Init(bool forceExtract = false)
        {

            var extractDirectory = GetExtractDirectory();

            if (forceExtract || !LibraryExists(extractDirectory))
            {
                ExtractAppropriateLibraryToDirectory(extractDirectory);
            }

        }

        static bool LibraryExists(string folder)
        {
            var library = GetAppropriateLibraryInfo();
            return File.Exists(Path.Combine(folder, library.FileName));
        }

        static string GetExtractDirectory()
        {
            return AppContext.BaseDirectory;
        }

        static void ExtractAppropriateLibraryToDirectory(string outputDirectory)
        {
            var library = GetAppropriateLibraryInfo();
            var embeddedResource = GetEmbeddedResource(library.ResourceName);

            if (embeddedResource == null) throw new TypeLoadException("Appropriate native library is not embedded in this wrapper library");

            var extractPath = Path.Combine(outputDirectory, library.FileName);
            File.WriteAllBytes(extractPath, embeddedResource);
        }

        static LibraryInfo GetAppropriateLibraryInfo()
        {

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && RuntimeInformation.ProcessArchitecture == Architecture.X86)
                return new LibraryInfo("libplctag.NativeImport.NetFramework.runtimes.win_x86.native", "plctag.dll");

            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && RuntimeInformation.ProcessArchitecture == Architecture.X64)
                return new LibraryInfo("libplctag.NativeImport.NetFramework.runtimes.win_x64.native", "plctag.dll");

            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && RuntimeInformation.ProcessArchitecture == Architecture.Arm)
                return new LibraryInfo("libplctag.NativeImport.NetFramework.runtimes.win_ARM.native", "plctag.dll");

            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
                return new LibraryInfo("libplctag.NativeImport.NetFramework.runtimes.win_ARM64.native", "plctag.dll");



            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && RuntimeInformation.ProcessArchitecture == Architecture.X86)
                return new LibraryInfo("libplctag.NativeImport.NetFramework.runtimes.linux_x86.native", "libplctag.so");

            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && RuntimeInformation.ProcessArchitecture == Architecture.X64)
                return new LibraryInfo("libplctag.NativeImport.NetFramework.runtimes.linux_x64.native", "libplctag.so");

            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && RuntimeInformation.ProcessArchitecture == Architecture.Arm)
                return new LibraryInfo("libplctag.NativeImport.NetFramework.runtimes.linux_ARM.native", "libplctag.so");

            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
                return new LibraryInfo("libplctag.NativeImport.NetFramework.runtimes.linux_ARM64.native", "libplctag.so");



            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) && RuntimeInformation.ProcessArchitecture == Architecture.X64)
                return new LibraryInfo("libplctag.NativeImport.NetFramework.runtimes.osx_x64.native", "libplctag.dylib");

            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) && RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
                return new LibraryInfo("libplctag.NativeImport.NetFramework.runtimes.osx_ARM64.native", "libplctag.dylib");

            else
                throw new TypeLoadException("Unknown platform");

        }

        static byte[] GetEmbeddedResource(string resourceName)
        {

            if (resourceName == null) return null;

            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null) return null;
                byte[] resource = new byte[stream.Length];
                stream.Read(resource, 0, resource.Length);
                return resource;
            }
        }

        class LibraryInfo
        {
            public string ResourceName { get; set; }
            public string FileName { get; set; }

            public LibraryInfo(string resourceNameWithoutFileName, string fileName)
            {
                ResourceName = resourceNameWithoutFileName == null ? null : $"{resourceNameWithoutFileName}.{fileName}";
                FileName = fileName;
            }
        }

    }
}
