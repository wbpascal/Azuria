using System;
using System.Linq;
using System.Reflection;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Api.v1.DataModels.List;
using Azuria.Api.v1.Input.List;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Enums;
using Azuria.Enums.Info;
using Azuria.Enums.List;
using Azuria.Helpers.Extensions;
using Azuria.Requests.Builder;
using Azuria.Test.Core.Helpers;
using NUnit.Framework;
using TagDataModel = Azuria.Api.v1.DataModels.List.TagDataModel;

namespace Azuria.Test.Api.v1.RequestBuilder
{
    [TestFixture]
    public class ListRequestBuilderTest : RequestBuilderTestBase<ListRequestBuilder>
    {
        [Test]
        public void EntrySearchInputNullTest()
        {
            Assert.Throws<ArgumentNullException>(() => this.RequestBuilder.EntrySearch(null));
        }

        [Test]
        public void EntrySearchTest()
        {
            SearchInput lInput = new SearchInput
            {
                FilterSpoilerTags = null,
                FilterUnratedTags = false,
                Fsk = new[] {Fsk.BadLanguage},
                GenreExclude = new[] {Genre.Adult},
                GenreInclude = new[] {Genre.Action, Genre.Adventure},
                Language = Language.English,
                Length = 150,
                LengthLimit = LengthLimit.Up,
                Name = "test_name",
                Sort = SearchResultSort.Clicks,
                TagsExclude = new[] {2, 3},
                TagsInclude = new[] {15, 4},
                Type = SearchMediaType.OneShot
            };
            IRequestBuilderWithResult<SearchDataModel[]> lRequest =
                this.RequestBuilder.EntrySearch(lInput, 30, 1);

            this.CheckUrl(lRequest, "list", "entrysearch");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.True(lRequest.PostParameter.ContainsKey("name"));
            Assert.True(lRequest.PostParameter.ContainsKey("type"));
            Assert.True(lRequest.PostParameter.ContainsKey("sort"));
            Assert.True(lRequest.PostParameter.ContainsKey("length-limit"));
            Assert.True(lRequest.PostParameter.ContainsKey("tagratefilter"));
            Assert.True(lRequest.PostParameter.ContainsKey("tagspoilerfilter"));
            Assert.True(lRequest.PostParameter.ContainsKey("language"));
            Assert.True(lRequest.PostParameter.ContainsKey("genre"));
            Assert.True(lRequest.PostParameter.ContainsKey("nogenre"));
            Assert.True(lRequest.PostParameter.ContainsKey("fsk"));
            Assert.True(lRequest.PostParameter.ContainsKey("length"));
            Assert.True(lRequest.PostParameter.ContainsKey("tags"));
            Assert.True(lRequest.PostParameter.ContainsKey("notags"));
            Assert.AreEqual("30", lRequest.GetParameters["limit"]);
            Assert.AreEqual("1", lRequest.GetParameters["p"]);
            Assert.AreEqual(lInput.Name, lRequest.PostParameter.GetValue("name").First());
            Assert.AreEqual("oneshot", lRequest.PostParameter.GetValue("type").First());
            Assert.AreEqual("clicks", lRequest.PostParameter.GetValue("sort").First());
            Assert.AreEqual("up", lRequest.PostParameter.GetValue("length-limit").First());
            Assert.AreEqual("rate_10", lRequest.PostParameter.GetValue("tagratefilter").First());
            Assert.AreEqual("spoiler_10", lRequest.PostParameter.GetValue("tagspoilerfilter").First());
            Assert.AreEqual("en", lRequest.PostParameter.GetValue("language").First());
            Assert.AreEqual("Action Abenteuer", lRequest.PostParameter.GetValue("genre").First());
            Assert.AreEqual("Adult", lRequest.PostParameter.GetValue("nogenre").First());
            Assert.AreEqual("bad_language", lRequest.PostParameter.GetValue("fsk").First());
            Assert.AreEqual("150", lRequest.PostParameter.GetValue("length").First());
            Assert.AreEqual("15 4", lRequest.PostParameter.GetValue("tags").First());
            Assert.AreEqual("2 3", lRequest.PostParameter.GetValue("notags").First());
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetEntryListInputNullTest()
        {
            Assert.Throws<ArgumentNullException>(() => this.RequestBuilder.GetEntryList(null));
        }

        [Test]
        public void GetEntryListTest([Values] bool startWithNonAlphabeticalChar)
        {
            EntryListInput lInput = new EntryListInput
            {
                Category = MediaEntryType.Manga,
                Medium = MediaMedium.Mangaseries,
                ShowHContent = true,
                SortBy = EntryListSort.Rating,
                SortDirection = SortDirection.Descending,
                StartsWith = "a",
                StartWithNonAlphabeticalChar = startWithNonAlphabeticalChar
            };
            IRequestBuilderWithResult<SearchDataModel[]> lRequest = this.RequestBuilder.GetEntryList(lInput, 50, 2);

            this.CheckUrl(lRequest, "list", "entrylist");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.True(lRequest.PostParameter.ContainsKey("kat"));
            Assert.True(lRequest.PostParameter.ContainsKey("isH"));
            Assert.True(lRequest.PostParameter.ContainsKey("start"));
            Assert.True(lRequest.PostParameter.ContainsKey("sort"));
            Assert.True(lRequest.PostParameter.ContainsKey("sort_type"));
            Assert.True(lRequest.PostParameter.ContainsKey("medium"));
            Assert.AreEqual("50", lRequest.GetParameters["limit"]);
            Assert.AreEqual("2", lRequest.GetParameters["p"]);
            Assert.AreEqual("manga", lRequest.PostParameter.GetValue("kat").First());
            Assert.AreEqual("true", lRequest.PostParameter.GetValue("isH").First());
            Assert.AreEqual(
                startWithNonAlphabeticalChar ? "nonAlpha" : lInput.StartsWith,
                lRequest.PostParameter.GetValue("start").First()
            );
            Assert.AreEqual("rating", lRequest.PostParameter.GetValue("sort").First());
            Assert.AreEqual("DESC", lRequest.PostParameter.GetValue("sort_type").First());
            Assert.AreEqual("mangaseries", lRequest.PostParameter.GetValue("medium").First());
            Assert.False(lRequest.CheckLogin);
        }

        [Test, Combinatorial]
        public void GetIndustriesCountryTest(
            [Values(Country.UnitedStates, Country.Japan, Country.Germany, Country.Misc)] Country country,
            [Values] IndustryType type)
        {
            IndustryListInput lInput = new IndustryListInput
            {
                Country = country,
                Contains = "test_contains",
                StartsWith = "test_start",
                Type = type
            };
            IRequestBuilderWithResult<IndustryDataModel[]> lRequest =
                this.RequestBuilder.GetIndustries(lInput, 30, 2);
            this.CheckUrl(lRequest, "list", "industrys");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("start"));
            Assert.True(lRequest.GetParameters.ContainsKey("contains"));
            Assert.True(lRequest.GetParameters.ContainsKey("country"));
            Assert.True(lRequest.GetParameters.ContainsKey("type"));
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.AreEqual("test_start", lRequest.GetParameters["start"]);
            Assert.AreEqual("test_contains", lRequest.GetParameters["contains"]);
            Assert.AreEqual(country.ToShortString(), lRequest.GetParameters["country"]);
            Assert.AreEqual(type.ToTypeString(), lRequest.GetParameters["type"]);
            Assert.AreEqual("30", lRequest.GetParameters["limit"]);
            Assert.AreEqual("2", lRequest.GetParameters["p"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test, Combinatorial]
        public void GetIndustryProjectsTest([Values] IndustryType type, [Values(null, true, false)] bool? isH)
        {
            IndustryProjectsInput lInput = new IndustryProjectsInput
            {
                IndustryId = 42,
                IsH = isH,
                Type = type
            };
            IRequestBuilderWithResult<IndustryProjectDataModel[]> lRequest =
                this.RequestBuilder.GetIndustryProjects(lInput, 1, 150);
            this.CheckUrl(lRequest, "list", "industryprojects");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.True(lRequest.GetParameters.ContainsKey("type"));
            Assert.True(lRequest.GetParameters.ContainsKey("isH"));
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            Assert.AreEqual("42", lRequest.GetParameters["id"]);
            Assert.AreEqual(type.ToTypeString(), lRequest.GetParameters["type"]);
            Assert.AreEqual(GetIsHString(isH), lRequest.GetParameters["isH"]);
            Assert.AreEqual("1", lRequest.GetParameters["p"]);
            Assert.AreEqual("150", lRequest.GetParameters["limit"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetTagIdsNullInputTest()
        {
            Assert.Throws<ArgumentNullException>(() => this.RequestBuilder.GetTagIds(null, new string[0]));
            Assert.Throws<ArgumentNullException>(() => this.RequestBuilder.GetTagIds(new string[0], null));
        }

        [Test]
        public void GetTagIdsTest()
        {
            IRequestBuilderWithResult<Tuple<int[], int[]>> lRequest = this.RequestBuilder.GetTagIds(
                new[] {"test1"}, new[] {"testExclude1", "testExclude2"});
            this.CheckUrl(lRequest, "list", "tagids");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("search"));
            Assert.AreEqual("test1 -testExclude1 -testExclude2", lRequest.GetParameters["search"]);
            Assert.NotNull(lRequest.CustomDataConverter);
            Assert.False(lRequest.CheckLogin);
        }

        [Test, Sequential]
        public void GetTagsTest([Values] TagType type, [Values] TagSubtype subtype, [Values] TagListSort sort, [Values] SortDirection direction)
        {
            TagListInput lInput = new TagListInput
            {
                Search = "test_search",
                Type = type,
                Sort = sort,
                SortDirection = direction,
                Subtype = subtype
            };
            IRequestBuilderWithResult<TagDataModel[]> lRequest = this.RequestBuilder.GetTags(lInput);
            this.CheckUrl(lRequest, "list", "tags");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("search"));
            Assert.True(lRequest.GetParameters.ContainsKey("type"));
            Assert.True(lRequest.GetParameters.ContainsKey("sort"));
            Assert.True(lRequest.GetParameters.ContainsKey("sort_type"));
            Assert.True(lRequest.GetParameters.ContainsKey("subtype"));
            Assert.AreEqual("test_search", lRequest.GetParameters["search"]);
            Assert.AreEqual(type.GetDescription(), lRequest.GetParameters["type"]);
            Assert.AreEqual(sort.ToString().ToLowerInvariant(), lRequest.GetParameters["sort"]);
            Assert.AreEqual(direction.GetDescription(), lRequest.GetParameters["sort_type"]);
            Assert.AreEqual(subtype.GetDescription(), lRequest.GetParameters["subtype"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetTranslatorgroupsTest(
            [Values(Country.England, Country.Germany, Country.Misc)] Country country)
        {
            TranslatorListInput lInput = new TranslatorListInput
            {
                Contains = "test_contains",
                Country = country,
                StartsWith = "test_start"
            };
            IRequestBuilderWithResult<TranslatorDataModel[]> lRequest =
                this.RequestBuilder.GetTranslatorgroups(lInput, 50, 1);
            this.CheckUrl(lRequest, "list", "translatorgroups");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("start"));
            Assert.True(lRequest.GetParameters.ContainsKey("contains"));
            Assert.True(lRequest.GetParameters.ContainsKey("country"));
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.AreEqual("test_start", lRequest.GetParameters["start"]);
            Assert.AreEqual("test_contains", lRequest.GetParameters["contains"]);
            Assert.AreEqual(country.ToShortString() ?? string.Empty, lRequest.GetParameters["country"]);
            Assert.AreEqual("50", lRequest.GetParameters["limit"]);
            Assert.AreEqual("1", lRequest.GetParameters["p"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test, Combinatorial]
        public void GetTranslatorProjectsTest([Values] TranslationStatus status, [Values(null, true, false)] bool? isH)
        {
            if(status == TranslationStatus.Unkown) return;
            
            TranslatorProjectsInput lInput = new TranslatorProjectsInput
            {
                IsH = isH,
                TranslationStatus = status,
                TranslatorId = 42
            };
            IRequestBuilderWithResult<TranslatorProjectDataModel[]> lRequest =
                this.RequestBuilder.GetTranslatorProjects(lInput, 1, 50);
            this.CheckUrl(lRequest, "list", "translatorgroupprojects");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.True(lRequest.GetParameters.ContainsKey("type"));
            Assert.True(lRequest.GetParameters.ContainsKey("isH"));
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            Assert.AreEqual("42", lRequest.GetParameters["id"]);
            Assert.AreEqual(((int) status).ToString(), lRequest.GetParameters["type"]);
            Assert.AreEqual(GetIsHString(isH), lRequest.GetParameters["isH"]);
            Assert.AreEqual("1", lRequest.GetParameters["p"]);
            Assert.AreEqual("50", lRequest.GetParameters["limit"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public override void ProxerClientTest()
        {
            base.ProxerClientTest();
        }

        private static string GetIsHString(bool? isH)
        {
            switch (isH)
            {
                case true:
                    return "1";
                case false:
                    return "-1";
                default:
                    return "0";
            }
        }
    }
}