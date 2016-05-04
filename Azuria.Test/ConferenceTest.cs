﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azuria.Community;
using Azuria.Test.Attributes;
using Azuria.Test.Utility;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace Azuria.Test
{
    [TestFixture, LoginRequired]
    public class ConferenceTest
    {
        private readonly Senpai _senpai = SenpaiTest.Senpai;
        private Conference _conference;

        [Test, Order(2)]
        public async Task BlockTest()
        {
            Assert.IsNotNull(this._conference, "this._conference == null");

            ProxerResult<bool> lIsBlockedResult = await this._conference.IsBlocked.GetObject();
            Assert.IsTrue(lIsBlockedResult.Success);
            ProxerResult<bool> lSetBlockedResult = await this._conference.IsBlocked.SetObject(!lIsBlockedResult.Result);
            Assert.IsTrue(lSetBlockedResult.Success);
            Assert.AreNotEqual(lIsBlockedResult.Result, lSetBlockedResult.Result);

            await Task.Delay(2000);
            lIsBlockedResult = await this._conference.IsBlocked.GetNewObject();
            Assert.IsTrue(lIsBlockedResult.Success);
            Assert.AreEqual(lIsBlockedResult.Result, lSetBlockedResult.Result);

            //Setzte Wert wieder zurück
            await Task.Delay(2000);
            lSetBlockedResult = await this._conference.IsBlocked.SetObject(!lIsBlockedResult.Result);
            Assert.IsTrue(lSetBlockedResult.Success);
            Assert.AreNotEqual(lIsBlockedResult.Result, lSetBlockedResult.Result);
        }

        [Test, Order(2)]
        public async Task FavouriteTest()
        {
            Assert.IsNotNull(this._conference, "this._conference == null");

            ProxerResult<bool> lIsFavouriteResult = await this._conference.IsFavourite.GetObject();
            Assert.IsTrue(lIsFavouriteResult.Success);
            ProxerResult<bool> lSetFavouriteResult =
                await this._conference.IsFavourite.SetObject(!lIsFavouriteResult.Result);
            Assert.IsTrue(lSetFavouriteResult.Success);
            Assert.AreNotEqual(lIsFavouriteResult.Result, lSetFavouriteResult.Result);

            await Task.Delay(2000);
            lIsFavouriteResult = await this._conference.IsFavourite.GetNewObject();
            Assert.IsTrue(lIsFavouriteResult.Success);
            Assert.AreEqual(lIsFavouriteResult.Result, lSetFavouriteResult.Result);

            //Setzte Wert wieder zurück
            await Task.Delay(2000);
            lSetFavouriteResult = await this._conference.IsFavourite.SetObject(!lIsFavouriteResult.Result);
            Assert.IsTrue(lSetFavouriteResult.Success);
            Assert.AreNotEqual(lIsFavouriteResult.Result, lSetFavouriteResult.Result);
        }

        [Test, Order(1)]
        public async Task GetConferenceTest()
        {
            ProxerResult<IEnumerable<Conference>> lConferences = await this._senpai.GetAllConferences();
            Assert.IsTrue(lConferences.Success);
            Assert.IsNotNull(lConferences.Result);
            Assert.IsNotEmpty(lConferences.Result);

            foreach (Conference conference in lConferences.Result)
            {
                PrivateObject lPrivateConference = new PrivateObject(conference);
                InitialisableProperty<bool> lIsConferenceProperty =
                    (InitialisableProperty<bool>) lPrivateConference.GetProperty("IsConference");

                if (!await lIsConferenceProperty.GetObject(false)) continue;
                this._conference = conference;
                Assert.Pass();
            }
            Assert.Fail("Keine Konfernz gefunden! Um erfolgreich zu testen, muss mindestens eine Konferenz" +
                        "vorhanden sein!");
        }

        [Test, Order(2)]
        public async Task LeaderTest()
        {
            Assert.IsNotNull(this._conference, "this._conference == null");

            User lLeader = await this._conference.Leader.GetObject(User.System);
            Assert.AreNotSame(lLeader, User.System);
        }

        [Test, Order(3), NUnit.Framework.Timeout(30000)]
        public async Task MessagesTest()
        {
            Assert.IsNotNull(this._conference, "this._conference == null");

            SemaphoreSlim lSignal = new SemaphoreSlim(0, 2);
            ProxerResult<string> lResult = null;
            string lHexString = RandomUtility.GetRandomHexString();
            this._conference.ErrorDuringPmFetchRaised += (sender, exceptions) =>
            {
                lResult = new ProxerResult<string>("Error") {Success = false};
                lSignal.Release(2);
            };
            this._conference.NeuePmRaised += async (sender, messages, nachrichten) =>
            {
                lResult = new ProxerResult<string>("Neue PM Raised 1") {Success = messages.Any()};
                if (nachrichten)
                {
                    ProxerResult lSendMessageResult = await this._conference.SendMessage(lHexString);
                    if (!lSendMessageResult.Success)
                    {
                        lResult = new ProxerResult<string>("Neue PM Raised 2") {Success = false};
                        lSignal.Release(2);
                    }
                }
                else
                {
                    if (!messages.Last().Content.Equals(lHexString))
                    {
                        lResult = new ProxerResult<string>("Neue PM Raised 3") {Success = false};
                    }
                }
                lSignal.Release();
            };

            this._conference.Active = true;
            await lSignal.WaitAsync();

            Assert.IsNotNull(lResult);
            Assert.IsTrue(lResult.Success, lResult.Result);

            if (this._conference.Active) this._conference.Active = false;
        }

        [Test, Order(2)]
        public async Task ParticipantsTest()
        {
            Assert.IsNotNull(this._conference, "this._conference == null");

            IEnumerable<User> lParticipants = await this._conference.Participants.GetObject(new User[0]);
            Assert.IsNotEmpty(lParticipants, "Participants.Count == 0");
        }

        [Test, Order(2)]
        public async Task TitleTest()
        {
            Assert.IsNotNull(this._conference, "this._conference == null");

            string lRandomHexString = RandomUtility.GetRandomHexString();
            string lTitle = await this._conference.Title.GetObject(lRandomHexString);
            Assert.AreNotSame(lTitle, lRandomHexString, $"Title = WTF-String:{lRandomHexString}");
        }
    }
}