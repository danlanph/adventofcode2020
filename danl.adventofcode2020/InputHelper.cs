using System;
using System.IO;

namespace danl.adventofcode2020
{
    public static class InputHelper
    {
        public const string LineEnding = "\n";

        public static string GetResourceFileAsString(string resourceName)
        {
            using (var resourceStream = System.Reflection.Assembly.GetCallingAssembly().GetManifestResourceStream(resourceName))
            {
                using (var streamReader = new StreamReader(resourceStream))
                {
                    return streamReader.ReadToEnd().Replace("\r\n", "\n");
                }
            }
        }
    }
}
