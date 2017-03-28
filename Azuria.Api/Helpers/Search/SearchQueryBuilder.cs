using System.Collections.Generic;
using Azuria.Api.Enums;
using Azuria.Api.Enums.Info;
using Azuria.Api.Enums.List;
using Azuria.Api.Helpers.Extensions;
using Azuria.Api.Input;

namespace Azuria.Api.Helpers.Search
{
    internal static class SearchQueryBuilder
    {
        #region Methods

        internal static Dictionary<string, string> Build(SearchInput input)
        {
            if (input == null) return new Dictionary<string, string>();
            Dictionary<string, string> lReturn = new Dictionary<string, string>
            {
                {"name", input.SearchTerm},
                {"type", TypeToString(input.Type)},
                {"sort", input.Sort.ToString().ToLowerInvariant()},
                {"length-limit", input.LengthLimit.ToString().ToLowerInvariant()},
                {"tagratefilter", input.IsFilteringUnratedTags ? "rate_1" : "rate_10"},
                {"tagspoilerfilter", input.IsFilteringSpoilerTags ? "spoiler_0" : "spoiler_10"}
            };
            lReturn.AddIf("language", input.Language.ToShortString(), (key, value) => !string.IsNullOrEmpty(value));
            lReturn.AddIf("genre", GenresToString(input.GenreInclude), (key, value) => !string.IsNullOrEmpty(value));
            lReturn.AddIf("nogenre", GenresToString(input.GenreExclude), (key, value) => !string.IsNullOrEmpty(value));
            lReturn.AddIf("fsk", FskToString(input.Fsk), (key, value) => !string.IsNullOrEmpty(value));
            lReturn.AddIf("length", input.Length.ToString(), (key, value) => input.Length != null);
            lReturn.AddIf("tags", input.TagsInclude.ToString(' '), (key, value) => !string.IsNullOrEmpty(value));
            lReturn.AddIf("notags", input.TagsExclude.ToString(' '), (key, value) => !string.IsNullOrEmpty(value));

            return lReturn;
        }

        internal static Dictionary<string, string> Build(EntryListInput input)
        {
            if (input == null) return new Dictionary<string, string>();
            Dictionary<string, string> lReturn = new Dictionary<string, string>
            {
                {"isH", input.ShowHContent.ToString()},
                {"start", input.StartWithNonAlphabeticalChar ? "nonAlpha" : input.StartWith}
            };
            if (input.Medium != MediaMedium.None)
                lReturn.Add("medium", input.Medium.ToString().ToLowerInvariant());

            return lReturn;
        }

        private static string FskToString(IEnumerable<Fsk> fskTypes)
        {
            if (fskTypes == null) return string.Empty;

            string lReturn = string.Empty;
            foreach (Fsk fskType in fskTypes)
                if (FskHelpers.FskToStringDictionary.ContainsKey(fskType))
                    lReturn += FskHelpers.FskToStringDictionary[fskType] + " ";
            return lReturn.TrimEnd();
        }

        private static string GenresToString(IEnumerable<Genre> genre)
        {
            if (genre == null) return string.Empty;

            string lReturn = string.Empty;
            Dictionary<Genre, string> lLookupDictionary = GenreHelpers.StringToGenreDictionary.ReverseDictionary();
            foreach (Genre genreType in genre)
                if (lLookupDictionary.ContainsKey(genreType)) lReturn += lLookupDictionary[genreType] + " ";
            return lReturn.TrimEnd();
        }

        private static string TypeToString(MediaSearchType type)
        {
            switch (type)
            {
                case MediaSearchType.AllAnime:
                    return "all-anime";
                case MediaSearchType.AllManga:
                    return "all-manga";
                default:
                    return type.ToString().ToLowerInvariant();
            }
        }

        #endregion
    }
}