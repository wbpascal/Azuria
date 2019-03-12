using Azuria.Api.v1.DataModels.Info;
using Azuria.Api.v1.Input.Converter;
using Azuria.Enums.Info;
using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.List
{
    /// <summary>
    /// 
    /// </summary>
    public class TranslatorProjectsInput : PagedInputDataModel
    {
        /// <summary>
        /// Gets or sets whether only H-content should be included in the results. If null both H and non-H content will be included.
        /// </summary>
        [InputData("isH", Converter = typeof(IsHConverter))]
        public bool? IsH { get; set; }

        /// <summary>
        /// Gets or sets the translation status all returned entries should have.
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("type", ConverterMethodName = nameof(GetTranslationStatusString), Optional = true)]
        public TranslationStatus? TranslationStatus { get; set; }

        /// <summary>
        /// Gets or sets the id of the translator group.
        /// </summary>
        [InputData("id")]
        public int TranslatorId { get; set; }

        internal string GetTranslationStatusString(TranslationStatus? status)
        {
            return ((int?) status)?.ToString();
        }
    }
}