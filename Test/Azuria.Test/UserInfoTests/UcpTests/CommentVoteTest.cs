using System.Linq;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.UserInfo.ControlPanel;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Azuria.Test.UserInfoTests.UcpTests
{
    [TestFixture]
    public class CommentVoteTest
    {
        private CommentVote _vote;
        private UserControlPanel _controlPanel;

        [OneTimeSetUp]
        public async Task Setup()
        {
            this._controlPanel = new UserControlPanel(GeneralSetup.SenpaiInstance);
            this._vote = (await this._controlPanel.CommentVotes.ThrowFirstOnNonSuccess()).FirstOrDefault();
            Assert.IsNotNull(this._vote);
        }

        [Test]
        public void AnimeMangaNameTest()
        {
            Assert.AreEqual("Anime Name", this._vote.AnimeMangaName);
        }

        [Test]
        public void AuthorTest()
        {
            Assert.AreEqual(163825, this._vote.Author.Id);
            Assert.AreEqual("KutoSan", this._vote.Author.UserName.GetObjectIfInitialised(string.Empty));
        }

        [Test]
        public void CommentContentTest()
        {
            Assert.AreEqual("Comment Content", this._vote.CommentContent);
        }

        [Test]
        public void CommentIdTest()
        {
            Assert.AreEqual(1, this._vote.CommentId);
        }

        [Test]
        public async Task DeleteVoteTest()
        {
            ProxerResult lResult = await this._vote.DeleteVote();
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
        }

        [Test]
        public void RatingTest()
        {
            Assert.AreEqual(9, this._vote.Rating);
        }

        [Test]
        public void UserControlPanelTest()
        {
            Assert.AreSame(this._controlPanel, this._vote.UserControlPanel);
        }

        [Test]
        public void VoteIdTest()
        {
            Assert.AreEqual(1, this._vote.VoteId);
        }
    }
}