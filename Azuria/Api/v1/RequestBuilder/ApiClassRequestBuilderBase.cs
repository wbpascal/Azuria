using System;
using Azuria.Api.v1.Input;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract class ApiClassRequestBuilderBase : IApiClassRequestBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxerClient"></param>
        protected ApiClassRequestBuilderBase(IProxerClient proxerClient)
        {
            this.ProxerClient = proxerClient;
        }

        /// <inheritdoc />
        public IProxerClient ProxerClient { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputDataModel"></param>
        protected void CheckInputDataModel(IInputDataModel inputDataModel)
        {
            if (inputDataModel == null)
                throw new ArgumentNullException(nameof(inputDataModel));
            if (inputDataModel is IPagedInputDataModel)
                this.CheckPagedInputDataModel(inputDataModel as PagedInputDataModel);
        }

        private void CheckPagedInputDataModel(IPagedInputDataModel pagedInputDataModel)
        {
            //Not needed right now
        }
    }
}