using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Azuria.Helpers;

namespace Azuria.Api.v1.DataModels
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DataModelBase : IDataModel, IEquatable<DataModelBase>
    {
        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType()) return false;
            return ReferenceEquals(this, obj) || this.Equals((DataModelBase) obj);
        }

        /// <inheritdoc />
        public virtual bool Equals(DataModelBase other)
        {
            return this.GetType().GetRuntimeProperties().All(info => info.ArePropertyValuesEqual(this, other));
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return this.GetType().GetRuntimeProperties().Aggregate(
                    0, (i, info) => (i * 397) ^ info.GetValue(this).GetHashCode()
                );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(DataModelBase left, DataModelBase right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(DataModelBase left, DataModelBase right)
        {
            return !Equals(left, right);
        }
    }
}