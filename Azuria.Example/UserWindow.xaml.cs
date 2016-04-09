using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Azuria.Example.Controls;
using Azuria.Main;
using Azuria.Main.Minor;
using Azuria.Main.User;
using Azuria.Utilities.ErrorHandling;

namespace Azuria.Example
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
            this.SendFriendRequestButton.IsEnabled = this._user.Id != this._senpai.Me?.Id;
        }

        #region

        private void FriendBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TextBlock lFriendBlock = sender as TextBlock;
            if (lFriendBlock == null) return;

            new UserWindow(lFriendBlock.DataContext as User, this._senpai).Show();
        }

        private async Task InitAnime()
        {
            foreach (Anime favAnime in await this._user.FavouriteAnime.GetObject(new Anime[0]))
            {
                this.AnimeFavsPanel.Children.Add(new AnimeMangaProgressControl(favAnime, this._senpai));
            }

            foreach (
                KeyValuePair<AnimeMangaProgress, AnimeMangaProgressObject<Anime>> anime in
                    await
                        this._user.Anime.GetObject(
                            new KeyValuePair<AnimeMangaProgress, AnimeMangaProgressObject<Anime>>[0]))
            {
                AnimeMangaProgressControl lProgressControl =
                    new AnimeMangaProgressControl(
                        new AnimeMangaProgressObject<IAnimeMangaObject>(anime.Value.User, anime.Value.AnimeMangaObject,
                            anime.Value.EntryId, anime.Value.CurrentProgress, anime.Value.MaxCount, anime.Value.Progress,
                            this._senpai), this._senpai);

                switch (anime.Key)
                {
                    //Anime wurde bereits geschaut
                    case AnimeMangaProgress.Finished:
                        this.AnimeGeschautPanel.Children.Add(lProgressControl);
                        break;
                    //Anime wird gerade geschaut
                    case AnimeMangaProgress.InProgress:
                        this.AnimeAmSchauenPanel.Children.Add(lProgressControl);
                        break;
                    //Anime wird noch geschaut
                    case AnimeMangaProgress.Planned:
                        this.AnimeWirdNochGeschautPanel.Children.Add(lProgressControl);
                        break;
                    //Anime wurde abgebrochen
                    case AnimeMangaProgress.Aborted:
                        this.AnimeAbgebrochenPanel.Children.Add(lProgressControl);
                        break;
                }
            }
        }

        private async void InitComments()
        {
            //Gibt die ersten 10 Kommentare(oder weniger, wenn nicht genug vorhanden sind) zurück
            ProxerResult<IEnumerable<Comment>> lCommentsResult = await this._user.GetComments(0, 10);

            if (!lCommentsResult.Success) return;

            foreach (Comment comment in lCommentsResult.Result ?? new Comment[0])
            {
                TextBlock lCommentContent = new TextBlock
                {
                    TextWrapping = TextWrapping.Wrap,
                    Text = "Gesamtwertung: " + comment.Stars + "\n\n" + comment.Content
                };
                comment.CategoryStars.ToList()
                    .ForEach(pair => lCommentContent.Text = pair.Key + ": " + pair.Value + "\n" + lCommentContent.Text);

                Button lGotoButton = new Button {Content = "Öffne Anime/Manga"};
                lGotoButton.Click += async (sender, args) =>
                {
                    ProxerResult<IAnimeMangaObject> lResult =
                        await ProxerClass.GetAnimeMangaById(comment.AnimeMangaId, this._senpai);

                    if (!lResult.Success)
                        MessageBox.Show("Es ist ein Fehler beim Abrufen des Anime/Manga aufgetreten!");

                    new AnimeMangaWindow(lResult.Result, this._senpai).Show();
                };

                StackPanel lStackPanel = new StackPanel();
                lStackPanel.Children.Add(lGotoButton);
                lStackPanel.Children.Add(lCommentContent);

                Expander lCommentExpander = new Expander {Header = comment.AnimeMangaId, Content = lStackPanel};

                this.CommentStackPanel.Children.Add(lCommentExpander);
            }
        }

        private async Task InitComponents()
        {
            this.Title = "User: " + await this._user.UserName.GetObject("ERROR");

            //ACHTUNG: Wenn Proxer nicht erreichbar ist kann hier ein Fehler auftreten
            //Wenn der User noch nicht initialisiert ist sollte die Eigenschaft den Standard-Avatar von Proxer zurückgeben
            this.ProfileImage.Source =
                new BitmapImage(await this._user.Avatar.GetObject(new Uri("https://cdn.proxer.me/avatar/nophoto.png")));

            this.IdLabel.Content = this._user.Id;
            this.UsernameLabel.Content = await this._user.UserName.GetObject("ERROR");

            this.OnlineLabel.Content = await this._user.IsOnline.GetObject(false) ? "Online" : "Offline";
            this.OnlineLabel.Foreground =
                new SolidColorBrush(await this._user.IsOnline.GetObject(false)
                    ? Color.FromRgb(79, 222, 43)
                    : Color.FromRgb(222, 43, 43));

            await this.InitInfo();
            await this.InitFriends();
            await this.InitAnime();
            await this.InitManga();
            this.InitComments();
        }

        private async Task InitFriends()
        {
            foreach (User friend in await this._user.Friends.GetObject(new User[0]))
            {
                TextBlock lFriendBlock = new TextBlock {Text = friend.ToString(), DataContext = friend};
                lFriendBlock.MouseLeftButtonUp += this.FriendBlock_MouseLeftButtonUp;

                this.FriendListBox.Items.Add(lFriendBlock);
            }
        }

        private async Task InitInfo()
        {
            this.PointsLabel.Content = await this._user.Points.GetObject(-1);
            this.RankLabel.Content = await this._user.Ranking.GetObject("ERROR");
            this.StatusBlock.Text = await this._user.Status.GetObject("ERROR");
            this.InfoBox.Text = await this._user.Info.GetObject("ERROR");
            this.InfoHtmlBox.Text = await this._user.InfoHtml.GetObject("ERROR");
        }

        private async Task InitManga()
        {
            foreach (Manga favManga in await this._user.FavouriteManga.GetObject(new Manga[0]))
            {
                this.MangaFavsPanel.Children.Add(new AnimeMangaProgressControl(favManga, this._senpai));
            }

            foreach (
                KeyValuePair<AnimeMangaProgress, AnimeMangaProgressObject<Manga>> manga in
                    await
                        this._user.Manga.GetObject(
                            new KeyValuePair<AnimeMangaProgress, AnimeMangaProgressObject<Manga>>[0]))
            {
                AnimeMangaProgressControl lProgressControl =
                    new AnimeMangaProgressControl(new AnimeMangaProgressObject<IAnimeMangaObject>(manga.Value.User,
                        manga.Value.AnimeMangaObject,
                        manga.Value.EntryId, manga.Value.CurrentProgress, manga.Value.MaxCount, manga.Value.Progress,
                        this._senpai), this._senpai);

                switch (manga.Key)
                {
                    //Manga wurde bereits gelesen
                    case AnimeMangaProgress.Finished:
                        this.MangaGelesenPanel.Children.Add(lProgressControl);
                        break;
                    //Manga wird gerade gelesen
                    case AnimeMangaProgress.InProgress:
                        this.MangaAmLesenPanel.Children.Add(lProgressControl);
                        break;
                    //Manga wird noch gelesen
                    case AnimeMangaProgress.Planned:
                        this.MangaWirdNochGelesenPanel.Children.Add(lProgressControl);
                        break;
                    //Manga wurde abgebrochen
                    case AnimeMangaProgress.Aborted:
                        this.MangaAbgebrochenPanel.Children.Add(lProgressControl);
                        break;
                }
            }
        }

        private void OpenWebsiteButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://proxer.me/user/" + this._user.Id);
        }

        private async void SendFriendRequestButton_Click(object sender, RoutedEventArgs e)
        {
            if ((await this._user.SendFriendRequest()).Success)
                MessageBox.Show("Die Freundschaftsanfrage wurde erfolgreich versendet!");
            else
                MessageBox.Show("Es ist ein Fehler beim Versenden der Freundschaftanfrage aufgetreten!", "Fehler",
                    MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await this.InitComponents();
        }

        #endregion
    }
}