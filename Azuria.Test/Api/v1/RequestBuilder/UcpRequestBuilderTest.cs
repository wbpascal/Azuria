using System;
using System.Linq;
using Azuria.Api.v1.DataModels.Ucp;
using Azuria.Api.v1.DataModels.User;
using Azuria.Api.v1.Input.Ucp;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Enums;
using Azuria.Enums.Info;
using Azuria.Enums.User;
using Azuria.Helpers.Extensions;
using Azuria.Requests.Builder;
using Azuria.Test.Core.Helpers;
using NUnit.Framework;
using HistoryDataModel = Azuria.Api.v1.DataModels.Ucp.HistoryDataModel;
using ToptenDataModel = Azuria.Api.v1.DataModels.Ucp.ToptenDataModel;

namespace Azuria.Test.Api.v1.RequestBuilder
{
    [TestFixture]
    public class UcpRequestBuilderTest : RequestBuilderTestBase<UcpRequestBuilder>
    {
        [Test]
        public void DeleteFavouriteTest()
        {
            int lRandomId = this.GetRandomNumber(10000);

            IRequestBuilder lRequest = this.RequestBuilder.DeleteFavourite(lRandomId);
            this.CheckUrl(lRequest, "ucp", "deletefavorite");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("id"));
            Assert.AreEqual(lRandomId.ToString(), lRequest.PostParameter.GetValue("id").First());
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void DeleteReminderTest()
        {
            int lRandomId = this.GetRandomNumber(10000);

            IRequestBuilder lRequest = this.RequestBuilder.DeleteReminder(lRandomId);
            this.CheckUrl(lRequest, "ucp", "deletereminder");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("id"));
            Assert.AreEqual(lRandomId.ToString(), lRequest.PostParameter.GetValue("id").First());
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void DeleteVoteTest()
        {
            int lRandomId = this.GetRandomNumber(10000);

            IRequestBuilder lRequest = this.RequestBuilder.DeleteVote(lRandomId);
            this.CheckUrl(lRequest, "ucp", "deletevote");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("id"));
            Assert.AreEqual(lRandomId.ToString(), lRequest.PostParameter.GetValue("id").First());
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void GetHistoryTest()
        {
            IRequestBuilderWithResult<HistoryDataModel[]> lRequest = this.RequestBuilder.GetHistory(3, 20);
            this.CheckUrl(lRequest, "ucp", "history");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            Assert.AreEqual("3", lRequest.GetParameters["p"]);
            Assert.AreEqual("20", lRequest.GetParameters["limit"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        [TestCase(MediaEntryType.Anime)]
        [TestCase(MediaEntryType.Manga)]
        public void GetListCategoryTest(MediaEntryType category)
        {
            string lSearch = RandomHelper.GetRandomString(20);
            string lSearchStart = RandomHelper.GetRandomString(20);

            UcpGetListInput lInputDataModel = new UcpGetListInput
            {
                Category = category,
                Search = lSearch,
                SearchStart = lSearchStart,
                Sort = UserListSort.ChangeDate,
                SortDirection = SortDirection.Descending
            };

            IRequestBuilderWithResult<ListDataModel[]> lRequest = this.RequestBuilder.GetList(lInputDataModel, 1, 140);
            this.CheckUrl(lRequest, "ucp", "list");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("kat"));
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            Assert.True(lRequest.GetParameters.ContainsKey("search"));
            Assert.True(lRequest.GetParameters.ContainsKey("search_start"));
            Assert.True(lRequest.GetParameters.ContainsKey("sort"));
            Assert.AreEqual(category.ToString().ToLowerInvariant(), lRequest.GetParameters["kat"]);
            Assert.AreEqual("1", lRequest.GetParameters["p"]);
            Assert.AreEqual("140", lRequest.GetParameters["limit"]);
            Assert.AreEqual(lSearch, lRequest.GetParameters["search"]);
            Assert.AreEqual(lSearchStart, lRequest.GetParameters["search_start"]);
            Assert.AreEqual("changeDateDESC", lRequest.GetParameters["sort"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        [TestCase(UserListSort.ChangeDate, SortDirection.Ascending)]
        [TestCase(UserListSort.Name, SortDirection.Ascending)]
        [TestCase(UserListSort.StateChangeDate, SortDirection.Ascending)]
        [TestCase(UserListSort.StateName, SortDirection.Ascending)]
        [TestCase(UserListSort.ChangeDate, SortDirection.Descending)]
        [TestCase(UserListSort.Name, SortDirection.Descending)]
        [TestCase(UserListSort.StateChangeDate, SortDirection.Descending)]
        [TestCase(UserListSort.StateName, SortDirection.Descending)]
        public void GetListSortTest(UserListSort sort, SortDirection sortDirection)
        {
            string lSearch = RandomHelper.GetRandomString(20);
            string lSearchStart = RandomHelper.GetRandomString(20);

            UcpGetListInput lInputDataModel = new UcpGetListInput
            {
                Category = MediaEntryType.Anime,
                Search = lSearch,
                SearchStart = lSearchStart,
                Sort = sort,
                SortDirection = sortDirection
            };

            IRequestBuilderWithResult<ListDataModel[]> lRequest = this.RequestBuilder.GetList(lInputDataModel, 1, 140);
            this.CheckUrl(lRequest, "ucp", "list");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("kat"));
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            Assert.True(lRequest.GetParameters.ContainsKey("search"));
            Assert.True(lRequest.GetParameters.ContainsKey("search_start"));
            Assert.True(lRequest.GetParameters.ContainsKey("sort"));
            Assert.AreEqual("anime", lRequest.GetParameters["kat"]);
            Assert.AreEqual("1", lRequest.GetParameters["p"]);
            Assert.AreEqual("140", lRequest.GetParameters["limit"]);
            Assert.AreEqual(lSearch, lRequest.GetParameters["search"]);
            Assert.AreEqual(lSearchStart, lRequest.GetParameters["search_start"]);
            Assert.AreEqual(sort.GetDescription() + sortDirection.GetDescription(), lRequest.GetParameters["sort"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        [TestCase(MediaEntryType.Anime)]
        [TestCase(MediaEntryType.Manga)]
        public void GetListsumTest(MediaEntryType category)
        {
            IRequestBuilderWithResult<int> lRequest = this.RequestBuilder.GetListsum(category);
            this.CheckUrl(lRequest, "ucp", "listsum");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("kat"));
            Assert.AreEqual(category.ToString().ToLowerInvariant(), lRequest.GetParameters["kat"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        [TestCase(null)]
        [TestCase(MediaEntryType.Anime)]
        [TestCase(MediaEntryType.Manga)]
        public void GetReminderTest(MediaEntryType? category)
        {
            IRequestBuilderWithResult<BookmarkDataModel[]> lRequest = this.RequestBuilder.GetReminder(category, 5, 1);
            this.CheckUrl(lRequest, "ucp", "reminder");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("kat") && category != null || category == null);
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            if (category != null)
                Assert.AreEqual(category?.ToString().ToLowerInvariant(), lRequest.GetParameters["kat"]);
            Assert.AreEqual("5", lRequest.GetParameters["p"]);
            Assert.AreEqual("1", lRequest.GetParameters["limit"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void GetToptenTest()
        {
            IRequestBuilderWithResult<ToptenDataModel[]> lRequest = this.RequestBuilder.GetTopten();
            this.CheckUrl(lRequest, "ucp", "topten");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void GetVotesTest()
        {
            IRequestBuilderWithResult<VoteDataModel[]> lRequest = this.RequestBuilder.GetVotes();
            this.CheckUrl(lRequest, "ucp", "votes");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void SetCommentStateTest()
        {
            int lRandomId = this.GetRandomNumber(10000);
            int lRandomProgress = this.GetRandomNumber(50);

            IRequestBuilder lRequest = this.RequestBuilder.SetCommentState(lRandomId, lRandomProgress);
            this.CheckUrl(lRequest, "ucp", "setcommentstate");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("id"));
            Assert.True(lRequest.PostParameter.ContainsKey("value"));
            Assert.AreEqual(lRandomId.ToString(), lRequest.PostParameter.GetValue("id").First());
            Assert.AreEqual(lRandomProgress.ToString(), lRequest.PostParameter.GetValue("value").First());
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        [TestCase(MediaLanguage.EngDub)]
        [TestCase(MediaLanguage.English)]
        [TestCase(MediaLanguage.EngSub)]
        [TestCase(MediaLanguage.GerDub)]
        [TestCase(MediaLanguage.German)]
        [TestCase(MediaLanguage.GerSub)]
        public void SetReminderLanguageTest(MediaLanguage language)
        {
            int lRandomId = this.GetRandomNumber(10000);
            int lRandomEpisode = this.GetRandomNumber(50);

            IRequestBuilder lRequest = this.RequestBuilder.SetReminder(
                lRandomId, lRandomEpisode, language, MediaEntryType.Anime);
            this.CheckUrl(lRequest, "ucp", "setreminder");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.True(lRequest.GetParameters.ContainsKey("episode"));
            Assert.True(lRequest.GetParameters.ContainsKey("language"));
            Assert.True(lRequest.GetParameters.ContainsKey("kat"));
            Assert.AreEqual(lRandomId.ToString(), lRequest.GetParameters["id"]);
            Assert.AreEqual(lRandomEpisode.ToString(), lRequest.GetParameters["episode"]);
            Assert.AreEqual(language.ToTypeString(), lRequest.GetParameters["language"]);
            Assert.AreEqual("anime", lRequest.GetParameters["kat"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        [TestCase(MediaEntryType.Anime)]
        [TestCase(MediaEntryType.Manga)]
        public void SetReminderCategoryTest(MediaEntryType category)
        {
            int lRandomId = this.GetRandomNumber(10000);
            int lRandomEpisode = this.GetRandomNumber(50);

            IRequestBuilder lRequest = this.RequestBuilder.SetReminder(
                lRandomId, lRandomEpisode, MediaLanguage.EngDub, category);
            this.CheckUrl(lRequest, "ucp", "setreminder");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.True(lRequest.GetParameters.ContainsKey("episode"));
            Assert.True(lRequest.GetParameters.ContainsKey("language"));
            Assert.True(lRequest.GetParameters.ContainsKey("kat"));
            Assert.AreEqual(lRandomId.ToString(), lRequest.GetParameters["id"]);
            Assert.AreEqual(lRandomEpisode.ToString(), lRequest.GetParameters["episode"]);
            Assert.AreEqual("engdub", lRequest.GetParameters["language"]);
            Assert.AreEqual(category.ToString().ToLowerInvariant(), lRequest.GetParameters["kat"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void SetReminderInvalidLanguageTest()
        {
            Assert.Throws<ArgumentException>(
                () => this.RequestBuilder.SetReminder(42, 42, MediaLanguage.Unkown, MediaEntryType.Anime)
            );
        }

        [Test]
        public override void ProxerClientTest()
        {
            base.ProxerClientTest();
        }
    }
}