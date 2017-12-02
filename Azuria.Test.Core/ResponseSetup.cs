using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Azuria.Test.Core
{
    public static class ResponseSetup
    {
        public static Dictionary<string, string> FileResponses { get; } = GetFileResponses();

        private static Dictionary<string, string> GetFileResponses()
        {
            Dictionary<string, string> lReturn = new Dictionary<string, string>();
            Assembly lAssembly = typeof(ResponseSetup).GetTypeInfo().Assembly;
            foreach (string resourceName in lAssembly.GetManifestResourceNames()
                .Where(s => s.StartsWith("Azuria.Test.Core.Response")))
                using (StreamReader lReader = new StreamReader(lAssembly.GetManifestResourceStream(resourceName)))
                {
                    lReturn.Add(resourceName.Remove(0, "Azuria.Test.Core.Response.".Length), lReader.ReadToEnd());
                }
            return lReturn;
        }
    }
}