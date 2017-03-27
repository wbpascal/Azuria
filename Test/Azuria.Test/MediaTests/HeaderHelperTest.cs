using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Media.Headers;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Azuria.Test.MediaTests
{
    [TestFixture]
    public class HeaderHelperTest
    {
        [Test]
        public async Task GetHeaderListTest()
        {
            IProxerResult<IEnumerable<HeaderInfo>> lResult = await HeaderHelper.GetHeaderList();
            AssertHelper.IsSuccess(lResult);
            Assert.IsNotNull(lResult.Result);
            Assert.AreEqual(34, lResult.Result.Count());
            Assert.IsFalse(lResult.Result.Any(info => info == HeaderInfo.None));
        }

        [Test]
        public async Task GetRandomHeaderTest([Values] HeaderStyle style)
        {
            IProxerResult<HeaderInfo> lResult = await HeaderHelper.GetRandomHeader(style);
            AssertHelper.IsSuccess(lResult);
            if (style == HeaderStyle.Black || style == HeaderStyle.Gray)
            {
                Assert.IsNotNull(lResult.Result);
                Assert.AreNotEqual(lResult.Result.HeaderId, default(int));
                Assert.IsTrue(
                    new Regex(@"https:\/\/cdn\.proxer\.me\/gallery\/originals\/[\S]+?\/[\S]+").IsMatch(
                        lResult.Result.HeaderUrl.AbsoluteUri));
            }
            else
            {
                Assert.AreEqual(HeaderInfo.None, lResult.Result);
            }
        }
    }
}