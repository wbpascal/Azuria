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

        [Test, Sequential]
        public void EntrySearchTest(
            [Values] SearchResultSort sort, [Values] SearchMediaType type, [Values] LengthLimit lengthLimit)
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
                Type = SearchMediaType.OneShot,
                Limit = 30,
                Page = 1
            };
            IRequestBuilderWithResult<SearchDataModel[]> lRequest = this.RequestBuilder.EntrySearch(lInput);

            this.CheckUrl(lRequest, "list", "entrysearch");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("limit"));
            Assert.True(lRequest.PostParameter.ContainsKey("p"));
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
            Assert.AreEqual("30", lRequest.PostParameter.GetValue("limit").First());
            Assert.AreEqual("1", lRequest.PostParameter.GetValue("p").First());
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
                StartWithNonAlphabeticalChar = startWithNonAlphabeticalChar,
                Limit = 50,
                Page = 2
            };
            IRequestBuilderWithResult<SearchDataModel[]> lRequest = this.RequestBuilder.GetEntryList(lInput);

            this.CheckUrl(lRequest, "list", "entrylist");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("limit"));
            Assert.True(lRequest.PostParameter.ContainsKey("p"));
            Assert.True(lRequest.PostParameter.ContainsKey("kat"));
            Assert.True(lRequest.PostParameter.ContainsKey("isH"));
            Assert.True(lRequest.PostParameter.ContainsKey("start"));
            Assert.True(lRequest.PostParameter.ContainsKey("sort"));
            Assert.True(lRequest.PostParameter.ContainsKey("sort_type"));
            Assert.True(lRequest.PostParameter.ContainsKey("medium"));
            Assert.AreEqual("50", lRequest.PostParameter.GetValue("limit").First());
            Assert.AreEqual("2", lRequest.PostParameter.GetValue("p").First());
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
                Type = type,
                Limit = 30,
                Page = 2
            };
            IRequestBuilderWithResult<IndustryDataModel[]> lRequest = this.RequestBuilder.GetIndustries(lInput);
            this.CheckUrl(lRequest, "list", "industrys");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("start"));
            Assert.True(lRequest.PostParameter.ContainsKey("contains"));
            Assert.True(lRequest.PostParameter.ContainsKey("country"));
            Assert.True(lRequest.PostParameter.ContainsKey("type"));
            Assert.True(lRequest.PostParameter.ContainsKey("limit"));
            Assert.True(lRequest.PostParameter.ContainsKey("p"));
            Assert.AreEqual("test_start", lRequest.PostParameter.GetValue("start").First());
            Assert.AreEqual("test_contains", lRequest.PostParameter.GetValue("contains").First());
            Assert.AreEqual(country.ToShortString(), lRequest.PostParameter.GetValue("country").First());
            Assert.AreEqual(type.ToTypeString(), lRequest.PostParameter.GetValue("type").First());
            Assert.AreEqual("30", lRequest.PostParameter.GetValue("limit").First());
            Assert.AreEqual("2", lRequest.PostParameter.GetValue("p").First());
            Assert.False(lRequest.CheckLogin);
        }

        [Test, Combinatorial]
        public void GetIndustryProjectsTest([Values] IndustryType type, [Values(null, true, false)] bool? isH)
        {
            IndustryProjectsInput lInput = new IndustryProjectsInput
            {
                IndustryId = 42,
                IsH = isH,
                Type = type,
                Limit = 150,
                Page = 1
            };
            IRequestBuilderWithResult<IndustryProjectDataModel[]> lRequest =
                this.RequestBuilder.GetIndustryProjects(lInput);
            this.CheckUrl(lRequest, "list", "industryprojects");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("id"));
            Assert.True(lRequest.PostParameter.ContainsKey("type"));
            Assert.True(lRequest.PostParameter.ContainsKey("isH"));
            Assert.True(lRequest.PostParameter.ContainsKey("p"));
            Assert.True(lRequest.PostParameter.ContainsKey("limit"));
            Assert.AreEqual("42", lRequest.PostParameter.GetValue("id").First());
            Assert.AreEqual(type.ToTypeString(), lRequest.PostParameter.GetValue("type").First());
            Assert.AreEqual(GetIsHString(isH), lRequest.PostParameter.GetValue("isH").First());
            Assert.AreEqual("1", lRequest.PostParameter.GetValue("p").First());
            Assert.AreEqual("150", lRequest.PostParameter.GetValue("limit").First());
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetTagIdsTest()
        {
            TagIdSearchInput lInput = new TagIdSearchInput
            {
                TagsExclude = new[] {"testExclude1", "testExclude2"},
                TagsInclude = new[] {"test1"}
            };

            IRequestBuilderWithResult<Tuple<int[], int[]>> lRequest = this.RequestBuilder.GetTagIds(lInput);
            this.CheckUrl(lRequest, "list", "tagids");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("search"));
            Assert.AreEqual("test1 -testExclude1 -testExclude2", lRequest.GetParameters["search"]);
            Assert.NotNull(lRequest.CustomDataConverter);
            Assert.False(lRequest.CheckLogin);
        }

        [Test, Sequential]
        public void GetTagsTest(
            [Values] TagType type, [Values] TagSubtype subtype, [Values] TagListSort sort,
            [Values] SortDirection direction)
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
            Assert.True(lRequest.PostParameter.ContainsKey("search"));
            Assert.True(lRequest.PostParameter.ContainsKey("type"));
            Assert.True(lRequest.PostParameter.ContainsKey("sort"));
            Assert.True(lRequest.PostParameter.ContainsKey("sort_type"));
            Assert.True(lRequest.PostParameter.ContainsKey("subtype"));
            Assert.AreEqual("test_search", lRequest.PostParameter.GetValue("search").First());
            Assert.AreEqual(type.GetDescription(), lRequest.PostParameter.GetValue("type").First());
            Assert.AreEqual(sort.ToString().ToLowerInvariant(), lRequest.PostParameter.GetValue("sort").First());
            Assert.AreEqual(direction.GetDescription(), lRequest.PostParameter.GetValue("sort_type").First());
            Assert.AreEqual(subtype.GetDescription(), lRequest.PostParameter.GetValue("subtype").First());
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
                StartsWith = "test_start",
                Limit = 50,
                Page = 1
            };
            IRequestBuilderWithResult<TranslatorDataModel[]> lRequest = this.RequestBuilder.GetTranslatorgroups(lInput);
            this.CheckUrl(lRequest, "list", "translatorgroups");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("start"));
            Assert.True(lRequest.PostParameter.ContainsKey("contains"));
            Assert.True(lRequest.PostParameter.ContainsKey("country"));
            Assert.True(lRequest.PostParameter.ContainsKey("limit"));
            Assert.True(lRequest.PostParameter.ContainsKey("p"));
            Assert.AreEqual("test_start", lRequest.PostParameter.GetValue("start").First());
            Assert.AreEqual("test_contains", lRequest.PostParameter.GetValue("contains").First());
            Assert.AreEqual(
                country.ToShortString() ?? string.Empty, lRequest.PostParameter.GetValue("country").First());
            Assert.AreEqual("50", lRequest.PostParameter.GetValue("limit").First());
            Assert.AreEqual("1", lRequest.PostParameter.GetValue("p").First());
            Assert.False(lRequest.CheckLogin);
        }

        [Test, Combinatorial]
        public void GetTranslatorProjectsTest([Values] TranslationStatus status, [Values(null, true, false)] bool? isH)
        {
            if (status == TranslationStatus.Unkown) return;

            TranslatorProjectsInput lInput = new TranslatorProjectsInput
            {
                IsH = isH,
                TranslationStatus = status,
                TranslatorId = 42,
                Limit = 50,
                Page = 1
            };
            IRequestBuilderWithResult<TranslatorProjectDataModel[]> lRequest =
                this.RequestBuilder.GetTranslatorProjects(lInput);
            this.CheckUrl(lRequest, "list", "translatorgroupprojects");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("id"));
            Assert.True(lRequest.PostParameter.ContainsKey("type"));
            Assert.True(lRequest.PostParameter.ContainsKey("isH"));
            Assert.True(lRequest.PostParameter.ContainsKey("p"));
            Assert.True(lRequest.PostParameter.ContainsKey("limit"));
            Assert.AreEqual("42", lRequest.PostParameter.GetValue("id").First());
            Assert.AreEqual(((int) status).ToString(), lRequest.PostParameter.GetValue("type").First());
            Assert.AreEqual(GetIsHString(isH), lRequest.PostParameter.GetValue("isH").First());
            Assert.AreEqual("1", lRequest.PostParameter.GetValue("p").First());
            Assert.AreEqual("50", lRequest.PostParameter.GetValue("limit").First());
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