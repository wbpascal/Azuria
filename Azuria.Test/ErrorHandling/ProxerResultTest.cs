using System;
using System.Collections.Generic;
using Azuria.ErrorHandling;
using Xunit;

namespace Azuria.Test.ErrorHandling
{
    public class ProxerResultTest
    {
        [Fact]
        public void DeconstructTest()
        {
            IProxerResult GetTestResult(bool isSuccess)
            {
                return isSuccess ? new ProxerResult() : new ProxerResult(new Exception("test"));
            }

            (bool success, IEnumerable<Exception> exceptions) = GetTestResult(true);
            Assert.True(success);
            Assert.Empty(exceptions);

            (success, exceptions) = GetTestResult(false);
            Assert.False(success);
            Assert.NotEmpty(exceptions);
        }

        [Fact]
        public void DeconstructWithValueTest()
        {
            IProxerResult<int> GetTestResult(bool isSuccess)
            {
                return isSuccess ? new ProxerResult<int>(int.MaxValue) : new ProxerResult<int>(new Exception("test"));
            }

            (bool success, IEnumerable<Exception> exceptions, int value) = GetTestResult(true);
            Assert.True(success);
            Assert.Empty(exceptions);
            Assert.Equal(int.MaxValue, value);

            (success, exceptions, value) = GetTestResult(false);
            Assert.False(success);
            Assert.NotEmpty(exceptions);
            Assert.Equal(default(int), value);
        }

        [Fact]
        public void ExceptionConstructorTest()
        {
            Exception lException = new Exception("test");
            Exception[] lExceptions = {lException};

            ProxerResult lResult = new ProxerResult(lExceptions);
            Assert.False(lResult.Success);
            Assert.Equal(lExceptions, lResult.Exceptions);
            lResult = new ProxerResult(lException);
            Assert.False(lResult.Success);
            Assert.Contains(lException, lResult.Exceptions);

            ProxerResult<int> lResultWithValue = new ProxerResult<int>(lExceptions);
            Assert.False(lResultWithValue.Success);
            Assert.Equal(lExceptions, lResultWithValue.Exceptions);
            Assert.Equal(default(int), lResultWithValue.Result);
            lResultWithValue = new ProxerResult<int>(lException);
            Assert.False(lResultWithValue.Success);
            Assert.Contains(lException, lResultWithValue.Exceptions);
            Assert.Equal(default(int), lResultWithValue.Result);
        }

        [Fact]
        public void SuccessResultTest()
        {
            ProxerResult lResult = new ProxerResult();
            Assert.True(lResult.Success);
            Assert.Empty(lResult.Exceptions);

            ProxerResult<int> lResultWithValue = new ProxerResult<int>(int.MaxValue);
            Assert.True(lResultWithValue.Success);
            Assert.Empty(lResultWithValue.Exceptions);
            Assert.Equal(int.MaxValue, lResultWithValue.Result);
        }
    }
}