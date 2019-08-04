using System.Linq;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Api.v1.Input;
using Azuria.Api.v1.Input.Info;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Enums.Info;
using Azuria.Helpers.Extensions;
using Azuria.Requests.Builder;
using Azuria.Test.Core.Helpers;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.RequestBuilder
{
    [TestFixture]
    public class InfoRequestBuilderTest : RequestBuilderTestBase<InfoRequestBuilder>
    {
        public InfoRequestBuilderTest() : base(client => new InfoRequestBuilder(client))
        {
        }

        [Test]
        public void GetCharacterInfoTest()
        {
            SimpleIdInput lInput = new SimpleIdInput {Id = this.GetRandomNumber(10000)};

            IRequestBuilderWithResult<CharacterInfoDataModel> lRequest = this.RequestBuilder.GetCharacterInfo(lInput);
            this.CheckUrl(lRequest, "info", "character");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("id"));
            Assert.Contains(lInput.Id.ToString(), lRequest.PostParameter.GetValue("id").ToList());
        }

        [Test]
        public void GetCharactersTest()
        {
            EntryIdInput lInput = this.GetRandomEntryIdInput();

            IRequestBuilderWithResult<CharacterDataModel[]> lRequest = this.RequestBuilder.GetCharacters(lInput);
            this.CheckUrl(lRequest, "info", "characters");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("id"));
            Assert.AreEqual(lInput.EntryId.ToString(), lRequest.PostParameter.GetValue("id").First());
        }

        [Test]
        public void GetCommentsTest([Values] CommentSort sort)
        {
            CommentListInput lInput = new CommentListInput
            {
                Id = this.GetRandomNumber(4200), Limit = 31, Page = 2, Sort = sort
            };

            IRequestBuilderWithResult<CommentDataModel[]> lRequest =
                this.RequestBuilder.GetComments(lInput);
            this.CheckUrl(lRequest, "info", "comments");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            Assert.True(lRequest.GetParameters.ContainsKey("sort"));
            Assert.AreEqual(lInput.Id.ToString(), lRequest.GetParameters["id"]);
            Assert.AreEqual("2", lRequest.GetParameters["p"]);
            Assert.AreEqual("31", lRequest.GetParameters["limit"]);
            Assert.AreEqual(sort.ToString().ToLowerInvariant(), lRequest.GetParameters["sort"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetEntryTagTest()
        {
            EntryIdInput lInput = this.GetRandomEntryIdInput();
            IRequestBuilderWithResult<TagDataModel[]> lRequest = this.RequestBuilder.GetEntryTags(lInput);
            this.CheckUrl(lRequest, "info", "entrytags");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.AreEqual(lInput.EntryId.ToString(), lRequest.GetParameters["id"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetEntryTest()
        {
            EntryIdInput lInput = this.GetRandomEntryIdInput();
            IRequestBuilderWithResult<EntryDataModel> lRequest = this.RequestBuilder.GetEntry(lInput);
            this.CheckUrl(lRequest, "info", "entry");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.AreEqual(lInput.EntryId.ToString(), lRequest.GetParameters["id"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetForumTest()
        {
            EntryIdInput lInput = this.GetRandomEntryIdInput();
            IRequestBuilderWithResult<ForumDataModel[]> lRequest = this.RequestBuilder.GetForum(lInput);
            this.CheckUrl(lRequest, "info", "forum");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("id"));
            Assert.AreEqual(lInput.EntryId.ToString(), lRequest.PostParameter.GetValue("id").First());
        }

        [Test]
        public void GetFullEntry()
        {
            EntryIdInput lInput = this.GetRandomEntryIdInput();
            IRequestBuilderWithResult<FullEntryDataModel> lRequest = this.RequestBuilder.GetFullEntry(lInput);
            this.CheckUrl(lRequest, "info", "fullentry");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.AreEqual(lInput.EntryId.ToString(), lRequest.GetParameters["id"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetGateTest()
        {
            EntryIdInput lInput = this.GetRandomEntryIdInput();
            IRequestBuilderWithResult<bool> lRequest = this.RequestBuilder.GetGate(lInput);
            this.CheckUrl(lRequest, "info", "gate");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.AreEqual(lInput.EntryId.ToString(), lRequest.GetParameters["id"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetGroupsTest()
        {
            EntryIdInput lInput = this.GetRandomEntryIdInput();
            IRequestBuilderWithResult<TranslatorBasicDataModel[]> lRequest = this.RequestBuilder.GetGroups(lInput);
            this.CheckUrl(lRequest, "info", "groups");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.AreEqual(lInput.EntryId.ToString(), lRequest.GetParameters["id"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetIndustryTest()
        {
            IndustryInfoInput lInput = new IndustryInfoInput {IndustryId = this.GetRandomNumber(1000)};
            IRequestBuilderWithResult<IndustryDataModel> lRequest = this.RequestBuilder.GetIndustry(lInput);
            this.CheckUrl(lRequest, "info", "industry");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.AreEqual(lInput.IndustryId.ToString(), lRequest.GetParameters["id"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetLanguageTest()
        {
            EntryIdInput lInput = this.GetRandomEntryIdInput();
            IRequestBuilderWithResult<MediaLanguage[]> lRequest = this.RequestBuilder.GetLanguage(lInput);
            this.CheckUrl(lRequest, "info", "lang");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.AreEqual(lInput.EntryId.ToString(), lRequest.GetParameters["id"]);
            Assert.NotNull(lRequest.CustomDataConverter);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetListInfoTest()
        {
            ListInfoInput lInput = new ListInfoInput {EntryId = this.GetRandomNumber(10000), Limit = 15, Page = 1};
            IRequestBuilderWithResult<ListInfoDataModel> lRequest = this.RequestBuilder.GetListInfo(lInput);
            this.CheckUrl(lRequest, "info", "listinfo");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            Assert.AreEqual(lInput.EntryId.ToString(), lRequest.GetParameters["id"]);
            Assert.AreEqual("1", lRequest.GetParameters["p"]);
            Assert.AreEqual("15", lRequest.GetParameters["limit"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetNameTest()
        {
            EntryIdInput lInput = this.GetRandomEntryIdInput();
            IRequestBuilderWithResult<EntryNameDataModel[]> lRequest = this.RequestBuilder.GetNames(lInput);
            this.CheckUrl(lRequest, "info", "names");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.AreEqual(lInput.EntryId.ToString(), lRequest.GetParameters["id"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetPersonInfoTest()
        {
            SimpleIdInput lInput = new SimpleIdInput {Id = this.GetRandomNumber(10000)};

            IRequestBuilderWithResult<PersonInfoDataModel> lRequest = this.RequestBuilder.GetPersonInfo(lInput);
            this.CheckUrl(lRequest, "info", "person");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("id"));
            Assert.AreEqual(lInput.Id.ToString(), lRequest.PostParameter.GetValue("id").First());
        }

        [Test]
        public void GetPersonsTest()
        {
            EntryIdInput lInput = this.GetRandomEntryIdInput();
            IRequestBuilderWithResult<PersonDataModel[]> lRequest = this.RequestBuilder.GetPersons(lInput);
            this.CheckUrl(lRequest, "info", "persons");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("id"));
            Assert.AreEqual(lInput.EntryId.ToString(), lRequest.PostParameter.GetValue("id").First());
        }

        [Test]
        public void GetPublisherTest()
        {
            EntryIdInput lInput = this.GetRandomEntryIdInput();
            IRequestBuilderWithResult<IndustryBasicDataModel[]> lRequest = this.RequestBuilder.GetPublisher(lInput);
            this.CheckUrl(lRequest, "info", "publisher");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.AreEqual(lInput.EntryId.ToString(), lRequest.GetParameters["id"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetRelationsTest()
        {
            RelationsInput lInput = new RelationsInput {EntryId = this.GetRandomNumber(10000), ContainsH = true};
            IRequestBuilderWithResult<RelationDataModel[]> lRequest = this.RequestBuilder.GetRelations(lInput);
            this.CheckUrl(lRequest, "info", "relations");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.True(lRequest.GetParameters.ContainsKey("isH"));
            Assert.AreEqual(lInput.EntryId.ToString(), lRequest.GetParameters["id"]);
            Assert.AreEqual("true", lRequest.GetParameters["isH"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetRecommendationsTest()
        {
            EntryIdInput lInput = this.GetRandomEntryIdInput();
            IRequestBuilderWithResult<RecommendationDataModel[]> lRequest =
                this.RequestBuilder.GetRecommendations(lInput);
            this.CheckUrl(lRequest, "info", "recommendations");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.AreEqual(lInput.EntryId.ToString(), lRequest.GetParameters["id"]);
        }

        [Test]
        public void GetSeasonsTest()
        {
            EntryIdInput lInput = this.GetRandomEntryIdInput();
            IRequestBuilderWithResult<SeasonDataModel[]> lRequest = this.RequestBuilder.GetSeason(lInput);
            this.CheckUrl(lRequest, "info", "season");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.AreEqual(lInput.EntryId.ToString(), lRequest.GetParameters["id"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetTranslatorGroupTest()
        {
            TranslatorInfoInput lInput = new TranslatorInfoInput {TranslatorId = this.GetRandomNumber(1000)};
            IRequestBuilderWithResult<TranslatorDataModel> lRequest = this.RequestBuilder.GetTranslatorGroup(lInput);
            this.CheckUrl(lRequest, "info", "translatorgroup");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.AreEqual(lInput.TranslatorId.ToString(), lRequest.GetParameters["id"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetUserInfoTest()
        {
            EntryIdInput lInput = this.GetRandomEntryIdInput();
            IRequestBuilderWithResult<UserListsDataModel> lRequest = this.RequestBuilder.GetUserInfo(lInput);
            this.CheckUrl(lRequest, "info", "userinfo");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.CheckLogin);
            Assert.True(lRequest.PostParameter.ContainsKey("id"));
            Assert.AreEqual(lInput.EntryId.ToString(), lRequest.PostParameter.GetValue("id").First());
        }

        [Test]
        public override void ProxerClientTest()
        {
            base.ProxerClientTest();
        }

        [Test]
        public void SetUserInfoTest([Values] UserList list)
        {
            SetUserInfoInput lInput = new SetUserInfoInput {EntryId = this.GetRandomNumber(10000), List = list};
            IRequestBuilder lRequest = this.RequestBuilder.SetUserInfo(lInput);
            this.CheckUrl(lRequest, "info", "setuserinfo");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(
                lRequest.PostParameter.Any(pair => pair.Key == "id" && pair.Value == lInput.EntryId.ToString())
            );
            Assert.True(lRequest.PostParameter.Any(pair => pair.Key == "type" && pair.Value == list.ToTypeString()));
            Assert.True(lRequest.CheckLogin);
        }

        private EntryIdInput GetRandomEntryIdInput()
        {
            return new EntryIdInput {EntryId = this.GetRandomNumber(10000)};
        }
    }
}