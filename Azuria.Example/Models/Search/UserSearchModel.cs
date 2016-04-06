using System;
using System.Threading.Tasks;

namespace Azuria.Example.Models.Search
{
    public class UserSearchModel
    {
        public UserSearchModel(User user)
        {
            this.UserObject = user;
        }

        #region Properties

        public Uri Avatar { get; private set; }

        public string UserName { get; private set; }
        public User UserObject { get; }

        #endregion

        #region

        public async Task<UserSearchModel> InitProperties()
        {
            this.Avatar = await this.UserObject.Avatar.GetObject(new Uri("https://cdn.proxer.me/avatar/nophoto.png"));
            this.UserName = await this.UserObject.UserName.GetObject("ERROR");

            return this;
        }

        #endregion
    }
}