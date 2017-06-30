using System;
using System.Linq;
using Azuria.Api.v1.DataModels;
using Azuria.Api.v1.DataModels.User;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Enums;
using Azuria.Enums.User;
using Azuria.Helpers.Extensions;
using Azuria.Requests.Builder;
using Azuria.Test.Core.Helpers;
using Xunit;

namespace Azuria.Test.Api.v1.RequestBuilder
{
    public class UserRequestBuilderTest : RequestBuilderTestBase<UserRequestBuilder>
    {
        public void GetHistoryTestBase(IRequestBuilderBase requestBuilderBase)
        {
            this.CheckUrl(requestBuilderBase, "user", "history");
            Assert.Same(this.ProxerClient, requestBuilderBase.Client);
            Assert.True(requestBuilderBase.GetParameters.ContainsKey("p"));
            Assert.True(requestBuilderBase.GetParameters.ContainsKey("limit"));
            Assert.Equal("4", requestBuilderBase.GetParameters["p"]);
            Assert.Equal("132", requestBuilderBase.GetParameters["limit"]);
            Assert.False(requestBuilderBase.CheckLogin);
        }

        [Fact]
        public void GetHistoryUsernameTest()
        {
            string lRandomUsername = RandomHelper.GetRandomString(10);
            IRequestBuilderWithResult<HistoryDataModel[]> lRequest = this.RequestBuilder.GetHistory(
                lRandomUsername, 4, 132);
            this.GetHistoryTestBase(lRequest);
            Assert.True(lRequest.GetParameters.ContainsKey("username"));
            Assert.Equal(lRandomUsername, lRequest.GetParameters["username"]);
        }

        [Fact]
        public void GetHistoryUserIdTest()
        {
            int lRandomId = this.GetRandomNumber(200_000);
            IRequestBuilderWithResult<HistoryDataModel[]> lRequest = this.RequestBuilder.GetHistory(
                lRandomId, 4, 132);
            this.GetHistoryTestBase(lRequest);
            Assert.True(lRequest.GetParameters.ContainsKey("uid"));
            Assert.Equal(lRandomId.ToString(), lRequest.GetParameters["uid"]);
        }

        public void GetInfoTestBase(IRequestBuilderBase requestBuilderBase)
        {
            this.CheckUrl(requestBuilderBase, "user", "userinfo");
            Assert.Same(this.ProxerClient, requestBuilderBase.Client);
        }

        [Fact]
        public void GetInfoTest()
        {
            IRequestBuilderWithResult<UserInfoDataModel> lRequest = this.RequestBuilder.GetInfo();
            this.GetInfoTestBase(lRequest);
            Assert.True(lRequest.CheckLogin);
        }

