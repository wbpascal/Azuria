using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.AnimeManga;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Ucp;
using Azuria.Api.v1.Enums;
using Azuria.Exceptions;
using Azuria.Utilities.ErrorHandling;
using JetBrains.Annotations;

namespace Azuria.User.ControlPanel
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HistoryEnumerator<T> : IEnumerator<AnimeMangaHistoryObject<T>> where T : IAnimeMangaObject
    {
        private const int ResultsPerPage = 50;
        private readonly UserControlPanel _controlPanel;
        private readonly Senpai _senpai;
        private AnimeMangaHistoryObject<T>[] _currentPageContent = new AnimeMangaHistoryObject<T>[0];
        private int _currentPageContentIndex = -1;
        private int _nextPage;

        internal HistoryEnumerator(Senpai senpai, UserControlPanel controlPanel)
        {
            this._senpai = senpai;
            this._controlPanel = controlPanel;
        }

        #region Properties

        /// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
        /// <returns>The element in the collection at the current position of the enumerator.</returns>
        [NotNull]
        public AnimeMangaHistoryObject<T> Current => this._currentPageContent[this._currentPageContentIndex];

        /// <summary>Gets the current element in the collection.</summary>
        /// <returns>The current element in the collection.</returns>
        object IEnumerator.Current => this.Current;

        #endregion

        #region Inherited

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            this._currentPageContent = new AnimeMangaHistoryObject<T>[0];
        }

        /// <summary>Advances the enumerator to the next element of the collection.</summary>
        /// <returns>
        ///     true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the
        ///     end of the collection.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        public bool MoveNext()
        {
            if (this._currentPageContentIndex >= this._currentPageContent.Length - 1)
            {
                if (this._currentPageContent.Length%ResultsPerPage != 0) return false;
                ProxerResult lGetSearchResult = Task.Run(this.GetNextPage).Result;
                if (!lGetSearchResult.Success)
                    throw lGetSearchResult.Exceptions.FirstOrDefault() ?? new WrongResponseException();
                this._nextPage++;
                this._currentPageContentIndex = -1;
            }
            this._currentPageContentIndex++;
            return true;
        }

        /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        public void Reset()
        {
            this._currentPageContent = new AnimeMangaHistoryObject<T>[0];
            this._currentPageContentIndex = ResultsPerPage - 1;
            this._nextPage = 0;
        }

        #endregion

        #region

        private IAnimeMangaContent<T> GetAnimeMangaContent(HistoryDataModel dataModel)
        {
            if (typeof(T) == typeof(Anime) ||
                (typeof(T) == typeof(IAnimeMangaObject) && dataModel.EntryType == AnimeMangaEntryType.Anime))
                return (IAnimeMangaContent<T>) new Anime.Episode(dataModel);
            if (typeof(T) == typeof(Manga) ||
                (typeof(T) == typeof(IAnimeMangaObject) && dataModel.EntryType == AnimeMangaEntryType.Manga))
                return (IAnimeMangaContent<T>) new Manga.Chapter(dataModel);

            return null;
        }

        [ItemNotNull]
        private async Task<ProxerResult> GetNextPage()
        {
            ProxerResult<ProxerApiResponse<HistoryDataModel[]>> lResult =
                await
                    RequestHandler.ApiRequest(ApiRequestBuilder.UcpGetHistory(this._nextPage, ResultsPerPage,
                        this._senpai));
            if (!lResult.Success || lResult.Result == null)
                return new ProxerResult(lResult.Exceptions);
            HistoryDataModel[] lData = lResult.Result.Data;

            this._currentPageContent = (from historyDataModel in lData
                select
                    new AnimeMangaHistoryObject<T>(this.GetAnimeMangaContent(historyDataModel),
                        historyDataModel.TimeStamp, this._controlPanel)).ToArray();

            return new ProxerResult();
        }

        #endregion
    }
}