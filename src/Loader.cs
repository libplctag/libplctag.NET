using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace libplctag.NativeImport
{
    class Loader
    {
        public static void Init(string libraryFileName = null)
        {

            //var tempFolderName = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var tempFolderName = ".";
            Directory.CreateDirectory(tempFolderName);

            string newFileName;

            if (libraryFileName != null)
            {
                newFileName = Path.Combine(tempFolderName, "plctag." + Path.GetExtension(libraryFileName));
                File.Copy(libraryFileName, newFileName);
            }
            else
            {
                var embeddedResourceName = GetResourceName();
                var embeddedResourceFileName = embeddedResourceName.Item2;

                var embeddedResource = GetEmbeddedResource(embeddedResourceName.Item1 + "." + embeddedResourceName.Item2);
                newFileName = Path.Combine(tempFolderName, embeddedResourceFileName);

                File.WriteAllBytes(newFileName, embeddedResource);
            }

            IntPtr h = LoadLibrary(newFileName);
            if (h == IntPtr.Zero)
            {
                throw new NotSupportedException();
            }

        }

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern IntPtr LoadLibrary(string lpFileName);

        static (string, string) GetResourceName()
        {

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return ("libplctag.runtime.win", "plctag.dll");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return ("libplctag.runtime.linux", "plctag.so");
            }
            else
            {
                throw new NotSupportedException("Platform not supported");
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

        public static string[] GetEmbeddedLibaries()
        {
            var assembly = Assembly.GetExecutingAssembly();
            return assembly.GetManifestResourceNames();
        }

    }
}
