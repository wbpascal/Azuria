using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.List
{
    /// <summary>
    /// 
    /// </summary>
    public class TagDataModel : IDataModel
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("tag")]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
