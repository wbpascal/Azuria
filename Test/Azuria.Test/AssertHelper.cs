using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using NUnit.Framework;

namespace Azuria.Test
{
    public static class AssertHelper
    {
        public static void IsSuccess(IProxerResult result)
        {
            Assert.IsTrue(result.Success,
                $"{result.Exceptions.FirstOrDefault()?.GetType().FullName}: " +
                $"{result.Exceptions.FirstOrDefault()?.Message}");
        }
    }
}
