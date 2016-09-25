using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Azuria.Api;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Azuria.Test
{
    [SetUpFixture]
    public class ApiSetup
    {
        [OneTimeSetUp]
        public void InitApi()
        {
            Stream lConfigStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Azuria.Test.config.json");
            Assert.NotNull(lConfigStream);

            using (StreamReader lReader = new StreamReader(lConfigStream))
            {
                Dictionary<string, string> lConfigDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(lReader.ReadToEnd());

                ApiInfo.Init(input =>
                {
                    input.ApiKeyV1 = lConfigDictionary["apiKey"].ToCharArray();
                });
            }
        }
    }
}
