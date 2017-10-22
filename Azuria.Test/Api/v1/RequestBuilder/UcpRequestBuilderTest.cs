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
            DeleteToptenInput lInput = new DeleteToptenInput
            {
                ToptenId = this.GetRandomNumber(10000)
            };
            IRequestBuilder lRequest = this.RequestBuilder.DeleteTopten(lInput);
            this.CheckUrl(lRequest, "ucp", "deletefavorite");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("id"));
            Assert.AreEqual(lInput.ToptenId.ToString(), lRequest.PostParameter.GetValue("id").First());
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void DeleteReminderTest()
        {
            DeleteReminderInput lInput = new DeleteReminderInput()
            {
                ReminderId = this.GetRandomNumber(10000)
            };
            IRequestBuilder lRequest = this.RequestBuilder.DeleteReminder(lInput);
            this.CheckUrl(lRequest, "ucp", "deletereminder");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("id"));
            Assert.AreEqual(lInput.ReminderId.ToString(), lRequest.PostParameter.GetValue("id").First());
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void DeleteVoteTest()
        {
            DeleteVoteInput lInput = new DeleteVoteInput()
            {
                VoteId = this.GetRandomNumber(10000)
            };
            IRequestBuilder lRequest = this.RequestBuilder.DeleteVote(lInput);
            this.CheckUrl(lRequest, "ucp", "deletevote");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("id"));
            Assert.AreEqual(lInput.VoteId.ToString(), lRequest.PostParameter.GetValue("id").First());
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void GetHistoryTest()
        {
            UcpEntryHistoryInput lInput = new UcpEntryHistoryInput
            {
                Limit = 20,
                Page = 3
            };
            IRequestBuilderWithResult<HistoryDataModel[]> lRequest = this.RequestBuilder.GetHistory(lInput);
            this.CheckUrl(lRequest, "ucp", "history");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            Assert.AreEqual("3", lRequest.GetParameters["p"]);
            Assert.AreEqual("20", lRequest.GetParameters["limit"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void GetListCategoryTest([Values] MediaEntryType category)
        {
            string lSearch = RandomHelper.GetRandomString(20);
            string lSearchStart = RandomHelper.GetRandomString(20);

            UcpGetListInput lInputDataModel = new UcpGetListInput
            {
                Category = category,
                Search = lSearch,
                SearchStart = lSearchStart,
                Sort = UserListSort.ChangeDate,
                SortDirection = SortDirection.Descending,
                Limit = 140,
                Page = 1
            };

            IRequestBuilderWithResult<ListDataModel[]> lRequest = this.RequestBuilder.GetList(lInputDataModel);
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

        [Test, Combinatorial]
        public void GetListSortTest([Values] UserListSort sort, [Values] SortDirection sortDirection)
        {
            string lSearch = RandomHelper.GetRandomString(20);
            string lSearchStart = RandomHelper.GetRandomString(20);

            UcpGetListInput lInputDataModel = new UcpGetListInput
            {
                Category = MediaEntryType.Anime,
                Search = lSearch,
                SearchStart = lSearchStart,
                Sort = sort,
                SortDirection = sortDirection,
                Limit = 140,
                Page = 1
            };

            IRequestBuilderWithResult<ListDataModel[]> lRequest = this.RequestBuilder.GetList(lInputDataModel);
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
        public void GetListsumTest([Values] MediaEntryType category)
        {
            ListsumInput lInput = new ListsumInput
            {
                Category = category
            };
            IRequestBuilderWithResult<int> lRequest = this.RequestBuilder.GetListsum(lInput);
            this.CheckUrl(lRequest, "ucp", "listsum");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("kat"));
            Assert.AreEqual(category.ToString().ToLowerInvariant(), lRequest.GetParameters["kat"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void GetReminderTest([Values(null, MediaEntryType.Anime, MediaEntryType.Manga)] MediaEntryType? category)
        {
            ReminderListInput lInput = new ReminderListInput
            {
                Category = category,
                Limit = 1,
                Page = 5
            };
            IRequestBuilderWithResult<BookmarkDataModel[]> lRequest = this.RequestBuilder.GetReminder(lInput);
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
            SetCommentProgressInput lInput = new SetCommentProgressInput
            {
                CommentId = this.GetRandomNumber(10000),
                Progress = this.GetRandomNumber(50)
            };

            IRequestBuilder lRequest = this.RequestBuilder.SetCommentState(lInput);
            this.CheckUrl(lRequest, "ucp", "setcommentstate");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("id"));
            Assert.True(lRequest.PostParameter.ContainsKey("value"));
            Assert.AreEqual(lInput.CommentId.ToString(), lRequest.PostParameter.GetValue("id").First());
            Assert.AreEqual(lInput.Progress.ToString(), lRequest.PostParameter.GetValue("value").First());
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void SetReminderLanguageTest([Values] MediaLanguage language)
        {
            SetReminderInput lInput = new SetReminderInput
            {
                Category = MediaEntryType.Anime,
                EntryId = this.GetRandomNumber(10000),
                Episode = this.GetRandomNumber(50),
                Language = language
            };

            if (language == MediaLanguage.Unkown)
            {
                Assert.Throws<InvalidOperationException>(() => this.RequestBuilder.SetReminder(lInput));
                return;
            }

            IRequestBuilder lRequest = this.RequestBuilder.SetReminder(lInput);
            this.CheckUrl(lRequest, "ucp", "setreminder");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.True(lRequest.GetParameters.ContainsKey("episode"));
            Assert.True(lRequest.GetParameters.ContainsKey("language"));
            Assert.True(lRequest.GetParameters.ContainsKey("kat"));
            Assert.AreEqual(lInput.EntryId.ToString(), lRequest.GetParameters["id"]);
            Assert.AreEqual(lInput.Episode.ToString(), lRequest.GetParameters["episode"]);
            Assert.AreEqual(language.ToTypeString(), lRequest.GetParameters["language"]);
            Assert.AreEqual("anime", lRequest.GetParameters["kat"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void SetReminderCategoryTest([Values] MediaEntryType category)
        {
            SetReminderInput lInput = new SetReminderInput
            {
                Category = category,
                EntryId = this.GetRandomNumber(10000),
                Episode = this.GetRandomNumber(50),
                Language = MediaLanguage.EngDub
            };

            IRequestBuilder lRequest = this.RequestBuilder.SetReminder(lInput);
            this.CheckUrl(lRequest, "ucp", "setreminder");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.True(lRequest.GetParameters.ContainsKey("episode"));
            Assert.True(lRequest.GetParameters.ContainsKey("language"));
            Assert.True(lRequest.GetParameters.ContainsKey("kat"));
            Assert.AreEqual(lInput.EntryId.ToString(), lRequest.GetParameters["id"]);
            Assert.AreEqual(lInput.Episode.ToString(), lRequest.GetParameters["episode"]);
            Assert.AreEqual("engdub", lRequest.GetParameters["language"]);
            Assert.AreEqual(category.ToString().ToLowerInvariant(), lRequest.GetParameters["kat"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public override void ProxerClientTest()
        {
            base.ProxerClientTest();
        }
    }
}