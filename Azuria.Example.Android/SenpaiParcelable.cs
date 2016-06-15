using System;
using System.Collections.Generic;
using System.Net;
using Android.OS;
using Java.Interop;
using Object = Java.Lang.Object;

namespace Azuria.Example.Android
{
    public class SenpaiParcelable : Object, IParcelable
    {
        [ExportField("CREATOR")]
        public static SenpaiParcelableCreator InititalizeCreator()
        {
            return new SenpaiParcelableCreator();
        }

        public SenpaiParcelable(Senpai senpai)
        {
            this.Senpai = senpai;
        }

        public SenpaiParcelable(SenpaiForParcelable senpai)
        {
            this.Senpai = senpai;
        }

        public Senpai Senpai { get; }

        /// <summary>Describe the kinds of special objects contained in this Parcelable's
        /// marshalled representation.</summary>
        /// <returns>To be added.</returns>
        /// <remarks>
        ///                <para tool="javadoc-to-mdoc">Describe the kinds of special objects contained in this Parcelable's
        /// marshalled representation.</para>
        ///                <para tool="javadoc-to-mdoc">
        ///                    <format type="text/html">
        ///                        <a href="http://developer.android.com/reference/android/os/Parcelable.html#describeContents()" target="_blank">[Android Documentation]</a>
        ///                    </format>
        ///                </para>
        ///            </remarks>
        /// <since version="Added in API level 1" />
        public int DescribeContents()
        {
            return 0;
        }

        /// <param name="dest">The Parcel in which the object should be written.</param>
        /// <param name="flags">Additional flags about how the object should be written.
        ///  May be 0 or <c><see cref="F:Android.OS.Parcelable.ParcelableWriteReturnValue" /></c>.
        /// </param>
        /// <summary>Flatten this object in to a Parcel.</summary>
        /// <remarks>
        ///     <para tool="javadoc-to-mdoc">Flatten this object in to a Parcel.</para>
        ///     <para tool="javadoc-to-mdoc">
        ///         <format type="text/html">
        ///             <a href="http://developer.android.com/reference/android/os/Parcelable.html#writeToParcel(android.os.Parcel, int)" target="_blank">[Android Documentation]</a>
        ///         </format>
        ///     </para>
        /// </remarks>
        /// <since version="Added in API level 1" />
        public void WriteToParcel(Parcel dest, ParcelableWriteFlags flags)
        {
            dest.WriteInt(this.Senpai.IsLoggedIn ? 1 : 0);
            List<string> lCookies = new List<string>();
            foreach (Cookie cookie in this.Senpai.LoginCookies.GetCookies(new Uri("https://proxer.me")))
            {
                lCookies.Add($"{cookie.Name}={cookie.Value}");
            } 
            dest.WriteStringArray(lCookies.ToArray());
            dest.WriteInt(this.Senpai.Me?.Id ?? -1);
        }
    }

    public class SenpaiParcelableCreator : Object, IParcelableCreator
    {
        /// <param name="source">The Parcel to read the object's data from.</param>
        /// <summary>Create a new instance of the Parcelable class, instantiating it
        /// from the given Parcel whose data had previously been written by
        /// <c><see cref="!:Android.OS.Parcelable.writeToParcel(android.os.Parcel, int)" /></c>.</summary>
        /// <returns>To be added.</returns>
        /// <remarks>
        ///                <para tool="javadoc-to-mdoc">Create a new instance of the Parcelable class, instantiating it
        /// from the given Parcel whose data had previously been written by
        /// <c><see cref="!:Android.OS.Parcelable.writeToParcel(android.os.Parcel, int)" /></c>.</para>
        ///                <para tool="javadoc-to-mdoc">
        ///                    <format type="text/html">
        ///                        <a href="http://developer.android.com/reference/android/os/Parcelable.Creator.html#createFromParcel(android.os.Parcel)" target="_blank">[Android Documentation]</a>
        ///                    </format>
        ///                </para>
        ///            </remarks>
        /// <since version="Added in API level 1" />
        public Object CreateFromParcel(Parcel source)
        {
            bool lIsLoggedIn = source.ReadInt() == 1;
            string[] lCookies = source.CreateStringArray();
            int lUserId = source.ReadInt();
            return new SenpaiParcelable(new SenpaiForParcelable(lIsLoggedIn, GetCookieContainer(lCookies), lUserId));
        }

        private static CookieContainer GetCookieContainer(IEnumerable<string> cookies)
        {
            CookieContainer lCookies = new CookieContainer();
            foreach (string cookie in cookies)
            {
                string[] lCookieInformation = cookie.Split('=');
                lCookies.Add(new Cookie(lCookieInformation[0], lCookieInformation[1], "/", "proxer.me"));
            }
            return lCookies;
        }

        /// <param name="size">Size of the array.</param>
        /// <summary>Create a new array of the Parcelable class.</summary>
        /// <returns>To be added.</returns>
        /// <remarks>
        ///     <para tool="javadoc-to-mdoc">Create a new array of the Parcelable class.</para>
        ///     <para tool="javadoc-to-mdoc">
        ///         <format type="text/html">
        ///             <a href="http://developer.android.com/reference/android/os/Parcelable.Creator.html#newArray(int)" target="_blank">[Android Documentation]</a>
        ///         </format>
        ///     </para>
        /// </remarks>
        /// <since version="Added in API level 1" />
        public Object[] NewArray(int size)
        {
            return new Object[size];
        }
    }

    public class SenpaiForParcelable : Senpai
    {
        public SenpaiForParcelable(bool isLoggedIn, CookieContainer lCookies, int meId) : base()
        {
            this.IsLoggedIn = isLoggedIn;
            this.LoginCookies = lCookies;
            this.Me = new User(meId, this);
        }
    }
}