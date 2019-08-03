using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input
{
    /// <inheritdoc cref="IPagedInputDataModel" />
    public abstract class PagedInputDataModel : InputDataModel, IPagedInputDataModel
    {
        /// <inheritdoc />
        [InputData("p", Optional = true)]
        public int? Page { get; set; }

        /// <inheritdoc />
        [InputData("limit", Optional = true)]
        public int? Limit { get; set; }
    }
}