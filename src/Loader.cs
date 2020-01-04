using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace libplctag.NativeImport
{
    class Loader
    {

        public static void Init(string customLibrary = null)
        {

            var tempFolderName = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempFolderName);

            string newFileName;

            if (customLibrary != null)
            {
                newFileName = Path.Combine(tempFolderName, "plctag." + Path.GetExtension(customLibrary));
                File.Copy(customLibrary, newFileName);
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
                var x = GetLastError();
                throw new TypeLoadException("Unable to load unmanaged library");
            }

        }

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern IntPtr GetLastError();

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern IntPtr FormatMessage();



        static (string, string) GetResourceName()
        {

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && RuntimeInformation.ProcessArchitecture == Architecture.X86)
            {
                return ("libplctag.runtime.win_x86", "plctag.dll");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && RuntimeInformation.ProcessArchitecture == Architecture.X64)
            {
                return ("libplctag.runtime.win_x86", "plctag.dll");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && RuntimeInformation.ProcessArchitecture == Architecture.X86)
            {
                return ("libplctag.runtime.linux_86", "plctag.so");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && RuntimeInformation.ProcessArchitecture == Architecture.X64)
            {
                return ("libplctag.runtime.linux_x64", "plctag.so");
            }
            else
            {
                throw new TypeLoadException("This platform is not supported, could not load appropriate unmanaged library");
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
