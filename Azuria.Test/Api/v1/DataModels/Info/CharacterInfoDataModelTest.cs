using System;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Enums.Info;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.Info
{
    [TestFixture]
    public class CharacterInfoDataModelTest : DataModelsTestBase<CharacterInfoDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["info_getcharacter.json"];
            ProxerApiResponse<CharacterInfoDataModel> lResponse = this.Convert(lJson);
            Assert.AreEqual(BuildDataModel(), lResponse.Result);
        }

        public static CharacterInfoDataModel BuildDataModel()
        {
            return new CharacterInfoDataModel
            {
                Birthday = new DateTime(1968, 7, 15),
                Bloodtype = "0",
                Description = new[]
                {
                    new DescriptionDataModel
                    {
                        Language = Language.German,
                        Subject = "intro",
                        Text = "intro text"
                    },
                    new DescriptionDataModel
                    {
                        Language = Language.English,
                        Subject = "appearance",
                        Text = "appearance text in english"
                    }
                },
                EyeColorHex = "#5c7582",
                Gender = CharacterGender.Female,
                HairColorHex = "#717582",
                Height = 420,
                Id = 62,
                Links = new[]
                {
                    new CharacterInfoDataModel.LinkDataModel
                    {
                        EntryId = 9839,
                        EntryName = "Girls und Panzer der Film",
                        Role = CharacterRole.Support
                    }
                },
                Name = "Rumi",
                Names = new[]
                {
                    new NameDataModel
                    {
                        Alternative = "\u30eb\u30df",
                        IsDisplayName = true,
                        Language = Language.Japanese,
                        Name = "Rumi"
                    },
                    new NameDataModel
                    {
                        Alternative = "DERumi",
                        IsDisplayName = false,
                        Language = Language.German,
                        Name = "RumiDE"
                    },
                    new NameDataModel
                    {
                        Alternative = "ENRumi",
                        IsDisplayName = false,
                        Language = Language.English,
                        Name = "RumiEN"
                    }
                },
                Persons = new[]
                {
                    new CharacterInfoDataModel.PersonDataModel
                    {
                        Id = 114,
                        Language = Language.Japanese,
                        Name = "Mai Nakahara",
                        Type = PersonType.Seiyuu
                    }
                },
                Weight = null
            };
        }
    }
}