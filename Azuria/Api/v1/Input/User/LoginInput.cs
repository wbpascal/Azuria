using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.User
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class LoginInput : InputDataModel
    {
        /// <summary>
        /// 
        /// </summary>
        public LoginInput()
        {
        }

        /// <inheritdoc />
        public LoginInput(string username, string password, string secretKey = null)
        {
            this.Username = username;
            this.Password = password;
            this.SecretKey = secretKey;
        }

        /// <summary>
        /// 
        /// </summary>
        [InputData("password")]
        public string Password { get; set; }

        /// <summary>
        /// The 2FA-Key. Optional
        /// </summary>
        [InputData("secretKey", Optional = true)]
        public string SecretKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [InputData("username")]
        public string Username { get; set; }
    }
}