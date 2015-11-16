using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Proxer.API.Utilities;

namespace Proxer.API.Example
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private readonly User _user;

        public UserWindow(User user)
        {
            this._user = user;
            this.InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!this._user.IstInitialisiert)
            {
                ProxerResult lInitResult = await this._user.Init();
                if (!lInitResult.Success)
                {
                    MessageBox.Show("Es ist ein Fehler beim Abrufen des Benutzers aufgetreten!", "Fehler",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                    return;
                }
            }

            this.InitComponents();
        }

        private void InitComponents()
        {
            this.Title = "User: " + this._user.UserName;

            this.ProfileImage.Source = new BitmapImage(this._user.Avatar);

            this.IdLabel.Content = this._user.Id;
            this.UsernameLabel.Content = this._user.UserName;

            this.OnlineLabel.Content = this._user.Online ? "Online" : "Offline";
            this.OnlineLabel.Foreground =
                new SolidColorBrush(this._user.Online ? Color.FromRgb(79, 222, 43) : Color.FromRgb(222, 43, 43));

            this.InitInfo();
        }

        private void InitInfo()
        {
            this.PointsLabel.Content = this._user.Punkte;
            this.RankLabel.Content = this._user.Rang;
            this.StatusBlock.Text = this._user.Status;
            this.InfoBox.Text = this._user.Info;
            this.InfoHtmlBox.Text = this._user.InfoHtml;
        }
    }
}