        [Fact]
        public void GetInfoUserIdTest()
        {
            int lRandomId = this.GetRandomNumber(200_000);
            IRequestBuilderWithResult<UserInfoDataModel> lRequest = this.RequestBuilder.GetInfo(lRandomId);
            this.GetInfoTestBase(lRequest);
            Assert.True(lRequest.GetParameters.ContainsKey("uid"));
            Assert.Equal(lRandomId.ToString(), lRequest.GetParameters["uid"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Fact]
        public void GetInfoUsernameTest()
        {
            string lRandomUsername = RandomHelper.GetRandomString(10);
            IRequestBuilderWithResult<UserInfoDataModel> lRequest = this.RequestBuilder.GetInfo(lRandomUsername);
            this.GetInfoTestBase(lRequest);
            Assert.True(lRequest.GetParameters.ContainsKey("username"));
            Assert.Equal(lRandomUsername, lRequest.GetParameters["username"]);
            Assert.False(lRequest.CheckLogin);
        }

        public void GetLatestCommentsTestBase(IRequestBuilderBase requestBuilderBase, MediaEntryType kat)
        {
            this.CheckUrl(requestBuilderBase, "user", "comments");
            Assert.Same(this.ProxerClient, requestBuilderBase.Client);
            Assert.True(requestBuilderBase.GetParameters.ContainsKey("p"));
            Assert.True(requestBuilderBase.GetParameters.ContainsKey("limit"));
            Assert.True(requestBuilderBase.GetParameters.ContainsKey("kat"));
            Assert.True(requestBuilderBase.GetParameters.ContainsKey("length"));
            Assert.Equal("4", requestBuilderBase.GetParameters["p"]);
            Assert.Equal("43", requestBuilderBase.GetParameters["limit"]);
            Assert.Equal(kat.ToString().ToLowerInvariant(), requestBuilderBase.GetParameters["kat"]);
            Assert.Equal("379", requestBuilderBase.GetParameters["length"]);
        }

        [Theory]
        [InlineData(MediaEntryType.Anime)]
        [InlineData(MediaEntryType.Manga)]
        public void GetLatestCommentsUsernameTest(MediaEntryType category)
        {
            string lRandomUsername = RandomHelper.GetRandomString(10);
            IRequestBuilderWithResult<CommentDataModel[]> lRequest =
                this.RequestBuilder.GetLatestComments(lRandomUsername, 4, 43, category, 379);
            this.GetLatestCommentsTestBase(lRequest, category);
            Assert.True(lRequest.GetParameters.ContainsKey("username"));
            Assert.Equal(lRandomUsername, lRequest.GetParameters["username"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Theory]
        [InlineData(MediaEntryType.Anime)]
        [InlineData(MediaEntryType.Manga)]
        public void GetLatestCommentsUserIdTest(MediaEntryType category)
        {
            int lRandomId = this.GetRandomNumber(200_000);
            IRequestBuilderWithResult<CommentDataModel[]> lRequest =
                this.RequestBuilder.GetLatestComments(lRandomId, 4, 43, category, 379);
            this.GetLatestCommentsTestBase(lRequest, category);
            Assert.True(lRequest.GetParameters.ContainsKey("uid"));
            Assert.Equal(lRandomId.ToString(), lRequest.GetParameters["uid"]);
            Assert.False(lRequest.CheckLogin);
        }

        public void GetListTestBase(
            IRequestBuilderBase requestBuilderBase, MediaEntryType category, UserListSort sort,
            SortDirection sortDirection)
        {
            this.CheckUrl(requestBuilderBase, "user", "list");
            Assert.Same(this.ProxerClient, requestBuilderBase.Client);
            Assert.True(requestBuilderBase.GetParameters.ContainsKey("p"));
            Assert.True(requestBuilderBase.GetParameters.ContainsKey("limit"));
            Assert.True(requestBuilderBase.GetParameters.ContainsKey("kat"));
            Assert.True(requestBuilderBase.GetParameters.ContainsKey("search"));
            Assert.True(requestBuilderBase.GetParameters.ContainsKey("search_start"));
            Assert.True(requestBuilderBase.GetParameters.ContainsKey("sort"));
            Assert.Equal("2", requestBuilderBase.GetParameters["p"]);
            Assert.Equal("72", requestBuilderBase.GetParameters["limit"]);
            Assert.Equal(category.ToString().ToLowerInvariant(), requestBuilderBase.GetParameters["kat"]);
            Assert.Equal("test_search", requestBuilderBase.GetParameters["search"]);
            Assert.Equal("test_search_start", requestBuilderBase.GetParameters["search_start"]);
            Assert.Equal(
                sort.GetDescription() + sortDirection.GetDescription(), requestBuilderBase.GetParameters["sort"]);
        }

        [Theory]
        [InlineData(MediaEntryType.Anime, UserListSort.ChangeDate, SortDirection.Ascending)]
        [InlineData(MediaEntryType.Anime, UserListSort.StateChangeDate, SortDirection.Descending)]
        [InlineData(MediaEntryType.Manga, UserListSort.Name, SortDirection.Descending)]
        [InlineData(MediaEntryType.Manga, UserListSort.StateName, SortDirection.Ascending)]
        public void GetListUserIdTest(MediaEntryType category, UserListSort sort, SortDirection sortDirection)
        {
            int lRandomId = this.GetRandomNumber(200_000);
            IRequestBuilderWithResult<ListDataModel[]> lRequest = this.RequestBuilder.GetList(
                lRandomId, category, 2, 72, "test_search", "test_search_start", sort, sortDirection
            );
            this.GetListTestBase(lRequest, category, sort, sortDirection);
            Assert.True(lRequest.GetParameters.ContainsKey("uid"));
            Assert.Equal(lRandomId.ToString(), lRequest.GetParameters["uid"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Theory]
        [InlineData(MediaEntryType.Anime, UserListSort.ChangeDate, SortDirection.Ascending)]
        [InlineData(MediaEntryType.Anime, UserListSort.StateChangeDate, SortDirection.Descending)]
        [InlineData(MediaEntryType.Manga, UserListSort.Name, SortDirection.Descending)]
        [InlineData(MediaEntryType.Manga, UserListSort.StateName, SortDirection.Ascending)]
        public void GetListUsernameTest(MediaEntryType category, UserListSort sort, SortDirection sortDirection)
        {
            string lRandomUsername = RandomHelper.GetRandomString(10);
            IRequestBuilderWithResult<ListDataModel[]> lRequest = this.RequestBuilder.GetList(
                lRandomUsername, category, 2, 72, "test_search", "test_search_start", sort, sortDirection
            );
            this.GetListTestBase(lRequest, category, sort, sortDirection);
            Assert.True(lRequest.GetParameters.ContainsKey("username"));
            Assert.Equal(lRandomUsername, lRequest.GetParameters["username"]);
            Assert.False(lRequest.CheckLogin);
        }

        public void GetToptenTestBase(IRequestBuilderBase requestBuilderBase, MediaEntryType category)
        {
            this.CheckUrl(requestBuilderBase, "user", "topten");
            Assert.Same(this.ProxerClient, requestBuilderBase.Client);
            Assert.True(requestBuilderBase.GetParameters.ContainsKey("kat"));
            Assert.Equal(category.ToString().ToLowerInvariant(), requestBuilderBase.GetParameters["kat"]);
        }

        [Theory]
        [InlineData(MediaEntryType.Anime)]
        [InlineData(MediaEntryType.Manga)]
        public void GetToptenUserIdTest(MediaEntryType category)
        {
            int lRandomId = this.GetRandomNumber(200_000);
            IRequestBuilderWithResult<ToptenDataModel[]> lRequest = this.RequestBuilder.GetTopten(lRandomId, category);
            this.GetToptenTestBase(lRequest, category);
            Assert.True(lRequest.GetParameters.ContainsKey("uid"));
            Assert.Equal(lRandomId.ToString(), lRequest.GetParameters["uid"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Theory]
        [InlineData(MediaEntryType.Anime)]
        [InlineData(MediaEntryType.Manga)]
        public void GetToptenUsernameTest(MediaEntryType category)
        {
            string lRandomUsername = RandomHelper.GetRandomString(10);
            IRequestBuilderWithResult<ToptenDataModel[]> lRequest = this.RequestBuilder.GetTopten(
                lRandomUsername, category);
            this.GetToptenTestBase(lRequest, category);
            Assert.True(lRequest.GetParameters.ContainsKey("username"));
            Assert.Equal(lRandomUsername, lRequest.GetParameters["username"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Fact]
        public void LoginTest()
        {
            string lRandomUsername = RandomHelper.GetRandomString(10);
            string lRandomPassword = RandomHelper.GetRandomString(20);
            IRequestBuilderWithResult<LoginDataModel> lRequest =
                this.RequestBuilder.Login(lRandomUsername, lRandomPassword);
            this.CheckUrl(lRequest, "user", "login");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("username"));
            Assert.True(lRequest.PostParameter.ContainsKey("password"));
            Assert.Equal(lRandomUsername, lRequest.PostParameter.GetValue("username").First());
            Assert.Equal(lRandomPassword, lRequest.PostParameter.GetValue("password").First());
            Assert.False(lRequest.CheckLogin);
        }

        [Fact]
        public void LoginSecretKeyTest()
        {
            string lRandomUsername = RandomHelper.GetRandomString(10);
            string lRandomPassword = RandomHelper.GetRandomString(20);
            string lRandomSecretKey = RandomHelper.GetRandomString(6);
            IRequestBuilderWithResult<LoginDataModel> lRequest =
                this.RequestBuilder.Login(lRandomUsername, lRandomPassword, lRandomSecretKey);
            this.CheckUrl(lRequest, "user", "login");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("username"));
            Assert.True(lRequest.PostParameter.ContainsKey("password"));
            Assert.True(lRequest.PostParameter.ContainsKey("secretKey"));
            Assert.Equal(lRandomUsername, lRequest.PostParameter.GetValue("username").First());
            Assert.Equal(lRandomPassword, lRequest.PostParameter.GetValue("password").First());
            Assert.Equal(lRandomSecretKey, lRequest.PostParameter.GetValue("secretKey").First());
            Assert.False(lRequest.CheckLogin);
        }

        [Fact]
        public void LogoutTest()
        {
            IRequestBuilder lRequest = this.RequestBuilder.Logout();
            this.CheckUrl(lRequest, "user", "logout");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.False(lRequest.CheckLogin);
        }

        [Fact]
        public override void ProxerClientTest()
        {
            base.ProxerClientTest();
        }
    }
}