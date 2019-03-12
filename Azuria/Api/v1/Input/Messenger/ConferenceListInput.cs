using Azuria.Api.v1.Input.Converter;
using Azuria.Enums.Messenger;
using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.Messenger
{
    /// <summary>
    /// 
    /// </summary>
    public class ConferenceListInput : InputDataModel
    {
        /// <summary>
        /// Gets or sets the list from which the conference are to be returned. 
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("type", Converter = typeof(ToLowerConverter), Optional = true)]
        public ConferenceList? List { get; set; }

        /// <summary>
        /// Gets or sets the page of the list that should be returned. 
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("p", Optional = true)]
        public int? Page { get; set; }
    }
}