using System;
using System.Linq;
using Azuria.Api.v1.DataModels.User;
using Azuria.Api.v1.Input.User;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Enums;
using Azuria.Enums.User;
using Azuria.Helpers.Extensions;
using Azuria.Requests.Builder;
using Azuria.Test.Core.Helpers;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.RequestBuilder
{
    [TestFixture]
    public class UserRequestBuilderTest : RequestBuilderTestBase<UserRequestBuilder>
    {
        public UserRequestBuilderTest() : base(client => new UserRequestBuilder(client))
        {
        }

        private void GetHistoryTestBase(IRequestBuilderBase requestBuilderBase)
        {
            this.CheckUrl(requestBuilderBase, "user", "history");
            Assert.AreSame(this.ProxerClient, requestBuilderBase.Client);
            Assert.True(requestBuilderBase.GetParameters.ContainsKey("p"));
            Assert.True(requestBuilderBase.GetParameters.ContainsKey("limit"));
            Assert.AreEqual("4", requestBuilderBase.GetParameters["p"]);
            Assert.AreEqual("132", requestBuilderBase.GetParameters["limit"]);
            Assert.False(requestBuilderBase.CheckLogin);
        }

        [Test]
        public void GetHistoryUsernameTest()
        {
            UserEntryHistoryInput lInput = new UserEntryHistoryInput
            {
                Username = RandomHelper.GetRandomString(10),
                Limit = 132,
                Page = 4
            };
            IRequestBuilderWithResult<HistoryDataModel[]> lRequest = this.RequestBuilder.GetHistory(lInput);
            this.GetHistoryTestBase(lRequest);
            Assert.True(lRequest.GetParameters.ContainsKey("username"));
            Assert.AreEqual(lInput.Username, lRequest.GetParameters["username"]);
        }

        [Test]
        public void GetHistoryUserIdTest()
        {
            UserEntryHistoryInput lInput = new UserEntryHistoryInput
            {
                UserId = this.GetRandomNumber(500_000),
                Limit = 132,
                Page = 4
            };
            IRequestBuilderWithResult<HistoryDataModel[]> lRequest = this.RequestBuilder.GetHistory(lInput);
            this.GetHistoryTestBase(lRequest);
            Assert.True(lRequest.GetParameters.ContainsKey("uid"));
            Assert.AreEqual(lInput.UserId.ToString(), lRequest.GetParameters["uid"]);
        }

        private void GetInfoTestBase(IRequestBuilderBase requestBuilderBase)
        {
            this.CheckUrl(requestBuilderBase, "user", "userinfo");
            Assert.AreSame(this.ProxerClient, requestBuilderBase.Client);
        }

        [Test]
        public void GetInfoTest()
        {
            IRequestBuilderWithResult<UserInfoDataModel> lRequest = this.RequestBuilder.GetInfo(new UserInfoInput());
            this.GetInfoTestBase(lRequest);
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void GetInfoUserIdTest()
        {
            UserInfoInput lInput = new UserInfoInput {UserId = this.GetRandomNumber(500_000)};
            IRequestBuilderWithResult<UserInfoDataModel> lRequest = this.RequestBuilder.GetInfo(lInput);
            this.GetInfoTestBase(lRequest);
            Assert.True(lRequest.GetParameters.ContainsKey("uid"));
            Assert.AreEqual(lInput.UserId.ToString(), lRequest.GetParameters["uid"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetInfoUsernameTest()
        {
            UserInfoInput lInput = new UserInfoInput {Username = RandomHelper.GetRandomString(10)};
            IRequestBuilderWithResult<UserInfoDataModel> lRequest = this.RequestBuilder.GetInfo(lInput);
            this.GetInfoTestBase(lRequest);
            Assert.True(lRequest.GetParameters.ContainsKey("username"));
            Assert.AreEqual(lInput.Username, lRequest.GetParameters["username"]);
            Assert.False(lRequest.CheckLogin);
        }

        private void GetLatestCommentsTestBase(IRequestBuilderBase requestBuilderBase, MediaEntryType kat)
        {
            this.CheckUrl(requestBuilderBase, "user", "comments");
            Assert.AreSame(this.ProxerClient, requestBuilderBase.Client);
            Assert.True(requestBuilderBase.GetParameters.ContainsKey("p"));
            Assert.True(requestBuilderBase.GetParameters.ContainsKey("limit"));
            Assert.True(requestBuilderBase.GetParameters.ContainsKey("kat"));
            Assert.True(requestBuilderBase.GetParameters.ContainsKey("length"));
            Assert.AreEqual("4", requestBuilderBase.GetParameters["p"]);
            Assert.AreEqual("43", requestBuilderBase.GetParameters["limit"]);
            Assert.AreEqual(kat.ToString().ToLowerInvariant(), requestBuilderBase.GetParameters["kat"]);
            Assert.AreEqual("379", requestBuilderBase.GetParameters["length"]);
        }

        [Test]
        public void GetLatestCommentsUsernameTest([Values] MediaEntryType category)
        {
            UserCommentsListInput lInput = new UserCommentsListInput
            {
                Username = RandomHelper.GetRandomString(10),
                Category = category,
                Page = 4,
                Limit = 43,
                Length = 379
            };
            IRequestBuilderWithResult<CommentDataModel[]> lRequest = this.RequestBuilder.GetLatestComments(lInput);
            this.GetLatestCommentsTestBase(lRequest, category);
            Assert.True(lRequest.GetParameters.ContainsKey("username"));
            Assert.AreEqual(lInput.Username, lRequest.GetParameters["username"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetLatestCommentsUserIdTest([Values] MediaEntryType category)
        {
            UserCommentsListInput lInput = new UserCommentsListInput
            {
                UserId = this.GetRandomNumber(500_000),
                Category = category,
                Page = 4,
                Limit = 43,
                Length = 379
            };
            IRequestBuilderWithResult<CommentDataModel[]> lRequest = this.RequestBuilder.GetLatestComments(lInput);
            this.GetLatestCommentsTestBase(lRequest, category);
            Assert.True(lRequest.GetParameters.ContainsKey("uid"));
            Assert.AreEqual(lInput.UserId.ToString(), lRequest.GetParameters["uid"]);
            Assert.False(lRequest.CheckLogin);
        }

        private void GetListTestBase(
            IRequestBuilderBase requestBuilderBase, MediaEntryType category, UserListSort sort,
            SortDirection sortDirection)
        {
            this.CheckUrl(requestBuilderBase, "user", "list");
            Assert.AreSame(this.ProxerClient, requestBuilderBase.Client);
            Assert.True(requestBuilderBase.GetParameters.ContainsKey("p"));
            Assert.True(requestBuilderBase.GetParameters.ContainsKey("limit"));
            Assert.True(requestBuilderBase.GetParameters.ContainsKey("kat"));
            Assert.True(requestBuilderBase.GetParameters.ContainsKey("search"));
            Assert.True(requestBuilderBase.GetParameters.ContainsKey("search_start"));
            Assert.True(requestBuilderBase.GetParameters.ContainsKey("sort"));
            Assert.AreEqual("2", requestBuilderBase.GetParameters["p"]);
            Assert.AreEqual("72", requestBuilderBase.GetParameters["limit"]);
            Assert.AreEqual(category.ToString().ToLowerInvariant(), requestBuilderBase.GetParameters["kat"]);
            Assert.AreEqual("test_search", requestBuilderBase.GetParameters["search"]);
            Assert.AreEqual("test_search_start", requestBuilderBase.GetParameters["search_start"]);
            Assert.AreEqual(
                sort.GetDescription() + sortDirection.GetDescription(), requestBuilderBase.GetParameters["sort"]
            );
        }

        [Test, Pairwise]
        public void GetListUserIdTest(
            [Values] MediaEntryType category, [Values] UserListSort sort, [Values] SortDirection sortDirection)
        {
            UserGetListInput lInputDataModel = new UserGetListInput
            {
                Category = category,
                Search = "test_search",
                SearchStart = "test_search_start",
                Sort = sort,
                SortDirection = sortDirection,
                UserId = this.GetRandomNumber(200_000),
                Limit = 72,
                Page = 2
            };

            IRequestBuilderWithResult<ListDataModel[]> lRequest = this.RequestBuilder.GetList(lInputDataModel);
            this.GetListTestBase(lRequest, category, sort, sortDirection);
            Assert.True(lRequest.GetParameters.ContainsKey("uid"));
            Assert.AreEqual(lInputDataModel.UserId.ToString(), lRequest.GetParameters["uid"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test, Pairwise]
        public void GetListUsernameTest(
            [Values] MediaEntryType category, [Values] UserListSort sort, [Values] SortDirection sortDirection)
        {
            UserGetListInput lInputDataModel = new UserGetListInput
            {
                Category = category,
                Search = "test_search",
                SearchStart = "test_search_start",
                Sort = sort,
                SortDirection = sortDirection,
                Username = RandomHelper.GetRandomString(10),
                Limit = 72,
                Page = 2
            };

            IRequestBuilderWithResult<ListDataModel[]> lRequest = this.RequestBuilder.GetList(lInputDataModel);
            this.GetListTestBase(lRequest, category, sort, sortDirection);
            Assert.True(lRequest.GetParameters.ContainsKey("username"));
            Assert.AreEqual(lInputDataModel.Username, lRequest.GetParameters["username"]);
            Assert.False(lRequest.CheckLogin);
        }

        private void GetToptenTestBase(IRequestBuilderBase requestBuilderBase, MediaEntryType category)
        {
            this.CheckUrl(requestBuilderBase, "user", "topten");
            Assert.AreSame(this.ProxerClient, requestBuilderBase.Client);
            Assert.True(requestBuilderBase.GetParameters.ContainsKey("kat"));
            Assert.AreEqual(category.ToString().ToLowerInvariant(), requestBuilderBase.GetParameters["kat"]);
        }

        [Test]
        public void GetToptenUserIdTest([Values] MediaEntryType category)
        {
            UserToptenListInput lInput = new UserToptenListInput
            {
                Category = category,
                UserId = this.GetRandomNumber(500_000)
            };
            IRequestBuilderWithResult<ToptenDataModel[]> lRequest = this.RequestBuilder.GetTopten(lInput);
            this.GetToptenTestBase(lRequest, category);
            Assert.True(lRequest.GetParameters.ContainsKey("uid"));
            Assert.AreEqual(lInput.UserId.ToString(), lRequest.GetParameters["uid"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetToptenUsernameTest([Values] MediaEntryType category)
        {
            UserToptenListInput lInput = new UserToptenListInput
            {
                Category = category,
                Username = RandomHelper.GetRandomString(10)
            };
            IRequestBuilderWithResult<ToptenDataModel[]> lRequest = this.RequestBuilder.GetTopten(lInput);
            this.GetToptenTestBase(lRequest, category);
            Assert.True(lRequest.GetParameters.ContainsKey("username"));
            Assert.AreEqual(lInput.Username, lRequest.GetParameters["username"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void LoginTest()
        {
            LoginInput lInput = new LoginInput
            {
                Username = RandomHelper.GetRandomString(10),
                Password = RandomHelper.GetRandomString(16)
            };
            IRequestBuilderWithResult<LoginDataModel> lRequest = this.RequestBuilder.Login(lInput);
            this.CheckUrl(lRequest, "user", "login");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("username"));
            Assert.True(lRequest.PostParameter.ContainsKey("password"));
            Assert.AreEqual(lInput.Username, lRequest.PostParameter.GetValue("username").First());
            Assert.AreEqual(lInput.Password, lRequest.PostParameter.GetValue("password").First());
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void LoginSecretKeyTest()
        {
            LoginInput lInput = new LoginInput
            {
                Username = RandomHelper.GetRandomString(10),
                Password = RandomHelper.GetRandomString(16),
                SecretKey = this.GetRandomNumber(6).ToString()
            };
            IRequestBuilderWithResult<LoginDataModel> lRequest = this.RequestBuilder.Login(lInput);
            this.CheckUrl(lRequest, "user", "login");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("username"));
            Assert.True(lRequest.PostParameter.ContainsKey("password"));
            Assert.True(lRequest.PostParameter.ContainsKey("secretKey"));
            Assert.AreEqual(lInput.Username, lRequest.PostParameter.GetValue("username").First());
            Assert.AreEqual(lInput.Password, lRequest.PostParameter.GetValue("password").First());
            Assert.AreEqual(lInput.SecretKey, lRequest.PostParameter.GetValue("secretKey").First());
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void LogoutTest()
        {
            IRequestBuilder lRequest = this.RequestBuilder.Logout();
            this.CheckUrl(lRequest, "user", "logout");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public override void ProxerClientTest()
        {
            base.ProxerClientTest();
        }
    }
}