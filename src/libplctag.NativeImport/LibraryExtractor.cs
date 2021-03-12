using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace libplctag.NativeImport
{
    class LibraryExtractor
    {

        public static void Init(bool forceExtract = false)
        {

            var extractDirectory = GetExecutingAssemblyDirectory();

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

        static string GetExecutingAssemblyDirectory()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            return Path.GetDirectoryName(location);
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
                return new LibraryInfo("libplctag.NativeImport.runtime.win_x86", "plctag.dll");

            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && RuntimeInformation.ProcessArchitecture == Architecture.X64)
                return new LibraryInfo("libplctag.NativeImport.runtime.win_x64", "plctag.dll");

            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && RuntimeInformation.ProcessArchitecture == Architecture.Arm)
                return new LibraryInfo("libplctag.NativeImport.runtime.win_ARM", "plctag.dll");

            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
                return new LibraryInfo("libplctag.NativeImport.runtime.win_ARM64", "plctag.dll");



            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && RuntimeInformation.ProcessArchitecture == Architecture.X86)
                return new LibraryInfo("libplctag.NativeImport.runtime.linux_x86", "libplctag.so");

            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && RuntimeInformation.ProcessArchitecture == Architecture.X64)
                return new LibraryInfo("libplctag.NativeImport.runtime.linux_x64", "libplctag.so");

            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && RuntimeInformation.ProcessArchitecture == Architecture.Arm)
                return new LibraryInfo("libplctag.NativeImport.runtime.linux_ARM", "libplctag.so");

            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
                return new LibraryInfo("libplctag.NativeImport.runtime.linux_ARM64", "libplctag.so");



            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) && RuntimeInformation.ProcessArchitecture == Architecture.X64)
                return new LibraryInfo("libplctag.NativeImport.runtime.osx_x64", "libplctag.dylib");


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
