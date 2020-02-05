using System;
using System.Collections.Generic;
using Azuria.ErrorHandling;

namespace Azuria.Middleware.Pipeline
{
    /// <summary>
    /// An interface that represents a middleware pipeline.
    /// TODO: Get instances of a specific middleware type
    /// </summary>
    public interface IPipeline
    {
        /// <summary>
        /// Gets or sets the middlewares contained in this pipeline and their order. Middlewares are executed in the order
        /// of this enumerable.
        /// </summary>
        IEnumerable<IMiddleware> Middlewares { get; set; }

        /// <summary>
        /// Builds the pipeline such that only the delegate that is returned must be called in order to execute the pipeline.
        /// This method will be called every time a request is executed.
        /// </summary>
        /// <returns>A middleware action that is used to execute the pipeline.</returns>
        MiddlewareAction BuildPipeline();

        /// <summary>
        /// Builds the pipeline such that only the delegate that is returned must be called in order to execute the pipeline.
        /// This method will be called every time a request is executed. The pipeline will return a <see cref="IProxerResult"/>
        /// that contains some data of type <typeparamref name="T"/>.
        /// </summary>
        /// <returns>A middleware action that is used to execute the pipeline.</returns>
        MiddlewareAction<T> BuildPipelineWithResult<T>();

        /// <summary>
        /// Inserts the given middleware after every instance of the given middleware type in the pipeline. If no instance 
        /// of the given type is found in the pipeline, the new middleware is not inserted.
        /// </summary>
        /// <param name="type">The type to search for in the pipeline.</param>
        /// <param name="middleware">The new middleware to insert.</param>
        /// <returns>True only if the new middleware was inserted into the pipeline at least once.</returns>
        bool InsertMiddlewareAfter(Type type, IMiddleware middleware);

        /// <summary>
        /// Inserts the given middleware before every instance of the given middleware type in the pipeline. If no instance 
        /// of the given type is found in the pipeline, the new middleware is not inserted.
        /// </summary>
        /// <param name="type">The type to search for in the pipeline.</param>
        /// <param name="middleware">The new middleware to insert.</param>
        /// <returns>True only if the new middleware was inserted into the pipeline at least once.</returns>
        bool InsertMiddlewareBefore(Type type, IMiddleware middleware);

        /// <summary>
        /// Removes all instances of a given middleware type from the pipeline.
        /// </summary>
        /// <param name="type">The type to remove from the pipeline.</param>
        /// <returns>True only if at least one instance was removed from the pipeline.</returns>
        bool RemoveMiddleware(Type type);

        /// <summary>
        /// Replaces all middleware instances of the given type with the given instance.
        /// </summary>
        /// <param name="type">The type that will be replaced.</param>
        /// <param name="middleware">The new middleware that will replace the instances of the type.</param>
        /// <returns>True only if the new middleware has replaced at least one instance.</returns>
        bool ReplaceMiddleware(Type type, IMiddleware middleware);
    }
}