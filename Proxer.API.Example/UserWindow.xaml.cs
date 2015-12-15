using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Proxer.API.Example.Controls;
using Proxer.API.Main;
using Proxer.API.Main.User;
using Proxer.API.Utilities;

namespace Proxer.API.Example
{
    public partial class UserWindow : Window
    {
        private readonly Senpai _senpai;
        private readonly User _user;

        public UserWindow(User user, Senpai senpai)
        {
            if (user == null)
            {
                this.Close();
                return;
            }

            this._user = user;
            this._senpai = senpai;
            this.InitializeComponent();

            //Freundschaftsanfragen können nur gesendet werden, wenn der Benutzer nicht der selbe wie der Senpai ist
            //Es müsste bei einer besseren Implementierung auch noch geprüft werden, ob der Benutzer nicht schon ein Freund ist
            this.SendFriendRequestButton.IsEnabled = this._user.Id != this._senpai.Me.Id;
        }

        #region

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //WICHTIG
            //Wenn der User schon initialisiert ist brauch das nicht noch einmal gemacht zu werden
            //Dies verringert die Belastung auf die Proxer-Server
            //Die Initialisierungs-Methode kann aber auch aufgerufen werden, um die Infos zu aktualisieren
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

            //Schreibe die Infos zu dem User in die zugehörigen Controls
            this.InitComponents();
        }

        private void InitComponents()
        {
            this.Title = "User: " + this._user.UserName;

            //ACHTUNG: Wenn Proxer nicht erreichbar ist kann hier ein Fehler auftreten
            //Wenn der User noch nicht initialisiert ist sollte die Eigenschaft den Standard-Avatar von Proxer zurückgeben
            this.ProfileImage.Source = new BitmapImage(this._user.Avatar);

            this.IdLabel.Content = this._user.Id;
            this.UsernameLabel.Content = this._user.UserName;

            this.OnlineLabel.Content = this._user.Online ? "Online" : "Offline";
            this.OnlineLabel.Foreground =
                new SolidColorBrush(this._user.Online ? Color.FromRgb(79, 222, 43) : Color.FromRgb(222, 43, 43));

            this.InitInfo();
            this.InitFriends();
            this.InitAnime();
            this.InitManga();
        }

        private void InitInfo()
        {
            this.PointsLabel.Content = this._user.Punkte;
            this.RankLabel.Content = this._user.Rang;
            this.StatusBlock.Text = this._user.Status;
            this.InfoBox.Text = this._user.Info;
            this.InfoHtmlBox.Text = this._user.InfoHtml;
        }

        private void InitFriends()
        {
            foreach (User friend in this._user.Freunde)
            {
                //Um den Freund darzustellen benutzte ich einen TextBlock
                //Der DataContext ist nur wichtig, um den Freund in dem MouseLeftButtonUp-Event abzurufen
                TextBlock lFriendBlock = new TextBlock {Text = friend.ToString(), DataContext = friend};
                lFriendBlock.MouseLeftButtonUp += this.FriendBlock_MouseLeftButtonUp;

                this.FriendListBox.Items.Add(lFriendBlock);
            }
        }

        private void InitAnime()
        {
            foreach (Anime favAnime in this._user.FavoritenAnime)
            {
                this.AnimeFavsPanel.Children.Add(new AnimeMangaProgressControl(favAnime));
            }

            foreach (
                KeyValuePair<AnimeMangaProgressObject.AnimeMangaProgress, AnimeMangaProgressObject> anime in
                    this._user.Anime)
            {
                //Hier wird keine Standartmäßiges Control-Element verwendet um den Anime darzustellen
                //Mehr Infos in der Klasse AnimeMangaProgressControl
                AnimeMangaProgressControl lProgressControl = new AnimeMangaProgressControl(anime.Value);

                //Füge AnimeMangaProgressControl dem zugehörigen StackPanel hinzu
                //Variiert je nachdem was anime.Key für einen Wert hat
                switch (anime.Key)
                {
                    //Anime wurde bereits geschaut
                    case AnimeMangaProgressObject.AnimeMangaProgress.Finished:
                        this.AnimeGeschautPanel.Children.Add(lProgressControl);
                        break;
                    //Anime wird gerade geschaut
                    case AnimeMangaProgressObject.AnimeMangaProgress.InProgress:
                        this.AnimeAmSchauenPanel.Children.Add(lProgressControl);
                        break;
                    //Anime wird noch geschaut
                    case AnimeMangaProgressObject.AnimeMangaProgress.Planned:
                        this.AnimeWirdNochGeschautPanel.Children.Add(lProgressControl);
                        break;
                    //Anime wurde abgebrochen
                    case AnimeMangaProgressObject.AnimeMangaProgress.Aborted:
                        this.AnimeAbgebrochenPanel.Children.Add(lProgressControl);
                        break;
                }
            }
        }

        private void InitManga()
        {
            foreach (Manga favManga in this._user.FavoritenManga)
            {
                this.MangaFavsPanel.Children.Add(new AnimeMangaProgressControl(favManga));
            }

            foreach (
                KeyValuePair<AnimeMangaProgressObject.AnimeMangaProgress, AnimeMangaProgressObject> manga in
                    this._user.Manga)
            {
                //Hier wird keine Standartmäßiges Control-Element verwendet um den Manga darzustellen
                //Mehr Infos in der Klasse AnimeMangaProgressControl
                AnimeMangaProgressControl lProgressControl = new AnimeMangaProgressControl(manga.Value);

                //Füge AnimeMangaProgressControl dem zugehörigen StackPanel hinzu
                //Variiert je nachdem was manga.Key für einen Wert hat
                switch (manga.Key)
                {
                    //Manga wurde bereits gelesen
                    case AnimeMangaProgressObject.AnimeMangaProgress.Finished:
                        this.MangaGelesenPanel.Children.Add(lProgressControl);
                        break;
                    //Manga wird gerade gelesen
                    case AnimeMangaProgressObject.AnimeMangaProgress.InProgress:
                        this.MangaAmLesenPanel.Children.Add(lProgressControl);
                        break;
                    //Manga wird noch gelesen
                    case AnimeMangaProgressObject.AnimeMangaProgress.Planned:
                        this.MangaWirdNochGelesenPanel.Children.Add(lProgressControl);
                        break;
                    //Manga wurde abgebrochen
                    case AnimeMangaProgressObject.AnimeMangaProgress.Aborted:
                        this.MangaAbgebrochenPanel.Children.Add(lProgressControl);
                        break;
                }
            }
        }

        private void FriendBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //Rufe den TextBlock aus den Parametern ab
            TextBlock lFriendBlock = sender as TextBlock;
            if (lFriendBlock == null) return;

            //Öffne ein neues UserWindow mit dem DataContext des TextBlock, der den User enthält
            new UserWindow(lFriendBlock.DataContext as User, this._senpai).Show();
        }

        private void OpenWebsiteButton_Click(object sender, RoutedEventArgs e)
        {
            //Öffne den Link zu dem Benutzer in dem Standardbrowser 
            Process.Start("https://proxer.me/user/" + this._user.Id);
        }

        private async void SendFriendRequestButton_Click(object sender, RoutedEventArgs e)
        {
            if ((await this._user.SendFriendRequest()).OnError(false))
                MessageBox.Show("Die Freundschaftsanfrage wurde erfolgreich versendet!");
            else
                MessageBox.Show("Es ist ein Fehler beim Versenden der Freundschaftanfrage aufgetreten!", "Fehler",
                    MessageBoxButton.OK, MessageBoxImage.Error);
        }

        #endregion
    }
}