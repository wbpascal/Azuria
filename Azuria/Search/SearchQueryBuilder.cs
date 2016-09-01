using System.Collections.Generic;
using Azuria.AnimeManga.Properties;
using Azuria.Search.Input;
using Azuria.Utilities.Extensions;

namespace Azuria.Search
{
    internal static class SearchQueryBuilder
    {
        #region

        internal static Dictionary<string, string> Build(SearchInput input)
        {
            Dictionary<string, string> lReturn = new Dictionary<string, string>
            {
                {"name", input.SearchTerm},
                {"type", TypeToString(input.Type)},
                {"sort", input.Sort.ToString().ToLowerInvariant()},
                {"length-limit", input.LengthLimit.ToString().ToLowerInvariant()},
                {"tagratefilter", input.IsFilteringUnratedTags ? "rate_1" : "rate-10"},
                {"tagspoilerfilter", input.IsFilteringSpoilerTags ? "spoiler_0" : "spoiler_10"}
            };
            lReturn.AddIf("language", LanguageToString(input.Language), (key, value) => string.IsNullOrEmpty(value));
            lReturn.AddIf("genre", GenresToString(input.GenreInclude), (key, value) => string.IsNullOrEmpty(value));
            lReturn.AddIf("nogenre", GenresToString(input.GenreExclude), (key, value) => string.IsNullOrEmpty(value));
            lReturn.AddIf("fsk", FskToString(input.Fsk), (key, value) => string.IsNullOrEmpty(value));
            lReturn.AddIf("length", input.Length.ToString(), (key, value) => input.Length != null);
            lReturn.AddIf("tags", TagsToString(input.TagsInclude), (key, value) => string.IsNullOrEmpty(value));
            lReturn.AddIf("notags", TagsToString(input.TagsExclude), (key, value) => string.IsNullOrEmpty(value));

            return lReturn;
        }

        private static string FskToString(IEnumerable<FskType> fskTypes)
        {
            if (fskTypes == null) return string.Empty;

            string lReturn = string.Empty;
            foreach (FskType fskType in fskTypes)
                if (FskHelper.FskToStringDictionary.ContainsKey(fskType))
                    lReturn += FskHelper.FskToStringDictionary[fskType] + " ";
            return lReturn.TrimEnd();
        }

        private static string GenresToString(IEnumerable<GenreType> genre)
        {
            if (genre == null) return string.Empty;

            string lReturn = string.Empty;
            Dictionary<GenreType, string> lLookupDictionary = GenreHelper.StringToGenreDictionary.ReverseDictionary();
            foreach (GenreType genreType in genre)
                if (lLookupDictionary.ContainsKey(genreType)) lReturn += lLookupDictionary[genreType] + " ";
            return lReturn.TrimEnd();
        }

        private static string LanguageToString(SearchLanguage language)
        {
            switch (language)
            {
                case SearchLanguage.English:
                    return "en";
                case SearchLanguage.German:
                    return "de";
                default:
                    return "";
            }
        }

        private static string TagsToString(IEnumerable<TagType> tags)
        {
            if (tags == null) return string.Empty;

            string lReturn = string.Empty;
            foreach (TagType tag in tags)
                lReturn += (int) tag + " ";
            return lReturn.TrimEnd();
        }

        private static string TypeToString(AnimeMangaSearchType type)
        {
            switch (type)
            {
                case AnimeMangaSearchType.AllAnime:
                    return "all-anime";
                case AnimeMangaSearchType.AllManga:
                    return "all-manga";
                default:
                    return type.ToString().ToLowerInvariant();
            }
        }

        #endregion
    }
}