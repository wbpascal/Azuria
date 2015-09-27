using System.Windows;
using System.Windows.Controls;

namespace Proxer.API.Example
{
    /// <summary>
    ///     Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly Senpai _senpai;

        /// <summary>
        /// </summary>
        public LoginWindow()
        {
            this._senpai = new Senpai();

            this.InitializeComponent();
        }

        #region

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.TextBox1.Text) || string.IsNullOrEmpty(this.PasswordBox1.Password))
            {
                MessageBox.Show("Bitte gib ein Benutzernamen und ein Password ein!");
                return;
            }

            (sender as Button).IsEnabled = false;

            if (await this._senpai.Login(this.TextBox1.Text, this.PasswordBox1.Password))
                MessageBox.Show("Du wurdest erfolgreich eingeloggt!");
            else
                MessageBox.Show("Die Benutzername/Passwort-Kombination konnte nicht erkannt werden!");

            (sender as Button).IsEnabled = true;
        }

        #endregion
    }
}