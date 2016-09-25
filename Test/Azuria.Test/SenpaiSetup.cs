using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Azuria.Api;
using Azuria.Utilities.Extensions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Azuria.Test
{
    [SetUpFixture]
    public class SenpaiSetup
    {
        public static Senpai Senpai;

        [OneTimeSetUp]
        public async Task InitSenpai()
        {
            Stream lConfigStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Azuria.Test.config.json");
            Assert.NotNull(lConfigStream);

            using (StreamReader lReader = new StreamReader(lConfigStream))
            {
                Dictionary<string, string> lConfigDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(lReader.ReadToEnd());

                Senpai = new Senpai(lConfigDictionary["username"]);
                await Senpai.Login(lConfigDictionary["password"]).ThrowFirstForNonSuccess();
            }

            Assert.True(Senpai.IsProbablyLoggedIn);
        }
    }
}
