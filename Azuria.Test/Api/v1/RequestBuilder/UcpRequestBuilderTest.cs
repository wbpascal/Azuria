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
using Xunit;
using HistoryDataModel = Azuria.Api.v1.DataModels.Ucp.HistoryDataModel;
using ToptenDataModel = Azuria.Api.v1.DataModels.Ucp.ToptenDataModel;

namespace Azuria.Test.Api.v1.RequestBuilder
{
    public class UcpRequestBuilderTest : RequestBuilderTestBase<UcpRequestBuilder>
    {
        [Fact]
        public void DeleteFavouriteTest()
        {
            int lRandomId = this.GetRandomNumber(10000);

            IRequestBuilder lRequest = this.RequestBuilder.DeleteFavourite(lRandomId);
            this.CheckUrl(lRequest, "ucp", "deletefavorite");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("id"));
            Assert.Equal(lRandomId.ToString(), lRequest.PostParameter.GetValue("id").First());
            Assert.True(lRequest.CheckLogin);
        }

        [Fact]
        public void DeleteReminderTest()
        {
            int lRandomId = this.GetRandomNumber(10000);

            IRequestBuilder lRequest = this.RequestBuilder.DeleteReminder(lRandomId);
            this.CheckUrl(lRequest, "ucp", "deletereminder");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("id"));
            Assert.Equal(lRandomId.ToString(), lRequest.PostParameter.GetValue("id").First());
            Assert.True(lRequest.CheckLogin);
        }

        [Fact]
        public void DeleteVoteTest()
        {
            int lRandomId = this.GetRandomNumber(10000);

            IRequestBuilder lRequest = this.RequestBuilder.DeleteVote(lRandomId);
            this.CheckUrl(lRequest, "ucp", "deletevote");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("id"));
            Assert.Equal(lRandomId.ToString(), lRequest.PostParameter.GetValue("id").First());
            Assert.True(lRequest.CheckLogin);
        }

        [Fact]
        public void GetHistoryTest()
        {
            IRequestBuilderWithResult<HistoryDataModel[]> lRequest = this.RequestBuilder.GetHistory(3, 20);
            this.CheckUrl(lRequest, "ucp", "history");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            Assert.Equal("3", lRequest.GetParameters["p"]);
            Assert.Equal("20", lRequest.GetParameters["limit"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Theory]
        [InlineData(MediaEntryType.Anime)]
        [InlineData(MediaEntryType.Manga)]
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
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("kat"));
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            Assert.True(lRequest.GetParameters.ContainsKey("search"));
            Assert.True(lRequest.GetParameters.ContainsKey("search_start"));
            Assert.True(lRequest.GetParameters.ContainsKey("sort"));
            Assert.Equal(category.ToString().ToLowerInvariant(), lRequest.GetParameters["kat"]);
            Assert.Equal("1", lRequest.GetParameters["p"]);
            Assert.Equal("140", lRequest.GetParameters["limit"]);
            Assert.Equal(lSearch, lRequest.GetParameters["search"]);
            Assert.Equal(lSearchStart, lRequest.GetParameters["search_start"]);
            Assert.Equal("changeDateDESC", lRequest.GetParameters["sort"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Theory]
        [InlineData(UserListSort.ChangeDate, SortDirection.Ascending)]
        [InlineData(UserListSort.Name, SortDirection.Ascending)]
        [InlineData(UserListSort.StateChangeDate, SortDirection.Ascending)]
        [InlineData(UserListSort.StateName, SortDirection.Ascending)]
        [InlineData(UserListSort.ChangeDate, SortDirection.Descending)]
        [InlineData(UserListSort.Name, SortDirection.Descending)]
        [InlineData(UserListSort.StateChangeDate, SortDirection.Descending)]
        [InlineData(UserListSort.StateName, SortDirection.Descending)]
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
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("kat"));
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            Assert.True(lRequest.GetParameters.ContainsKey("search"));
            Assert.True(lRequest.GetParameters.ContainsKey("search_start"));
            Assert.True(lRequest.GetParameters.ContainsKey("sort"));
            Assert.Equal("anime", lRequest.GetParameters["kat"]);
            Assert.Equal("1", lRequest.GetParameters["p"]);
            Assert.Equal("140", lRequest.GetParameters["limit"]);
            Assert.Equal(lSearch, lRequest.GetParameters["search"]);
            Assert.Equal(lSearchStart, lRequest.GetParameters["search_start"]);
            Assert.Equal(sort.GetDescription() + sortDirection.GetDescription(), lRequest.GetParameters["sort"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Theory]
        [InlineData(MediaEntryType.Anime)]
        [InlineData(MediaEntryType.Manga)]
        public void GetListsumTest(MediaEntryType category)
        {
            IRequestBuilderWithResult<int> lRequest = this.RequestBuilder.GetListsum(category);
            this.CheckUrl(lRequest, "ucp", "listsum");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("kat"));
            Assert.Equal(category.ToString().ToLowerInvariant(), lRequest.GetParameters["kat"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(MediaEntryType.Anime)]
        [InlineData(MediaEntryType.Manga)]
        public void GetReminderTest(MediaEntryType? category)
        {
            IRequestBuilderWithResult<BookmarkDataModel[]> lRequest = this.RequestBuilder.GetReminder(category, 5, 1);
            this.CheckUrl(lRequest, "ucp", "reminder");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("kat") && category != null || category == null);
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            if (category != null)
                Assert.Equal(category?.ToString().ToLowerInvariant(), lRequest.GetParameters["kat"]);
            Assert.Equal("5", lRequest.GetParameters["p"]);
            Assert.Equal("1", lRequest.GetParameters["limit"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Fact]
        public void GetToptenTest()
        {
            IRequestBuilderWithResult<ToptenDataModel[]> lRequest = this.RequestBuilder.GetTopten();
            this.CheckUrl(lRequest, "ucp", "topten");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.CheckLogin);
        }

        [Fact]
        public void GetVotesTest()
        {
            IRequestBuilderWithResult<VoteDataModel[]> lRequest = this.RequestBuilder.GetVotes();
            this.CheckUrl(lRequest, "ucp", "votes");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.CheckLogin);
        }

        [Fact]
        public void SetCommentStateTest()
        {
            int lRandomId = this.GetRandomNumber(10000);
            int lRandomProgress = this.GetRandomNumber(50);

            IRequestBuilder lRequest = this.RequestBuilder.SetCommentState(lRandomId, lRandomProgress);
            this.CheckUrl(lRequest, "ucp", "setcommentstate");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("id"));
            Assert.True(lRequest.PostParameter.ContainsKey("value"));
            Assert.Equal(lRandomId.ToString(), lRequest.PostParameter.GetValue("id").First());
            Assert.Equal(lRandomProgress.ToString(), lRequest.PostParameter.GetValue("value").First());
            Assert.True(lRequest.CheckLogin);
        }

        [Theory]
        [InlineData(MediaLanguage.EngDub)]
        [InlineData(MediaLanguage.English)]
        [InlineData(MediaLanguage.EngSub)]
        [InlineData(MediaLanguage.GerDub)]
        [InlineData(MediaLanguage.German)]
        [InlineData(MediaLanguage.GerSub)]
        public void SetReminderLanguageTest(MediaLanguage language)
        {
            int lRandomId = this.GetRandomNumber(10000);
            int lRandomEpisode = this.GetRandomNumber(50);

            IRequestBuilder lRequest = this.RequestBuilder.SetReminder(
                lRandomId, lRandomEpisode, language, MediaEntryType.Anime);
            this.CheckUrl(lRequest, "ucp", "setreminder");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.True(lRequest.GetParameters.ContainsKey("episode"));
            Assert.True(lRequest.GetParameters.ContainsKey("language"));
            Assert.True(lRequest.GetParameters.ContainsKey("kat"));
            Assert.Equal(lRandomId.ToString(), lRequest.GetParameters["id"]);
            Assert.Equal(lRandomEpisode.ToString(), lRequest.GetParameters["episode"]);
            Assert.Equal(language.ToTypeString(), lRequest.GetParameters["language"]);
            Assert.Equal("anime", lRequest.GetParameters["kat"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Theory]
        [InlineData(MediaEntryType.Anime)]
        [InlineData(MediaEntryType.Manga)]
        public void SetReminderCategoryTest(MediaEntryType category)
        {
            int lRandomId = this.GetRandomNumber(10000);
            int lRandomEpisode = this.GetRandomNumber(50);

            IRequestBuilder lRequest = this.RequestBuilder.SetReminder(
                lRandomId, lRandomEpisode, MediaLanguage.EngDub, category);
            this.CheckUrl(lRequest, "ucp", "setreminder");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.True(lRequest.GetParameters.ContainsKey("episode"));
            Assert.True(lRequest.GetParameters.ContainsKey("language"));
            Assert.True(lRequest.GetParameters.ContainsKey("kat"));
            Assert.Equal(lRandomId.ToString(), lRequest.GetParameters["id"]);
            Assert.Equal(lRandomEpisode.ToString(), lRequest.GetParameters["episode"]);
            Assert.Equal("engdub", lRequest.GetParameters["language"]);
            Assert.Equal(category.ToString().ToLowerInvariant(), lRequest.GetParameters["kat"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Fact]
        public void SetReminderInvalidLanguageTest()
        {
            Assert.Throws<ArgumentException>(
                () => this.RequestBuilder.SetReminder(42, 42, MediaLanguage.Unkown, MediaEntryType.Anime)
            );
        }

        [Fact]
        public override void ProxerClientTest()
        {
            base.ProxerClientTest();
        }
    }
}