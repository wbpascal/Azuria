using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api.Builder;
using Azuria.ErrorHandling;
using Azuria.Middleware;
using Azuria.Requests;
using Azuria.Requests.Builder;
using NUnit.Framework;

namespace Azuria.Test.Middleware
{
    public class PipelineTest
    {
        private readonly IApiRequestBuilder _apiRequestBuilder;

        public PipelineTest()
        {
            IProxerClient client = ProxerClient.Create(new char[32]);
            this._apiRequestBuilder = client.CreateRequest();
        }

        [Test]
        public void Constructor_SetsFieldsTest()
        {
            var middleware = new IMiddleware[] {new ErrorMiddleware()};
            var pipeline = new Pipeline(middleware);

            Assert.AreEqual(middleware, pipeline.Middlewares);
        }

        [Test]
        public async Task BuildPipeline_BuildsEmptyPipelineTest()
        {
            var pipeline = new Pipeline(new IMiddleware[0]);

            MiddlewareAction action = pipeline.BuildPipeline();
            Assert.NotNull(action);

            IRequestBuilder request = this._apiRequestBuilder.FromUrl(new Uri("https://proxer.me/api/v1"));
            IProxerResult result = await action.Invoke(request);
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Test]
        public async Task BuildPipeline_BuildsPipeline()
        {
            var pipeline = new Pipeline(new IMiddleware[] {new TestMiddleware(null)});

            MiddlewareAction action = pipeline.BuildPipeline();
            Assert.NotNull(action);

            IRequestBuilder request = this._apiRequestBuilder.FromUrl(new Uri("https://proxer.me/api/v1"));
            IProxerResult result = await action.Invoke(request);
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.IsInstanceOf<ProxerApiResponse>(result);
            Assert.AreEqual("TestMiddleware", (result as ProxerApiResponse)?.Message);
        }

        [Test]
        public async Task BuildPipelineWithResult_BuildsEmptyPipelineTest()
        {
            var pipeline = new Pipeline(new IMiddleware[0]);

            MiddlewareAction<object> action = pipeline.BuildPipelineWithResult<object>();
            Assert.NotNull(action);

            IRequestBuilderWithResult<object> request =
                this._apiRequestBuilder.FromUrl(new Uri("https://proxer.me/api/v1")).WithResult<object>();
            IProxerResult<object> result = await action.Invoke(request);
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Test]
        public async Task BuildPipelineWithResult_BuildsPipeline()
        {
            var pipeline = new Pipeline(new IMiddleware[] {new TestMiddleware("TestPipeline")});

            MiddlewareAction<string> action = pipeline.BuildPipelineWithResult<string>();
            Assert.NotNull(action);

            IRequestBuilderWithResult<string> request =
                this._apiRequestBuilder.FromUrl(new Uri("https://proxer.me/api/v1")).WithResult<string>();
            IProxerResult<string> result = await action.Invoke(request);
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.IsInstanceOf<ProxerApiResponse<string>>(result);
            Assert.AreEqual("TestMiddleware", (result as ProxerApiResponse<string>)?.Message);
            Assert.AreEqual("TestPipeline", (result as ProxerApiResponse<string>)?.Result);
        }

        [Test]
        public void InsertMiddlewareAfter_DoesNotInsertIfEmpty()
        {
            var pipeline = new Pipeline(new IMiddleware[0]);

            bool inserted = pipeline.InsertMiddlewareAfter(typeof(HttpJsonRequestMiddleware), new TestMiddleware(null));
            Assert.False(inserted);
            Assert.AreEqual(0, pipeline.Middlewares.Count());
        }

        [Test]
        public void InsertMiddlewareAfter_DoesNotInsertIfNotFound()
        {
            var pipeline = new Pipeline(new IMiddleware[] {new ErrorMiddleware()});

            bool inserted = pipeline.InsertMiddlewareAfter(typeof(HttpJsonRequestMiddleware), new TestMiddleware(null));
            Assert.False(inserted);
            Assert.AreEqual(1, pipeline.Middlewares.Count());
            Assert.IsInstanceOf<ErrorMiddleware>(pipeline.Middlewares.FirstOrDefault());
        }

