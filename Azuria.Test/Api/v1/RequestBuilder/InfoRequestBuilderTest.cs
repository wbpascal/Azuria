using System.Linq;
using Azuria.Api.v1.DataModels;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Enums.Info;
using Azuria.Helpers.Extensions;
using Azuria.Requests.Builder;
using Xunit;

namespace Azuria.Test.Api.v1.RequestBuilder
{
    public class InfoRequestBuilderTest : RequestBuilderTestBase<InfoRequestBuilder>
    {
        [Theory]
        [InlineData(CommentSort.Newest)]
        [InlineData(CommentSort.Rating)]
        public void GetCommentsTest(CommentSort sort)
        {
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilderWithResult<CommentDataModel[]> lRequest =
                this.RequestBuilder.GetComments(lRandomId, 2, 31, sort);
            this.CheckUrl(lRequest, "info", "comments");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            Assert.True(lRequest.GetParameters.ContainsKey("sort"));
            Assert.Equal(lRandomId.ToString(), lRequest.GetParameters["id"]);
            Assert.Equal("2", lRequest.GetParameters["p"]);
            Assert.Equal("31", lRequest.GetParameters["limit"]);
            Assert.Equal(sort.ToString().ToLowerInvariant(), lRequest.GetParameters["sort"]);
        }

        [Fact]
        public void GetEntryTagTest()
        {
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilderWithResult<MediaTagDataModel[]> lRequest = this.RequestBuilder.GetEntryTags(lRandomId);
            this.CheckUrl(lRequest, "info", "entrytags");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.Equal(lRandomId.ToString(), lRequest.GetParameters["id"]);
        }

        [Fact]
        public void GetEntryTest()
        {
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilderWithResult<EntryDataModel> lRequest = this.RequestBuilder.GetEntry(lRandomId);
            this.CheckUrl(lRequest, "info", "entry");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.Equal(lRandomId.ToString(), lRequest.GetParameters["id"]);
        }

        [Fact]
        public void GetFullEntry()
        {
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilderWithResult<FullEntryDataModel> lRequest = this.RequestBuilder.GetFullEntry(lRandomId);
            this.CheckUrl(lRequest, "info", "fullentry");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.Equal(lRandomId.ToString(), lRequest.GetParameters["id"]);
        }

        [Fact]
        public void GetGateTest()
        {
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilderWithResult<bool> lRequest = this.RequestBuilder.GetGate(lRandomId);
            this.CheckUrl(lRequest, "info", "gate");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.Equal(lRandomId.ToString(), lRequest.GetParameters["id"]);
        }

        [Fact]
        public void GetGroupsTest()
        {
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilderWithResult<TranslatorDataModel[]> lRequest = this.RequestBuilder.GetGroups(lRandomId);
            this.CheckUrl(lRequest, "info", "groups");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.Equal(lRandomId.ToString(), lRequest.GetParameters["id"]);
        }

        [Fact]
        public void GetIndustryTest()
        {
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilderWithResult<TranslatorDataModel[]> lRequest = this.RequestBuilder.GetIndustry(lRandomId);
            this.CheckUrl(lRequest, "info", "industry");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.Equal(lRandomId.ToString(), lRequest.GetParameters["id"]);
        }

        [Fact]
        public void GetLanguageTest()
        {
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilderWithResult<MediaLanguage[]> lRequest = this.RequestBuilder.GetLanguage(lRandomId);
            this.CheckUrl(lRequest, "info", "lang");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.Equal(lRandomId.ToString(), lRequest.GetParameters["id"]);
            Assert.NotNull(lRequest.CustomDataConverter);
        }

        [Fact]
        public void GetListInfoTest()
        {
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilderWithResult<ListInfoDataModel> lRequest = this.RequestBuilder.GetListInfo(lRandomId, 1, 15);
            this.CheckUrl(lRequest, "info", "listinfo");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            Assert.Equal(lRandomId.ToString(), lRequest.GetParameters["id"]);
            Assert.Equal("1", lRequest.GetParameters["p"]);
            Assert.Equal("15", lRequest.GetParameters["limit"]);
        }

        [Fact]
        public void GetNameTest()
        {
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilderWithResult<NameDataModel[]> lRequest = this.RequestBuilder.GetName(lRandomId);
            this.CheckUrl(lRequest, "info", "names");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.Equal(lRandomId.ToString(), lRequest.GetParameters["id"]);
        }

        [Fact]
        public void GetPublisherTest()
        {
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilderWithResult<PublisherDataModel[]> lRequest = this.RequestBuilder.GetPublisher(lRandomId);
            this.CheckUrl(lRequest, "info", "publisher");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.Equal(lRandomId.ToString(), lRequest.GetParameters["id"]);
        }

        [Fact]
        public void GetRelationsTest()
        {
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilderWithResult<RelationDataModel[]> lRequest = this.RequestBuilder.GetRelations(lRandomId);
            this.CheckUrl(lRequest, "info", "relations");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.Equal(lRandomId.ToString(), lRequest.GetParameters["id"]);
        }

        [Fact]
        public void GetSeasonsTest()
        {
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilderWithResult<SeasonDataModel[]> lRequest = this.RequestBuilder.GetSeason(lRandomId);
            this.CheckUrl(lRequest, "info", "season");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.Equal(lRandomId.ToString(), lRequest.GetParameters["id"]);
        }

        [Fact]
        public void GetTranslatorGroupTest()
        {
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilderWithResult<TranslatorDataModel[]> lRequest =
                this.RequestBuilder.GetTranslatorGroup(lRandomId);
            this.CheckUrl(lRequest, "info", "translatorgroup");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.Equal(lRandomId.ToString(), lRequest.GetParameters["id"]);
        }

        [Fact]
        public override void ProxerClientTest()
        {
            base.ProxerClientTest();
        }

        [Theory]
        [InlineData(UserList.Favourites)]
        [InlineData(UserList.Finished)]
        [InlineData(UserList.Note)]
        public void SetUserInfoTest(UserList list)
        {
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilder lRequest = this.RequestBuilder.SetUserInfo(lRandomId, list);
            this.CheckUrl(lRequest, "info", "setuserinfo");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostArguments.Any(pair => pair.Key == "id" && pair.Value == lRandomId.ToString()));
            Assert.True(lRequest.PostArguments.Any(pair => pair.Key == "type" && pair.Value == list.ToTypeString()));
            Assert.True(lRequest.CheckLogin);
        }
    }
}