﻿using System.Collections;
using System.Collections.Generic;
using Azuria.User.Comment;

namespace Azuria.AnimeManga.Properties
{
    /// <summary>
    /// </summary>
    public class CommentEnumerable<T> : IEnumerable<Comment<T>> where T : IAnimeMangaObject
    {
        private readonly T _animeMangaObject;
        private readonly Senpai _senpai;
        private readonly string _sort;

        internal CommentEnumerable(T animeMangaObject, string sort, Senpai senpai)
        {
            this._animeMangaObject = animeMangaObject;
            this._sort = sort;
            this._senpai = senpai;
        }

        #region Inherited

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<Comment<T>> GetEnumerator()
        {
            return new CommentEnumerator<T>(this._animeMangaObject, this._sort, this._senpai);
        }

        #endregion
    }
}