        [Test]
        public void InsertMiddlewareAfter_InsertsAfterEachInstancesOfType()
        {
            var pipeline = new Pipeline(new IMiddleware[] {new ErrorMiddleware(), new ErrorMiddleware()});

            bool inserted = pipeline.InsertMiddlewareAfter(typeof(ErrorMiddleware), new TestMiddleware(null));
            Assert.True(inserted);

            IMiddleware[] middlewares = pipeline.Middlewares.ToArray();
            Assert.AreEqual(4, middlewares.Length);
            Assert.IsInstanceOf<ErrorMiddleware>(middlewares[0]);
            Assert.IsInstanceOf<TestMiddleware>(middlewares[1]);
            Assert.IsInstanceOf<ErrorMiddleware>(middlewares[2]);
            Assert.IsInstanceOf<TestMiddleware>(middlewares[3]);
        }

        [Test]
        public void InsertMiddlewareAfter_InsertsBeforeInstancesOfOtherTypes()
        {
            var pipeline = new Pipeline(new IMiddleware[]
                {new ErrorMiddleware(), new StaticHeaderMiddleware(new Dictionary<string, string>())});

            bool inserted = pipeline.InsertMiddlewareAfter(typeof(ErrorMiddleware), new TestMiddleware(null));
            Assert.True(inserted);

            IMiddleware[] middlewares = pipeline.Middlewares.ToArray();
            Assert.AreEqual(3, middlewares.Length);
            Assert.IsInstanceOf<ErrorMiddleware>(middlewares[0]);
            Assert.IsInstanceOf<TestMiddleware>(middlewares[1]);
            Assert.IsInstanceOf<StaticHeaderMiddleware>(middlewares[2]);
        }

        [Test]
        public void InsertMiddlewareBefore_DoesNotInsertIfEmpty()
        {
            var pipeline = new Pipeline(new IMiddleware[0]);

            bool inserted =
                pipeline.InsertMiddlewareBefore(typeof(HttpJsonRequestMiddleware), new TestMiddleware(null));
            Assert.False(inserted);
            Assert.AreEqual(0, pipeline.Middlewares.Count());
        }

        [Test]
        public void InsertMiddlewareBefore_DoesNotInsertIfNotFound()
        {
            var pipeline = new Pipeline(new IMiddleware[] {new ErrorMiddleware()});

            bool inserted =
                pipeline.InsertMiddlewareBefore(typeof(HttpJsonRequestMiddleware), new TestMiddleware(null));
            Assert.False(inserted);
            Assert.AreEqual(1, pipeline.Middlewares.Count());
            Assert.IsInstanceOf<ErrorMiddleware>(pipeline.Middlewares.FirstOrDefault());
        }

        [Test]
        public void InsertMiddlewareBefore_InsertsBeforeEachInstancesOfType()
        {
            var pipeline = new Pipeline(new IMiddleware[] {new ErrorMiddleware(), new ErrorMiddleware()});

            bool inserted = pipeline.InsertMiddlewareBefore(typeof(ErrorMiddleware), new TestMiddleware(null));
            Assert.True(inserted);

            IMiddleware[] middlewares = pipeline.Middlewares.ToArray();
            Assert.AreEqual(4, middlewares.Length);
            Assert.IsInstanceOf<TestMiddleware>(middlewares[0]);
            Assert.IsInstanceOf<ErrorMiddleware>(middlewares[1]);
            Assert.IsInstanceOf<TestMiddleware>(middlewares[2]);
            Assert.IsInstanceOf<ErrorMiddleware>(middlewares[3]);
        }

        [Test]
        public void InsertMiddlewareBefore_InsertsAfterInstancesOfOtherTypes()
        {
            var pipeline = new Pipeline(new IMiddleware[]
                {new StaticHeaderMiddleware(new Dictionary<string, string>()), new ErrorMiddleware()});

            bool inserted = pipeline.InsertMiddlewareBefore(typeof(ErrorMiddleware), new TestMiddleware(null));
            Assert.True(inserted);

            IMiddleware[] middlewares = pipeline.Middlewares.ToArray();
            Assert.AreEqual(3, middlewares.Length);
            Assert.IsInstanceOf<StaticHeaderMiddleware>(middlewares[0]);
            Assert.IsInstanceOf<TestMiddleware>(middlewares[1]);
            Assert.IsInstanceOf<ErrorMiddleware>(middlewares[2]);
        }

