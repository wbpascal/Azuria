using Azuria.Api.v1.DataModels.Info;
using Azuria.Api.v1.Input.Converter;
using Azuria.Enums.Info;
using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.List
{
    /// <summary>
    /// 
    /// </summary>
    public class TranslatorProjectsInput : InputDataModel
    {
        /// <summary>
        /// 
        /// </summary>
        [InputData("isH", Converter = typeof(IsHConverter), Optional = true)]
        public bool? IsH { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [InputData("type", ConverterMethodName = nameof(GetTranslationStatusString), Optional = true)]
        public TranslationStatus? TranslationStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [InputData("id")]
        public int TranslatorId { get; set; }

        internal string GetTranslationStatusString(TranslationStatus? status)
        {
            return ((int?) status)?.ToString();
        }
    }
}