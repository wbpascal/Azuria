using System.Linq;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Enums.Info;
using Azuria.Helpers.Extensions;
using Azuria.Requests.Builder;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.RequestBuilder
{
    [TestFixture]
    public class InfoRequestBuilderTest : RequestBuilderTestBase<InfoRequestBuilder>
    {
        [Test]
        public void GetCommentsTest([Values] CommentSort sort)
        {
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilderWithResult<CommentDataModel[]> lRequest =
                this.RequestBuilder.GetComments(lRandomId, 2, 31, sort);
            this.CheckUrl(lRequest, "info", "comments");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            Assert.True(lRequest.GetParameters.ContainsKey("sort"));
            Assert.AreEqual(lRandomId.ToString(), lRequest.GetParameters["id"]);
            Assert.AreEqual("2", lRequest.GetParameters["p"]);
            Assert.AreEqual("31", lRequest.GetParameters["limit"]);
            Assert.AreEqual(sort.ToString().ToLowerInvariant(), lRequest.GetParameters["sort"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetEntryTagTest()
        {
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilderWithResult<TagDataModel[]> lRequest = this.RequestBuilder.GetEntryTags(lRandomId);
            this.CheckUrl(lRequest, "info", "entrytags");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.AreEqual(lRandomId.ToString(), lRequest.GetParameters["id"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetEntryTest()
        {
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilderWithResult<EntryDataModel> lRequest = this.RequestBuilder.GetEntry(lRandomId);
            this.CheckUrl(lRequest, "info", "entry");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.AreEqual(lRandomId.ToString(), lRequest.GetParameters["id"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetFullEntry()
        {
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilderWithResult<FullEntryDataModel> lRequest = this.RequestBuilder.GetFullEntry(lRandomId);
            this.CheckUrl(lRequest, "info", "fullentry");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.AreEqual(lRandomId.ToString(), lRequest.GetParameters["id"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetGateTest()
        {
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilderWithResult<bool> lRequest = this.RequestBuilder.GetGate(lRandomId);
            this.CheckUrl(lRequest, "info", "gate");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.AreEqual(lRandomId.ToString(), lRequest.GetParameters["id"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetGroupsTest()
        {
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilderWithResult<TranslatorDataModel[]> lRequest = this.RequestBuilder.GetGroups(lRandomId);
            this.CheckUrl(lRequest, "info", "groups");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.AreEqual(lRandomId.ToString(), lRequest.GetParameters["id"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetIndustryTest()
        {
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilderWithResult<IndustryDataModel> lRequest = this.RequestBuilder.GetIndustry(lRandomId);
            this.CheckUrl(lRequest, "info", "industry");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.AreEqual(lRandomId.ToString(), lRequest.GetParameters["id"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetLanguageTest()
        {
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilderWithResult<MediaLanguage[]> lRequest = this.RequestBuilder.GetLanguage(lRandomId);
            this.CheckUrl(lRequest, "info", "lang");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.AreEqual(lRandomId.ToString(), lRequest.GetParameters["id"]);
            Assert.NotNull(lRequest.CustomDataConverter);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetListInfoTest()
        {
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilderWithResult<ListInfoDataModel> lRequest = this.RequestBuilder.GetListInfo(lRandomId, 1, 15);
            this.CheckUrl(lRequest, "info", "listinfo");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            Assert.AreEqual(lRandomId.ToString(), lRequest.GetParameters["id"]);
            Assert.AreEqual("1", lRequest.GetParameters["p"]);
            Assert.AreEqual("15", lRequest.GetParameters["limit"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetNameTest()
        {
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilderWithResult<NameDataModel[]> lRequest = this.RequestBuilder.GetNames(lRandomId);
            this.CheckUrl(lRequest, "info", "names");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.AreEqual(lRandomId.ToString(), lRequest.GetParameters["id"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetPublisherTest()
        {
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilderWithResult<IndustryBasicDataModel[]> lRequest = this.RequestBuilder.GetPublisher(lRandomId);
            this.CheckUrl(lRequest, "info", "publisher");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.AreEqual(lRandomId.ToString(), lRequest.GetParameters["id"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetRelationsTest()
        {
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilderWithResult<RelationDataModel[]> lRequest = this.RequestBuilder.GetRelations(lRandomId);
            this.CheckUrl(lRequest, "info", "relations");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.AreEqual(lRandomId.ToString(), lRequest.GetParameters["id"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetSeasonsTest()
        {
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilderWithResult<SeasonDataModel[]> lRequest = this.RequestBuilder.GetSeason(lRandomId);
            this.CheckUrl(lRequest, "info", "season");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.AreEqual(lRandomId.ToString(), lRequest.GetParameters["id"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetTranslatorGroupTest()
        {
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilderWithResult<TranslatorDataModel> lRequest = this.RequestBuilder.GetTranslatorGroup(lRandomId);
            this.CheckUrl(lRequest, "info", "translatorgroup");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.AreEqual(lRandomId.ToString(), lRequest.GetParameters["id"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public override void ProxerClientTest()
        {
            base.ProxerClientTest();
        }

        [Test]
        public void SetUserInfoTest([Values] UserList list)
        {
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilder lRequest = this.RequestBuilder.SetUserInfo(lRandomId, list);
            this.CheckUrl(lRequest, "info", "setuserinfo");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.Any(pair => pair.Key == "id" && pair.Value == lRandomId.ToString()));
            Assert.True(lRequest.PostParameter.Any(pair => pair.Key == "type" && pair.Value == list.ToTypeString()));
            Assert.True(lRequest.CheckLogin);
        }
    }
}