        [Test]
        public void MiddlewaresProperty_OverridesMiddlewaresInSet()
        {
            var pipeline = new Pipeline(new IMiddleware[]
                {new ErrorMiddleware(), new StaticHeaderMiddleware(new Dictionary<string, string>())});

            pipeline.Middlewares = new IMiddleware[] {new TestMiddleware(null)};
            IMiddleware[] middlewares = pipeline.Middlewares.ToArray();
            Assert.NotNull(middlewares);
            Assert.AreEqual(1, middlewares.Length);
            Assert.IsInstanceOf<TestMiddleware>(middlewares[0]);
        }

        [Test]
        public void MiddlewaresProperty_ReturnsMiddlewares()
        {
            var pipeline = new Pipeline(new IMiddleware[]
                {new ErrorMiddleware(), new StaticHeaderMiddleware(new Dictionary<string, string>())});

            IMiddleware[] middlewares = pipeline.Middlewares.ToArray();
            Assert.NotNull(middlewares);
            Assert.AreEqual(2, middlewares.Length);
            Assert.IsInstanceOf<ErrorMiddleware>(middlewares[0]);
            Assert.IsInstanceOf<StaticHeaderMiddleware>(middlewares[1]);
        }

        [Test]
        public void RemoveMiddleware_DoesNotRemoveIfEmpty()
        {
            var pipeline = new Pipeline(new IMiddleware[0]);

            bool removed = pipeline.RemoveMiddleware(typeof(HttpJsonRequestMiddleware));
            Assert.False(removed);
            Assert.AreEqual(0, pipeline.Middlewares.Count());
        }

        [Test]
        public void RemoveMiddleware_DoesNotRemoveIfNotFound()
        {
            var pipeline = new Pipeline(new IMiddleware[] {new ErrorMiddleware()});

            bool removed = pipeline.RemoveMiddleware(typeof(StaticHeaderMiddleware));
            Assert.False(removed);
            Assert.AreEqual(1, pipeline.Middlewares.Count());
            Assert.IsInstanceOf<ErrorMiddleware>(pipeline.Middlewares.FirstOrDefault());
        }

        [Test]
        public void RemoveMiddleware_RemovesAllInstancesOfType()
        {
            var pipeline = new Pipeline(new IMiddleware[]
            {
                new ErrorMiddleware(), new StaticHeaderMiddleware(new Dictionary<string, string>()),
                new ErrorMiddleware()
            });

            bool removed = pipeline.RemoveMiddleware(typeof(ErrorMiddleware));
            Assert.True(removed);
            Assert.AreEqual(1, pipeline.Middlewares.Count());
            Assert.IsInstanceOf<StaticHeaderMiddleware>(pipeline.Middlewares.FirstOrDefault());
        }

        [Test]
        public void ReplaceMiddleware_DoesNotReplaceIfEmpty()
        {
            var pipeline = new Pipeline(new IMiddleware[0]);

            bool replaced = pipeline.ReplaceMiddleware(typeof(HttpJsonRequestMiddleware), new TestMiddleware(null));
            Assert.False(replaced);
            Assert.AreEqual(0, pipeline.Middlewares.Count());
        }

        [Test]
        public void ReplaceMiddleware_DoesNotReplaceIfNotFound()
        {
            var pipeline = new Pipeline(new IMiddleware[] {new ErrorMiddleware()});

            bool replaced = pipeline.ReplaceMiddleware(typeof(StaticHeaderMiddleware), new TestMiddleware(null));
            Assert.False(replaced);
            Assert.AreEqual(1, pipeline.Middlewares.Count());
            Assert.IsInstanceOf<ErrorMiddleware>(pipeline.Middlewares.FirstOrDefault());
        }

        [Test]
        public void ReplaceMiddleware_ReplacesAllInstancesOfType()
        {
            var pipeline = new Pipeline(new IMiddleware[]
            {
                new ErrorMiddleware(), new StaticHeaderMiddleware(new Dictionary<string, string>()),
                new ErrorMiddleware()
            });

            bool replaced = pipeline.ReplaceMiddleware(typeof(ErrorMiddleware), new TestMiddleware(null));
            Assert.True(replaced);

            IMiddleware[] middlewares = pipeline.Middlewares.ToArray();
            Assert.AreEqual(3, middlewares.Length);
            Assert.IsInstanceOf<TestMiddleware>(middlewares[0]);
            Assert.IsInstanceOf<StaticHeaderMiddleware>(middlewares[1]);
            Assert.IsInstanceOf<TestMiddleware>(middlewares[2]);
        }
    }
}