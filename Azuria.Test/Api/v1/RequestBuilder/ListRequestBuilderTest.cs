using System;
using System.Linq;
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
using Xunit;

namespace Azuria.Test.Api.v1.RequestBuilder
{
    public class ListRequestBuilderTest : RequestBuilderTestBase<ListRequestBuilder>
    {
        [Fact]
        public void EntrySearchInputNullTest()
        {
            Assert.Throws<ArgumentNullException>(() => this.RequestBuilder.EntrySearch(null));
        }

        [Fact]
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
            Assert.Same(this.ProxerClient, lRequest.Client);
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
            Assert.Equal("30", lRequest.GetParameters["limit"]);
            Assert.Equal("1", lRequest.GetParameters["p"]);
            Assert.Equal(lInput.Name, lRequest.PostParameter.GetValue("name").First());
            Assert.Equal("oneshot", lRequest.PostParameter.GetValue("type").First());
            Assert.Equal("clicks", lRequest.PostParameter.GetValue("sort").First());
            Assert.Equal("up", lRequest.PostParameter.GetValue("length-limit").First());
            Assert.Equal("rate_10", lRequest.PostParameter.GetValue("tagratefilter").First());
            Assert.Equal("spoiler_10", lRequest.PostParameter.GetValue("tagspoilerfilter").First());
            Assert.Equal("en", lRequest.PostParameter.GetValue("language").First());
            Assert.Equal("Action Abenteuer", lRequest.PostParameter.GetValue("genre").First());
            Assert.Equal("Adult", lRequest.PostParameter.GetValue("nogenre").First());
            Assert.Equal("bad_language", lRequest.PostParameter.GetValue("fsk").First());
            Assert.Equal("150", lRequest.PostParameter.GetValue("length").First());
            Assert.Equal("15 4", lRequest.PostParameter.GetValue("tags").First());
            Assert.Equal("2 3", lRequest.PostParameter.GetValue("notags").First());
            Assert.False(lRequest.CheckLogin);
        }

