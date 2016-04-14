using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Azuria.Main;
using Azuria.Main.Minor;
using Azuria.Main.User;
using Azuria.Utilities.ErrorHandling;

namespace Azuria.Example
{
    public partial class AnimeMangaWindow : Window
    {
        private readonly IAnimeMangaObject _animeMangaObject;
        private readonly Senpai _senpai;

        public AnimeMangaWindow(IAnimeMangaObject animeMangaObject, Senpai senpai)
        {
            this._animeMangaObject = animeMangaObject;
            this._senpai = senpai;
            this.InitializeComponent();
        }

        #region

        private static string ArrayToString(IEnumerable<GenreObject> array)
        {
            StringBuilder builder = new StringBuilder();
            foreach (GenreObject value in array)
            {
                builder.Append(value.Genre);
                builder.Append(',');
            }
            return builder.ToString();
        }

        private async void EpisodenComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Anime.Episode lEpisode = this.EpisodenComboBox.SelectedItem as Anime.Episode;
            if (lEpisode == null)
            {
                MessageBox.Show("Die Episode konnte nicht abgerufen werden!", "Fehler",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.StreamsStackPanel.Children.Clear();
            if (
                (await
                    lEpisode.Streams.GetObject(
                        new KeyValuePair<Anime.Episode.Stream.StreamPartner, Anime.Episode.Stream>[0])).Any())
                foreach (
                    KeyValuePair<Anime.Episode.Stream.StreamPartner, Anime.Episode.Stream> pair in
                        (await
                            lEpisode.Streams.GetObject(
                                new KeyValuePair<Anime.Episode.Stream.StreamPartner, Anime.Episode.Stream>[0])).ToList()
                    )
                {
                    this.StreamsStackPanel.Children.Add(new Expander
                    {
                        Header = pair.Key,
                        Content = new TextBlock {Text = pair.Value.Link.OriginalString}
                    });
                }
            else
                this.StreamsStackPanel.Children.Add(new TextBlock
                {
                    Text = "Nicht verfügbar!",
                    Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0))
                });
        }

        private async void KapitelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Manga.Chapter lChapter = this.KapitelComboBox.SelectedItem as Manga.Chapter;
            if (lChapter == null)
            {
                MessageBox.Show("Das Kapitel konnte nicht abgerufen werden!", "Fehler",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.KapitelSeitenListView.Children.Clear();
            if (!await lChapter.Available.GetObject(false))
                this.KapitelSeitenListView.Children.Add(new TextBlock
                {
                    Text = "Nicht verfügbar!",
                    Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0))
                });
            this.KapitelSeitenListView.Children.Add(new TextBlock
            {
                Text = "Titel: " + await lChapter.Titel.GetObject("ERROR")
            });
            this.KapitelSeitenListView.Children.Add(new TextBlock
            {
                Text =
                    "Scanlator-Gruppe: " + (await lChapter.ScanlatorGroup.GetObject(Group.Error)).Name + " [ID:" +
                    (await lChapter.ScanlatorGroup.GetObject(Group.Error)).Id + "]"
            });
            this.KapitelSeitenListView.Children.Add(new TextBlock
            {
                Text = "Datum: " + await lChapter.Date.GetObject(DateTime.MinValue)
            });
            this.KapitelSeitenListView.Children.Add(new TextBlock
            {
                Text = "Uploader: " + await lChapter.UploaderName.GetObject("ERROR")
            });

