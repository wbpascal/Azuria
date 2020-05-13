using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Azuria.ErrorHandling;

namespace Azuria.Middleware
{
    /// <summary>
    /// The default implementation for a middleware pipeline
    /// </summary>
    public class Pipeline : IPipeline
    {
        private List<IMiddleware> _middlewares = new List<IMiddleware>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pipeline"></param>
        public Pipeline(IEnumerable<IMiddleware> pipeline)
        {
            this.Middlewares = pipeline;
        }

        /// <inheritdoc />
        public IEnumerable<IMiddleware> Middlewares
        {
            get => this._middlewares;
            set => this._middlewares = new List<IMiddleware>(value);
        }

        /// <inheritdoc />
        public MiddlewareAction BuildPipeline()
        {
            MiddlewareAction action = (request, token) => Task.FromResult((IProxerResult) new ProxerResult());

            for (int index = this._middlewares.Count - 1; index >= 0; index--)
            {
                IMiddleware middleware = this._middlewares[index];
                MiddlewareAction nextAction =
                    action; // Needed so that the anonymous function below always accesses the current method in "action"
                action = (request, token) => middleware.Invoke(request, nextAction, token);
            }

            return action;
        }

        /// <inheritdoc />
        public MiddlewareAction<T> BuildPipelineWithResult<T>()
        {
            MiddlewareAction<T> action = (request, token) =>
                Task.FromResult((IProxerResult<T>) new ProxerResult<T>(default(T)));

            for (int index = this._middlewares.Count - 1; index >= 0; index--)
            {
                IMiddleware middleware = this._middlewares[index];
                MiddlewareAction<T> nextAction =
                    action; // Needed so that the anonymous function below always accesses the current method in "action"
                action = (request, token) => middleware.InvokeWithResult(request, nextAction, token);
            }

            return action;
        }

        /// <inheritdoc />
        public bool InsertMiddlewareAfter(Type type, IMiddleware middleware)
        {
            var inserted = 0;
            for (int index = this._middlewares.Count - 1; index >= 0; index--)
            {
                IMiddleware obj = this._middlewares[index];
                if (!type.GetTypeInfo().IsAssignableFrom(obj.GetType().GetTypeInfo())) continue;

                this._middlewares.Insert(index + 1, middleware);
                inserted++;
            }

            return inserted != 0;
        }

        /// <inheritdoc />
        public bool InsertMiddlewareBefore(Type type, IMiddleware middleware)
        {
            var inserted = 0;
            for (int index = this._middlewares.Count - 1; index >= 0; index--)
            {
                IMiddleware obj = this._middlewares[index];
                if (!type.GetTypeInfo().IsAssignableFrom(obj.GetType().GetTypeInfo())) continue;

                this._middlewares.Insert(index, middleware);
                inserted++;
            }

            return inserted != 0;
        }

        /// <inheritdoc />
        public bool RemoveMiddleware(Type type)
        {
            int removed =
                this._middlewares.RemoveAll(middleware =>
                    type.GetTypeInfo().IsAssignableFrom(middleware.GetType().GetTypeInfo())
                );
            return removed != 0;
        }

        /// <inheritdoc />
        public bool ReplaceMiddleware(Type type, IMiddleware middleware)
        {
            var replaced = 0;
            for (var index = 0; index < this._middlewares.Count; index++)
            {
                IMiddleware obj = this._middlewares[index];
                if (!type.GetTypeInfo().IsAssignableFrom(obj.GetType().GetTypeInfo())) continue;

                this._middlewares[index] = middleware;
                replaced++;
            }

            return replaced != 0;
        }
    }
}