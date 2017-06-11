namespace Azuria.Api.v1.DataModels.Manga
{
    /// <summary>
    /// </summary>
    public class PageDataModel : IDataModel
    {
        /// <summary>
        /// </summary>
        public int PageHeight { get; set; }

        /// <summary>
        /// </summary>
        public int PageWidth { get; set; }

        /// <summary>
        /// </summary>
        public string ServerFileName { get; set; }
    }
}