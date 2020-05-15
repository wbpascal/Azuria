using System;
using System.Collections.Generic;
using System.Linq;
using Azuria.ErrorHandling;
using NUnit.Framework;

namespace Azuria.Test.ErrorHandling
{
    // TODO: Test constructor for ProxerResultBase (used to convert ProxerResult <-> ProxerResult<T>)
    [TestFixture]
    public class ProxerResultTest
    {
        [Test]
        public void DeconstructTest()
        {
            IProxerResult GetTestResult(bool isSuccess)
            {
                return isSuccess ? new ProxerResult() : new ProxerResult(new Exception("test"));
            }

            (bool success, IEnumerable<Exception> exceptions) = GetTestResult(true);
            Assert.True(success);
            Assert.IsEmpty(exceptions);

            (success, exceptions) = GetTestResult(false);
            Assert.False(success);
            Assert.IsNotEmpty(exceptions);
        }

        [Test]
        public void DeconstructWithValueTest()
        {
            IProxerResult<int> GetTestResult(bool isSuccess)
            {
                return isSuccess ? new ProxerResult<int>(int.MaxValue) : new ProxerResult<int>(new Exception("test"));
            }

            (bool success, IEnumerable<Exception> exceptions, int value) = GetTestResult(true);
            Assert.True(success);
            Assert.IsEmpty(exceptions);
            Assert.AreEqual(int.MaxValue, value);

            (success, exceptions, value) = GetTestResult(false);
            Assert.False(success);
            Assert.IsNotEmpty(exceptions);
            Assert.AreEqual(default(int), value);
        }

        [Test]
        public void ExceptionConstructorTest()
        {
            var lException = new Exception("test");
            Exception[] lExceptions = {lException};

            var lResult = new ProxerResult(lExceptions);
            Assert.False(lResult.Success);
            Assert.AreEqual(lExceptions, lResult.Exceptions);
            lResult = new ProxerResult(lException);
            Assert.False(lResult.Success);
            Assert.Contains(lException, lResult.Exceptions.ToArray());

            var lResultWithValue = new ProxerResult<int>(lExceptions);
            Assert.False(lResultWithValue.Success);
            Assert.AreEqual(lExceptions, lResultWithValue.Exceptions);
            Assert.AreEqual(default(int), lResultWithValue.Result);
            lResultWithValue = new ProxerResult<int>(lException);
            Assert.False(lResultWithValue.Success);
            Assert.Contains(lException, lResultWithValue.Exceptions.ToArray());
            Assert.AreEqual(default(int), lResultWithValue.Result);
        }

        [Test]
        public void SuccessResultTest()
        {
            var lResult = new ProxerResult();
            Assert.True(lResult.Success);
            Assert.IsEmpty(lResult.Exceptions);

            var lResultWithValue = new ProxerResult<int>(int.MaxValue);
            Assert.True(lResultWithValue.Success);
            Assert.IsEmpty(lResultWithValue.Exceptions);
            Assert.AreEqual(int.MaxValue, lResultWithValue.Result);
        }
    }
}