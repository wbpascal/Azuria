using System;
using System.Threading.Tasks;
using Azuria.Main;

namespace Azuria.Example.Models.Search
{
    public class AnimeMangaSearchModel
    {
        public AnimeMangaSearchModel(IAnimeMangaObject animeMangaObject)
        {
            this.AnimeMangaObject = animeMangaObject;
        }

        #region Properties

        public IAnimeMangaObject AnimeMangaObject { get; }

        public Uri CoverUri { get; private set; }

        public string Name { get; private set; }

        #endregion

        #region

        public async Task<AnimeMangaSearchModel> InitProperties()
        {
            this.Name = await this.AnimeMangaObject.Name.GetObject("ERROR");
            this.CoverUri = this.AnimeMangaObject.CoverUri;

            return this;
        }

        #endregion
    }
}