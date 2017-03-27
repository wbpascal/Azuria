using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azuria.Api.v1.DataModels.List;
using Azuria.Media;

namespace Azuria.Info
{
    /// <summary>
    /// 
    /// </summary>
    public class TranslatorProject
    {
        internal TranslatorProject(TranslatorProjectDataModel dataModel)
        {
            this.TranslationStatus = dataModel.Status;
        }

        /// <summary>
        /// 
        /// </summary>
        public TranslationStatus TranslationStatus { get; }

        /// <summary>
        /// 
        /// </summary>
        public IMediaObject Entry { get; }
    }
}