        [Fact]
        public void GetEntryListInputNullTest()
        {
            Assert.Throws<ArgumentNullException>(() => this.RequestBuilder.GetEntryList(null));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void GetEntryListTest(bool startWithNonAlphabeticalChar)
        {
            EntryListInput lInput = new EntryListInput
            {
                Category = MediaEntryType.Manga,
                Medium = MediaMedium.Mangaseries,
                ShowHContent = true,
                SortBy = EntryListSort.Rating,
                SortDirection = SortDirection.Descending,
                StartWith = "a",
                StartWithNonAlphabeticalChar = startWithNonAlphabeticalChar
            };
            IRequestBuilderWithResult<SearchDataModel[]> lRequest = this.RequestBuilder.GetEntryList(lInput, 50, 2);

            this.CheckUrl(lRequest, "list", "entrylist");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.True(lRequest.PostParameter.ContainsKey("kat"));
            Assert.True(lRequest.PostParameter.ContainsKey("isH"));
            Assert.True(lRequest.PostParameter.ContainsKey("start"));
            Assert.True(lRequest.PostParameter.ContainsKey("sort"));
            Assert.True(lRequest.PostParameter.ContainsKey("sort_type"));
            Assert.True(lRequest.PostParameter.ContainsKey("medium"));
            Assert.Equal("50", lRequest.GetParameters["limit"]);
            Assert.Equal("2", lRequest.GetParameters["p"]);
            Assert.Equal("manga", lRequest.PostParameter.GetValue("kat").First());
            Assert.Equal("true", lRequest.PostParameter.GetValue("isH").First());
            Assert.Equal(
                startWithNonAlphabeticalChar ? "nonAlpha" : lInput.StartWith,
                lRequest.PostParameter.GetValue("start").First()
            );
            Assert.Equal("rating", lRequest.PostParameter.GetValue("sort").First());
            Assert.Equal("DESC", lRequest.PostParameter.GetValue("sort_type").First());
            Assert.Equal("mangaseries", lRequest.PostParameter.GetValue("medium").First());
            Assert.False(lRequest.CheckLogin);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(Country.Germany)]
        [InlineData(Country.Japan)]
        [InlineData(Country.Misc)]
        [InlineData(Country.UnitedStates)]
        public void GetIndustriesCountryTest(Country? country)
        {
            IRequestBuilderWithResult<IndustryDataModel[]> lRequest =
                this.RequestBuilder.GetIndustries("test_start", "test_contains", country, null, 30, 2);
            this.CheckUrl(lRequest, "list", "industrys");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("start"));
            Assert.True(lRequest.GetParameters.ContainsKey("contains"));
            Assert.True(lRequest.GetParameters.ContainsKey("country"));
            Assert.True(lRequest.GetParameters.ContainsKey("type"));
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.Equal("test_start", lRequest.GetParameters["start"]);
            Assert.Equal("test_contains", lRequest.GetParameters["contains"]);
            Assert.Equal(country?.ToShortString() ?? string.Empty, lRequest.GetParameters["country"]);
            Assert.Equal(string.Empty, lRequest.GetParameters["type"]);
            Assert.Equal("30", lRequest.GetParameters["limit"]);
            Assert.Equal("2", lRequest.GetParameters["p"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Fact]
        public void GetIndustriesInvalidCountryTest()
        {
            Assert.Throws<ArgumentException>(() => this.RequestBuilder.GetIndustries(country: Country.England));
        }

        [Fact]
        public void GetIndustriesInvalidTypeTest()
        {
            Assert.Throws<ArgumentException>(() => this.RequestBuilder.GetIndustries(type: IndustryType.Unknown));
        }

        [Theory]
        [InlineData(null)]
        [InlineData(IndustryType.Producer)]
        [InlineData(IndustryType.Publisher)]
        [InlineData(IndustryType.RecordLabel)]
        [InlineData(IndustryType.Streaming)]
        [InlineData(IndustryType.Studio)]
        [InlineData(IndustryType.TalentAgent)]
        public void GetIndustriesTypeTest(IndustryType? type)
        {
            IRequestBuilderWithResult<IndustryDataModel[]> lRequest =
                this.RequestBuilder.GetIndustries("test_start", "test_contains", Country.Germany, type, 30, 2);
            this.CheckUrl(lRequest, "list", "industrys");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("start"));
            Assert.True(lRequest.GetParameters.ContainsKey("contains"));
            Assert.True(lRequest.GetParameters.ContainsKey("country"));
            Assert.True(lRequest.GetParameters.ContainsKey("type"));
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.Equal("test_start", lRequest.GetParameters["start"]);
            Assert.Equal("test_contains", lRequest.GetParameters["contains"]);
            Assert.Equal("de", lRequest.GetParameters["country"]);
            Assert.Equal(type?.ToTypeString() ?? string.Empty, lRequest.GetParameters["type"]);
            Assert.Equal("30", lRequest.GetParameters["limit"]);
            Assert.Equal("2", lRequest.GetParameters["p"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Fact]
        public void GetIndustryProjectsInvalidTypeTest()
        {
            Assert.Throws<ArgumentException>(
                () => this.RequestBuilder.GetIndustryProjects(42, IndustryType.Unknown)
            );
        }

        [Theory]
        [InlineData(null)]
        [InlineData(IndustryType.Producer)]
        [InlineData(IndustryType.Publisher)]
        [InlineData(IndustryType.RecordLabel)]
        [InlineData(IndustryType.Streaming)]
        [InlineData(IndustryType.Studio)]
        [InlineData(IndustryType.TalentAgent)]
        public void GetIndustryProjectsTest(IndustryType? type)
        {
            IRequestBuilderWithResult<IndustryProjectDataModel[]> lRequest =
                this.RequestBuilder.GetIndustryProjects(42, type, null, 1, 150);
            this.CheckUrl(lRequest, "list", "industryprojects");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.True(lRequest.GetParameters.ContainsKey("type"));
            Assert.True(lRequest.GetParameters.ContainsKey("isH"));
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            Assert.Equal("42", lRequest.GetParameters["id"]);
            Assert.Equal(type?.ToTypeString() ?? string.Empty, lRequest.GetParameters["type"]);
            Assert.Equal("0", lRequest.GetParameters["isH"]);
            Assert.Equal("1", lRequest.GetParameters["p"]);
            Assert.Equal("150", lRequest.GetParameters["limit"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Fact]
        public void GetTagIdsNullInputTest()
        {
            Assert.Throws<ArgumentNullException>(() => this.RequestBuilder.GetTagIds(null, new string[0]));
            Assert.Throws<ArgumentNullException>(() => this.RequestBuilder.GetTagIds(new string[0], null));
        }

        [Fact]
        public void GetTagIdsTest()
        {
            IRequestBuilderWithResult<Tuple<int[], int[]>> lRequest = this.RequestBuilder.GetTagIds(
                new[] {"test1"}, new[] {"testExclude1", "testExclude2"});
            this.CheckUrl(lRequest, "list", "tagids");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("search"));
            Assert.Equal("test1 -testExclude1 -testExclude2", lRequest.GetParameters["search"]);
            Assert.NotNull(lRequest.CustomDataConverter);
            Assert.False(lRequest.CheckLogin);
        }

        [Fact]
        public void GetTagsInvalidSubtypeTest()
        {
            Assert.Throws<ArgumentException>(() => this.RequestBuilder.GetTags(subtype: TagSubtype.Unkown));
        }

        [Fact]
        public void GetTagsInvalidTypeTest()
        {
            Assert.Throws<ArgumentException>(() => this.RequestBuilder.GetTags(type: TagType.Unkown));
        }

        [Theory]
        [InlineData(null)]
        [InlineData(TagSubtype.AnimationDrawing)]
        [InlineData(TagSubtype.Feelings)]
        [InlineData(TagSubtype.Future)]
        [InlineData(TagSubtype.Misc)]
        [InlineData(TagSubtype.People)]
        [InlineData(TagSubtype.Personality)]
        [InlineData(TagSubtype.Protagonist)]
        [InlineData(TagSubtype.Sport)]
        [InlineData(TagSubtype.Story)]
        [InlineData(TagSubtype.Supernatural)]
        public void GetTagsSubypeTest(TagSubtype? subtype)
        {
            IRequestBuilderWithResult<TagDataModel[]> lRequest = this.RequestBuilder.GetTags(
                "test_search", TagType.Gallery, TagListSort.Id, SortDirection.Descending, subtype);
            this.CheckUrl(lRequest, "list", "tags");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("search"));
            Assert.True(lRequest.GetParameters.ContainsKey("type"));
            Assert.True(lRequest.GetParameters.ContainsKey("sort"));
            Assert.True(lRequest.GetParameters.ContainsKey("sort_type"));
            Assert.True(lRequest.GetParameters.ContainsKey("subtype"));
            Assert.Equal("test_search", lRequest.GetParameters["search"]);
            Assert.Equal("gallery", lRequest.GetParameters["type"]);
            Assert.Equal("id", lRequest.GetParameters["sort"]);
            Assert.Equal("DESC", lRequest.GetParameters["sort_type"]);
            Assert.Equal(subtype?.GetDescription() ?? string.Empty, lRequest.GetParameters["subtype"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(TagType.EntryGenre)]
        [InlineData(TagType.EntryTag)]
        [InlineData(TagType.EntryTagH)]
        [InlineData(TagType.Gallery)]
        [InlineData(TagType.EntryGenre)]
        public void GetTagsTypeTest(TagType? type)
        {
            IRequestBuilderWithResult<TagDataModel[]> lRequest = this.RequestBuilder.GetTags(
                "test_search", type, TagListSort.Id, SortDirection.Descending, null);
            this.CheckUrl(lRequest, "list", "tags");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("search"));
            Assert.True(lRequest.GetParameters.ContainsKey("type"));
            Assert.True(lRequest.GetParameters.ContainsKey("sort"));
            Assert.True(lRequest.GetParameters.ContainsKey("sort_type"));
            Assert.True(lRequest.GetParameters.ContainsKey("subtype"));
            Assert.Equal("test_search", lRequest.GetParameters["search"]);
            Assert.Equal(type?.GetDescription() ?? string.Empty, lRequest.GetParameters["type"]);
            Assert.Equal("id", lRequest.GetParameters["sort"]);
            Assert.Equal("DESC", lRequest.GetParameters["sort_type"]);
            Assert.Equal(string.Empty, lRequest.GetParameters["subtype"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Fact]
        public void GetTranslatorgroupsInvalidCountryTest()
        {
            Assert.Throws<ArgumentException>(() => this.RequestBuilder.GetTranslatorgroups(country: Country.Japan));
            Assert.Throws<ArgumentException>(
                () => this.RequestBuilder.GetTranslatorgroups(country: Country.UnitedStates)
            );
        }

        [Theory]
        [InlineData(null)]
        [InlineData(Country.England)]
        [InlineData(Country.Germany)]
        [InlineData(Country.Misc)]
        public void GetTranslatorgroupsTest(Country? country)
        {
            IRequestBuilderWithResult<TranslatorDataModel[]> lRequest =
                this.RequestBuilder.GetTranslatorgroups("test_start", "test_contains", country, 50, 1);
            this.CheckUrl(lRequest, "list", "translatorgroups");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("start"));
            Assert.True(lRequest.GetParameters.ContainsKey("contains"));
            Assert.True(lRequest.GetParameters.ContainsKey("country"));
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.Equal("test_start", lRequest.GetParameters["start"]);
            Assert.Equal("test_contains", lRequest.GetParameters["contains"]);
            Assert.Equal(country?.ToShortString() ?? string.Empty, lRequest.GetParameters["country"]);
            Assert.Equal("50", lRequest.GetParameters["limit"]);
            Assert.Equal("1", lRequest.GetParameters["p"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(TranslationStatus.Cancelled)]
        [InlineData(TranslationStatus.Finished)]
        [InlineData(TranslationStatus.Licensed)]
        [InlineData(TranslationStatus.Ongoing)]
        [InlineData(TranslationStatus.Planned)]
        [InlineData(TranslationStatus.Unkown)]
        public void GetTranslatorProjectsTest(TranslationStatus? status)
        {
            IRequestBuilderWithResult<TranslatorProjectDataModel[]> lRequest =
                this.RequestBuilder.GetTranslatorProjects(42, status, true, 1, 50);
            this.CheckUrl(lRequest, "list", "translatorgroupprojects");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.True(lRequest.GetParameters.ContainsKey("type"));
            Assert.True(lRequest.GetParameters.ContainsKey("isH"));
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            Assert.Equal("42", lRequest.GetParameters["id"]);
            Assert.Equal(
                status != null ? ((int) status.Value).ToString() : string.Empty, lRequest.GetParameters["type"]);
            Assert.Equal("1", lRequest.GetParameters["isH"]);
            Assert.Equal("1", lRequest.GetParameters["p"]);
            Assert.Equal("50", lRequest.GetParameters["limit"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Fact]
        public override void ProxerClientTest()
        {
            base.ProxerClientTest();
        }
    }
}