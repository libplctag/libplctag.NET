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
                ExtractAppropriateLibrary(extractDirectory);
            }

        }

        static bool LibraryExists(string folder)
        {
            var library = GetAppropriateLibrary();
            return File.Exists(Path.Combine(folder, library.FileName));
        }

        static string GetExecutingAssemblyDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        static void ExtractAppropriateLibrary(string folder)
        {
            var library = GetAppropriateLibrary();
            var embeddedResource = GetEmbeddedResource(library.ResourceName);
            var extractPath = Path.Combine(folder, library.FileName);

            File.WriteAllBytes(extractPath, embeddedResource);
        }

        static LibraryInfo GetAppropriateLibrary()
        {

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && RuntimeInformation.ProcessArchitecture == Architecture.X86)
            {
                return new LibraryInfo("libplctag.NativeImport.runtime.win_x86", "plctag.dll");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && RuntimeInformation.ProcessArchitecture == Architecture.X64)
            {
                return new LibraryInfo("libplctag.NativeImport.runtime.win_x64", "plctag.dll");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && RuntimeInformation.ProcessArchitecture == Architecture.X86)
            {
                return new LibraryInfo("libplctag.NativeImport.runtime.linux_86", "libplctag.so");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && RuntimeInformation.ProcessArchitecture == Architecture.X64)
            {
                return new LibraryInfo("libplctag.NativeImport.runtime.linux_x64", "libplctag.so");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) && RuntimeInformation.ProcessArchitecture == Architecture.X64)
            {
                return new LibraryInfo("libplctag.NativeImport.runtime.osx_x64", "libplctag.dylib");
            }
            else
            {
                throw new TypeLoadException("Could not could not find the appropriate unmanaged library");
            }

        }

        static byte[] GetEmbeddedResource(string resourceName)
        {
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
                ResourceName = $"{resourceNameWithoutFileName}.{fileName}";
                FileName = fileName;
            }
        }

    }
}
