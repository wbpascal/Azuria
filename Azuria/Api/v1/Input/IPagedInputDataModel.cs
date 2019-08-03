namespace Azuria.Api.v1.Input
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a input for an api method that returns the results in multiple pages with a given amount of elements per page.
    /// </summary>
    public interface IPagedInputDataModel : IInputDataModel
    {
        /// <summary>
        /// Gets or sets the page that is to be returned. Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        int? Page { get; set; }

        /// <summary>
        /// Gets or sets the number of elements that are returned per page. 
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        int? Limit { get; set; }
    }
}