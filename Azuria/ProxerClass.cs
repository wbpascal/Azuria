using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Api.v1.Enums;
using Azuria.ErrorHandling;
using Azuria.Media;

namespace Azuria
{
    /// <summary>
    ///     Some random functions that are to be moved later.
    /// </summary>
    public static class ProxerClass
    {
        #region Methods

        /// <summary>
        ///     Gets an <see cref="Anime" /> or <see cref="Manga" /> of a specified id.
        /// </summary>
        /// <param name="id">The id of the <see cref="Anime" /> or <see cref="Manga" />.</param>
        /// <returns>
        ///     If the action was successful and if it was, an object representing either an <see cref="Anime" /> or
        ///     <see cref="Manga" />.
        /// </returns>
        public static async Task<ProxerResult<IAnimeMangaObject>> GetAnimeMangaById(int id)
        {
            ProxerResult<ProxerApiResponse<EntryDataModel>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.InfoGetEntry(id));
            if (!lResult.Success || (lResult.Result == null))
                return new ProxerResult<IAnimeMangaObject>(lResult.Exceptions);
            EntryDataModel lDataModel = lResult.Result.Data;

            return
                new ProxerResult<IAnimeMangaObject>(lDataModel.EntryType == AnimeMangaEntryType.Anime
                    ? new Anime(lDataModel)
                    : (IAnimeMangaObject) new Manga(lDataModel));
        }

        #endregion
    }
}