            TextBlock lSeitenLinks = new TextBlock();
            (await lChapter.Pages.GetObject(new Uri[0])).ToList()
                .ForEach(uri => lSeitenLinks.Text += uri.OriginalString + "\n");
            this.KapitelSeitenListView.Children.Add(new Expander {Header = "Seiten", Content = lSeitenLinks});
        }

        private async void LoadCommentsLatest()
        {
            //Gibt die ersten 20 Kommentare(oder weniger, wenn nicht genug vorhanden sind) zurück
            ProxerResult<IEnumerable<Comment>> lCommentsResult = await this._animeMangaObject.GetCommentsLatest(0, 20);

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

                Button lGotoButton = new Button {Content = "Gehe zu Benutzer"};
                lGotoButton.Click += (sender, args) => { new UserWindow(comment.Author, this._senpai).Show(); };

                StackPanel lStackPanel = new StackPanel();
                lStackPanel.Children.Add(lGotoButton);
                lStackPanel.Children.Add(lCommentContent);

                Expander lCommentExpander = new Expander
                {
                    Header = await comment.Author.UserName.GetObject("ERROR") + "(" + comment.Author.Id + ")",
                    Content = lStackPanel
                };

                this.LatestCommentsStackPanel.Children.Add(lCommentExpander);
            }
        }

        private async void LoadCommentsRating()
        {
            //Gibt die ersten 20 Kommentare(oder weniger, wenn nicht genug vorhanden sind) zurück
            ProxerResult<IEnumerable<Comment>> lCommentsResult = await this._animeMangaObject.GetCommentsRating(0, 20);

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

                Button lGotoButton = new Button {Content = "Gehe zu Benutzer"};
                lGotoButton.Click += (sender, args) => { new UserWindow(comment.Author, this._senpai).Show(); };

                StackPanel lStackPanel = new StackPanel();
                lStackPanel.Children.Add(lGotoButton);
                lStackPanel.Children.Add(lCommentContent);

                Expander lCommentExpander = new Expander
                {
                    Header = await comment.Author.UserName.GetObject("ERROR") + "(" + comment.Author.Id + ")",
                    Content = lStackPanel
                };

                this.RatingCommentsStackPanel.Children.Add(lCommentExpander);
            }
        }

        private async void LoadEpisoden()
        {
            this.KapitelTab.Visibility = Visibility.Hidden;
            this.EpisodenComboBox.Items.Clear();

            Anime lAnime = this._animeMangaObject as Anime;
            if (lAnime == null)
            {
                MessageBox.Show("Es ist ein Fehler beim Initialisieren des Objekts aufgetreten!", "Fehler",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }

            //Wird hier nur zu demonstrations Zwecken überprüft, es können vom API aus jede Sprache abgerufen werden, 
            //die auch in der AvailableLanguages-Eigenschaft eingetragen sind
            if (!(await lAnime.AvailableLanguages.GetObject(new Anime.Language[0])).Contains(Anime.Language.EngSub))
                return;

            ProxerResult<IEnumerable<Anime.Episode>> lEpisodenResult = await lAnime.GetEpisodes(Anime.Language.EngSub);
            if (lEpisodenResult.Success)
            {
                lEpisodenResult.Result?.ToList().ForEach(episode => this.EpisodenComboBox.Items.Add(episode));
            }
        }

        private async void LoadInfos()
        {
            try
            {
                //CoverUri wird nur als Uri zurückgegeben, das Bild muss dann noch heruntergeladen werden
                //BitmapImage übernimmt das herunterladen, es muss nur eine Uri angegeben werden
                this.CoverImage.Source = new BitmapImage(this._animeMangaObject.CoverUri);
            }
            catch
            {
                //ignoriert, nur hier in diesem Beispiel, sonst sollte hier etwas stehen
            }

            #region Namen

            this.InfoStackPanel.Children.Add(new Expander
            {
                Content = await this._animeMangaObject.Name.GetObject("ERROR"),
                Header = "Original Titel"
            });
            this.InfoStackPanel.Children.Add(new Expander
            {
                Content = await this._animeMangaObject.EnglishTitle.GetObject("ERROR"),
                Header = "Eng. Titel"
            });
            this.InfoStackPanel.Children.Add(new Expander
            {
                Content = await this._animeMangaObject.GermanTitle.GetObject("ERROR"),
                Header = "Ger. Titel"
            });
            this.InfoStackPanel.Children.Add(new Expander
            {
                Content = await this._animeMangaObject.JapaneseTitle.GetObject("ERROR"),
                Header = "Jap. Titel"
            });
            this.InfoStackPanel.Children.Add(new Expander
            {
                Content = await this._animeMangaObject.Synonym.GetObject("ERROR"),
                Header = "Synonym"
            });

            #endregion

            #region Genre

            this.InfoStackPanel.Children.Add(new Expander
            {
                Content = ArrayToString(await this._animeMangaObject.Genre.GetObject(new GenreObject[0])),
                Header = "Genre"
            });

            #endregion

            #region FSK

            //Hier werden nur die FSK-Infobilder dargestellt, es können aber noch Infos zu den Bilder dargestellt werden (Values)
            //Die Values enthalten jeweils einen kleinen Satz, der das FSK-Bild kurz beschreibt
            StackPanel lFskPanel = new StackPanel {Orientation = Orientation.Horizontal, Height = 80};
            (await this._animeMangaObject.Fsk.GetObject(new Dictionary<Uri, string>())).Keys.ToList()
                .ForEach(uri => lFskPanel.Children.Add(new Image
                {
                    Source = new BitmapImage(uri),
                    Width = 80,
                    Height = 80
                }));
            this.InfoStackPanel.Children.Add(new Expander
            {
                Content = lFskPanel,
                Header = "FSK"
            });

            #endregion

            #region Season

            //Die Größe des IEnumerable-Sammlung gibt auskunft, welche Informationen vorhanden sind
            //Größe 0: Keine Infos
            //Größe 1: Nur die Start Season ist angegeben an dem Index 0
            //Größe 2: Start und End Season sind angegeben, Start Season an dem Index 0 und End Season an dem Index 1
            string[] lSeasonArray = (await this._animeMangaObject.Season.GetObject(new string[0])).ToArray();
            string lSeasons = "";
            if (lSeasonArray.Length > 0) lSeasons += "Start: " + lSeasonArray[0];
            if (lSeasonArray.Length > 1) lSeasons += "\nEnde: " + lSeasonArray[1];
            this.InfoStackPanel.Children.Add(new Expander
            {
                Content = lSeasons,
                Header = "Season"
            });

            #endregion

            #region Status

            this.InfoStackPanel.Children.Add(new Expander
            {
                Content = await this._animeMangaObject.Status.GetObject(AnimeMangaStatus.Unknown),
                Header = "Status"
            });

            #endregion

            #region Gruppen

            string lGruppenString =
                "Proxer.Me bietet keinerlei Downloads an. " +
                "Diesbezüglich leiten wir dich an die betroffenen Sub- und Scanlationgruppen weiter.";
            (await this._animeMangaObject.Groups.GetObject(new Group[0])).ToList()
                .ForEach(x => lGruppenString += "\n" + x.Name + "[ID:" + x.Id + "]");
            this.InfoStackPanel.Children.Add(new Expander
            {
                Content =
                    new TextBlock
                    {
                        Text = lGruppenString,
                        Width = this.InfoStackPanel.ActualWidth - 10,
                        TextWrapping = TextWrapping.WrapWithOverflow
                    },
                Header = "Gruppen"
            });

            #endregion

            #region Industrie

            string lIndustrieString = "";
            (await this._animeMangaObject.Industry.GetObject(new Industry[0])).ToList()
                .ForEach(x => lIndustrieString += x.Name + " (" + x.Type + ") [ID:" + x.Id + "]\n");
            this.InfoStackPanel.Children.Add(new Expander
            {
                Content = lIndustrieString,
                Header = "Industrie"
            });

            #endregion

            #region Lizenz

            this.InfoStackPanel.Children.Add(new Expander
            {
                Content = await this._animeMangaObject.IsLicensed.GetObject(false) ? "Lizensiert!" : "Nicht Lizensiert!",
                Header = "Lizen"
            });

            #endregion

            #region Beschreibung

            this.InfoStackPanel.Children.Add(new Expander
            {
                Content =
                    new TextBlock
                    {
                        Text = await this._animeMangaObject.Description.GetObject("ERROR"),
                        Width = this.InfoStackPanel.ActualWidth - 10,
                        TextWrapping = TextWrapping.WrapWithOverflow
                    },
                Header = "Beschreibung"
            });

            #endregion
        }

        private async void LoadKapitel()
        {
            this.EpisodenTab.Visibility = Visibility.Hidden;

            Manga lManga = this._animeMangaObject as Manga;
            if (lManga == null)
            {
                MessageBox.Show("Es ist ein Fehler beim Initialisieren des Objekts aufgetreten!", "Fehler",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }

            //Wird hier nur zu demonstrations Zwecken überprüft, es können vom API aus jede Sprache abgerufen werden, 
            //die auch in der AvailableLanguages-Eigenschaft eingetragen sind
            if (!(await lManga.AvailableLanguages.GetObject(new Language[0])).Contains(Main.Minor.Language.English))
                return;

            ProxerResult<IEnumerable<Manga.Chapter>> lKapitelResult =
                await lManga.GetChapters(Main.Minor.Language.English);
            if (lKapitelResult.Success)
            {
                lKapitelResult.Result?.ToList().ForEach(kapitel => this.KapitelComboBox.Items.Add(kapitel));
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Wenn das AnimeMangaObject null ist, dann muss ein Fehler bei der Abfrage passiert seien
            //Wird hier verwendet da bei der Abfrage ein .OnError(null) angehängt wurde (siehe LoginWindow)
            if (this._animeMangaObject == null)
            {
                MessageBox.Show("Es ist ein Fehler bei der Abfrage des Anime/Manga aufgetreten!", "Fehler",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }

            this.LoadInfos();
            this.LoadCommentsLatest();
            this.LoadCommentsRating();

            switch (this._animeMangaObject.ObjectType)
            {
                case AnimeMangaType.Anime:
                    this.LoadEpisoden();
                    break;
                case AnimeMangaType.Manga:
                    this.LoadKapitel();
                    break;
            }
        }

        #endregion
    }
}