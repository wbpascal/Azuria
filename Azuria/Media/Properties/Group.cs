﻿using Azuria.Api.v1.DataModels.Info;

namespace Azuria.Media.Properties
{
    /// <summary>
    ///     Represents a class which describes a <see cref="Group" />, who translates <see cref="Anime" /> or
    ///     <see cref="Manga" />.
    /// </summary>
    public class Group
    {
        /// <summary>
        ///     Represents an error.
        /// </summary>
        public static Group Error = new Group(new GroupDataModel {Id = -1, Name = "ERROR"});

        internal Group(GroupDataModel dataModel)
        {
            this.Id = dataModel.Id;
            this.Name = dataModel.Name;
            this.Language = dataModel.Language;
        }

        #region Properties

        /// <summary>
        ///     Gets the id of the <see cref="Group" />.
        /// </summary>
        public int Id { get; }

        /// <summary>
        ///     Gets the Language of the <see cref="Group" />.
        /// </summary>
        public Language Language { get; internal set; }

        /// <summary>
        ///     Gets the name of the <see cref="Group" />.
        /// </summary>
        public string Name { get; }

        #endregion
    }
}