using System;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.List;
using Azuria.Api.v1.Enums;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Info;
using Azuria.Media.Properties;
using Azuria.Search.Input;
using NUnit.Framework;

// ReSharper disable AsyncConverter.ConfigureAwaitHighlighting

namespace Azuria.Test.ApiTests
{
    [TestFixture]
    public class ListTest
    {
        [Test]
        public async Task GetTagIdsTest()
        {
            ProxerApiResponse<Tuple<int[], int[]>> lResult =
                await RequestHandler.ApiRequest(ListRequestBuilder.GetTagIds("yuri -action"));
            AssertHelper.IsSuccess(lResult);
            Assert.AreEqual(lResult.Result.Item1, new[] {206});
            Assert.AreEqual(lResult.Result.Item2, new[] {175});
        }

        [Test]
        public async Task GetTranslatorgroupsProjectsTest()
        {
            ProxerApiResponse<TranslatorProjectDataModel[]> lResult = await RequestHandler.ApiRequest(
                ListRequestBuilder.GetTranslatorProjects(1, TranslationStatus.Cancelled, null));
            AssertHelper.IsSuccess(lResult);
            Assert.AreEqual(1, lResult.Result.Length);

            TranslatorProjectDataModel lDataModel = lResult.Result[0];
            Assert.AreEqual(3247, lDataModel.EntryId);
            Assert.AreEqual("Accel World", lDataModel.EntryName);
            Assert.AreEqual(new[] {Fsk.Fsk12}, lDataModel.EntryFsk);
            Assert.Contains(Genre.Action, lDataModel.EntryGenre);
            Assert.Contains(Genre.Romance, lDataModel.EntryGenre);
            Assert.Contains(Genre.Shounen, lDataModel.EntryGenre);
            Assert.AreEqual(MediaMedium.Animeseries, lDataModel.EntryMedium);
            Assert.AreEqual(MediaEntryType.Anime, lDataModel.EntryType);
            Assert.AreEqual(MediaStatus.Completed, lDataModel.EntryStatus);
            Assert.AreEqual(49674, lDataModel.EntryRatingsSum);
            Assert.AreEqual(6164, lDataModel.EntryRatingsCount);
            Assert.AreEqual(TranslationStatus.Cancelled, lDataModel.Status);
        }
    }
}