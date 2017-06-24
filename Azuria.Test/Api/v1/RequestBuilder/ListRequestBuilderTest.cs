using System.Linq;
using Azuria.Api.v1.DataModels.List;
using Azuria.Api.v1.Input;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Enums.Info;
using Azuria.Enums.List;
using Azuria.Requests.Builder;
using Azuria.Test.Core.Helpers;
using Xunit;

namespace Azuria.Test.Api.v1.RequestBuilder
{
    public class ListRequestBuilderTest : RequestBuilderTestBase<ListRequestBuilder>
    {
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
                this.RequestBuilder.EntrySearch(lInput, limit: 30, page: 1);
            
            this.CheckUrl(lRequest, "list", "entrysearch");
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.True(lRequest.PostArguments.ContainsKey("name"));
            Assert.True(lRequest.PostArguments.ContainsKey("type"));
            Assert.True(lRequest.PostArguments.ContainsKey("sort"));
            Assert.True(lRequest.PostArguments.ContainsKey("length-limit"));
            Assert.True(lRequest.PostArguments.ContainsKey("tagratefilter"));
            Assert.True(lRequest.PostArguments.ContainsKey("tagspoilerfilter"));
            Assert.True(lRequest.PostArguments.ContainsKey("language"));
            Assert.True(lRequest.PostArguments.ContainsKey("genre"));
            Assert.True(lRequest.PostArguments.ContainsKey("nogenre"));
            Assert.True(lRequest.PostArguments.ContainsKey("fsk"));
            Assert.True(lRequest.PostArguments.ContainsKey("length"));
            Assert.True(lRequest.PostArguments.ContainsKey("tags"));
            Assert.True(lRequest.PostArguments.ContainsKey("notags"));
            Assert.Equal("30", lRequest.GetParameters["limit"]);
            Assert.Equal("1", lRequest.GetParameters["p"]);
            Assert.Equal(lInput.Name, lRequest.PostArguments.GetValue("name").First());
            Assert.Equal("oneshot", lRequest.PostArguments.GetValue("type").First());
            Assert.Equal("clicks", lRequest.PostArguments.GetValue("sort").First());
            Assert.Equal("up", lRequest.PostArguments.GetValue("length-limit").First());
            Assert.Equal("rate_10", lRequest.PostArguments.GetValue("tagratefilter").First());
            Assert.Equal("spoiler_10", lRequest.PostArguments.GetValue("tagspoilerfilter").First());
            Assert.Equal("en", lRequest.PostArguments.GetValue("language").First());
            Assert.Equal("Action Abenteuer", lRequest.PostArguments.GetValue("genre").First());
            Assert.Equal("Adult", lRequest.PostArguments.GetValue("nogenre").First());
            Assert.Equal("bad_language", lRequest.PostArguments.GetValue("fsk").First());
            Assert.Equal("150", lRequest.PostArguments.GetValue("length").First());
            Assert.Equal("15 4", lRequest.PostArguments.GetValue("tags").First());
            Assert.Equal("2 3", lRequest.PostArguments.GetValue("notags").First());
        }

        [Fact]
        public override void ProxerClientTest()
        {
            base.ProxerClientTest();
        }
    }
}