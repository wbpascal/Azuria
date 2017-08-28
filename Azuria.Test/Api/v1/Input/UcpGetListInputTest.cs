using System.Collections.Generic;
using Azuria.Api.v1.Input;
using Azuria.Api.v1.Input.Ucp;
using Azuria.Enums;
using Xunit;

namespace Azuria.Test.Api.v1.Input
{
    public class UcpGetListInputTest
    {
        [Fact]
        public void BuildTest()
        {
            UcpGetListInput lInput = new UcpGetListInput {Category = MediaEntryType.Anime};
            Dictionary<string, string> lBuildDict = lInput.Build();
            Assert.True(lBuildDict.ContainsKey("kat"));
        }
    }
}