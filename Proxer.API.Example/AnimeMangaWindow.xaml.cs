using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Proxer.API.Main;
using Proxer.API.Utilities;

namespace Proxer.API.Example
{
    /// <summary>
    ///     Interaction logic for AnimeMangaWindow.xaml
    /// </summary>
    public partial class AnimeMangaWindow : Window
    {
        private readonly IAnimeMangaObject _animeMangaObject;

        public AnimeMangaWindow(IAnimeMangaObject animeMangaObject)
        {
            this._animeMangaObject = animeMangaObject;
            this.InitializeComponent();
        }

        #region

        private async void Window_Loaded(object sender, RoutedEventArgs e)
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

            //Empfohlen: Überprüfen, ob das Objekt bereits initialisiert ist
            if (!this._animeMangaObject.IstInitialisiert)
                if (!(await this._animeMangaObject.Init()).Success)
                {
                    MessageBox.Show("Es ist ein Fehler beim Initialisieren des Objekts aufgetreten!", "Fehler",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                    return;
                }

            this.LoadInfos();

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

        private void LoadKapitel()
        {
            this.EpisodenTab.Visibility = Visibility.Hidden;

            Manga lManga = (this._animeMangaObject as Manga);
            if (lManga == null)
            {
                MessageBox.Show("Es ist ein Fehler beim Initialisieren des Objekts aufgetreten!", "Fehler",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }

            //Wird hier nur zu demonstrations Zwecken überprüft, es können vom API aus jede Sprache abgerufen werden, 
            //die auch in der Sprachen-Eigenschaft eingetragen sind
            if (!lManga.Sprachen.Contains(Manga.Language.English)) return;

            ProxerResult<IEnumerable<Manga.Chapter>> lKapitelResult = lManga.GetChapters(Manga.Language.English);
            if (lKapitelResult.Success)
            {
                lKapitelResult.Result.ToList().ForEach(kapitel => this.KapitelComboBox.Items.Add(kapitel));
            }
        }

        private void LoadEpisoden()
        {
            this.KapitelTab.Visibility = Visibility.Hidden;
            this.EpisodenComboBox.Items.Clear();

            Anime lAnime = (this._animeMangaObject as Anime);
            if (lAnime == null)
            {
                MessageBox.Show("Es ist ein Fehler beim Initialisieren des Objekts aufgetreten!", "Fehler",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }

            //Wird hier nur zu demonstrations Zwecken überprüft, es können vom API aus jede Sprache abgerufen werden, 
            //die auch in der Sprachen-Eigenschaft eingetragen sind
            if (!lAnime.Sprachen.Contains(Anime.Language.EngSub)) return;

            ProxerResult<IEnumerable<Anime.Episode>> lEpisodenResult = lAnime.GetEpisodes(Anime.Language.EngSub);
            if (lEpisodenResult.Success)
            {
                lEpisodenResult.Result.ToList().ForEach(episode => this.EpisodenComboBox.Items.Add(episode));
            }
        }

        private void LoadInfos()
        {
            try
            {
                //CoverUri wird nur als Uri zurückgegeben, das Bild muss dann noch heruntergeladen werden
                //BitmapImage übernimmt das herunterladen, es muss nur eine Uri angegeben werden
                this.CoverImage.Source = new BitmapImage(this._animeMangaObject.CoverUri);
            }
            catch
            {
                //ignoriert
            }

            //Um die Informationen alle übersichtlich darzustellen, werden sie in Expander gesteckt, die dann
            //dem StackPanel hinzugefügt werden, das die Expander automatisch anordnet.

            #region Namen

            this.InfoStackPanel.Children.Add(new Expander
            {
                Content = this._animeMangaObject.Name,
                Header = "Original Titel"
            });
            this.InfoStackPanel.Children.Add(new Expander
            {
                Content = this._animeMangaObject.EnglischTitel,
                Header = "Eng. Titel"
            });
            this.InfoStackPanel.Children.Add(new Expander
            {
                Content = this._animeMangaObject.DeutschTitel,
                Header = "Ger. Titel"
            });
            this.InfoStackPanel.Children.Add(new Expander
            {
                Content = this._animeMangaObject.JapanTitel,
                Header = "Jap. Titel"
            });
            this.InfoStackPanel.Children.Add(new Expander
            {
                Content = this._animeMangaObject.Synonym,
                Header = "Synonym"
            });

            #endregion

            #region Genre

            this.InfoStackPanel.Children.Add(new Expander
            {
                Content = ArrayToString(this._animeMangaObject.Genre),
                Header = "Genre"
            });

            #endregion

            #region FSK

            //Hier werden nur die FSK-Infobilder dargestellt, es können aber noch Infos zu den Bilder dargestellt werden (Values)
            StackPanel lFskPanel = new StackPanel {Orientation = Orientation.Horizontal, Height = 80};
            this._animeMangaObject.Fsk.Keys.ToList()
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
            string[] lSeasonArray = this._animeMangaObject.Season.ToArray();
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
                Content = this._animeMangaObject.Status,
                Header = "Status"
            });

            #endregion

            #region Gruppen

            string lGruppenString =
                "Proxer.Me bietet keinerlei Downloads an. " +
                "Diesbezüglich leiten wir dich an die betroffenen Sub- und Scanlationgruppen weiter.";
            this._animeMangaObject.Gruppen.ToList().ForEach(x => lGruppenString += "\n" + x.Name + "[ID:" + x.Id + "]");
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
            this._animeMangaObject.Industrie.ToList()
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
                Content = this._animeMangaObject.Lizensiert ? "Lizensiert!" : "Nicht Lizensiert!",
                Header = "Lizen"
            });

            #endregion

            #region Beschreibung

            this.InfoStackPanel.Children.Add(new Expander
            {
                Content =
                    new TextBlock
                    {
                        Text = this._animeMangaObject.Beschreibung,
                        Width = this.InfoStackPanel.ActualWidth - 10,
                        TextWrapping = TextWrapping.WrapWithOverflow
                    },
                Header = "Beschreibung"
            });

            #endregion
        }

        private static string ArrayToString(IEnumerable<string> array)
        {
            //string wird mit Komma getrennt aneinander gereiht
            StringBuilder builder = new StringBuilder();
            foreach (string value in array)
            {
                builder.Append(value);
                builder.Append(',');
            }
            return builder.ToString();
        }

        #endregion

        private async void EpisodenComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Anime.Episode lEpisode = this.EpisodenComboBox.SelectedItem as Anime.Episode;
            if (lEpisode == null)
            {
                MessageBox.Show("Die Episode konnte nicht abgerufen werden!", "Fehler",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!lEpisode.IstInitialisiert)
            {
                if (!(await lEpisode.Init()).Success)
                {
                    MessageBox.Show("Es ist ein Fehler beim Initialisieren der Episode aufgetreten!", "Fehler",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            this.StreamsStackPanel.Children.Clear();
            if (lEpisode.Streams.Any())
                lEpisode.Streams.ToList()
                        .ForEach(
                            pair =>
                                this.StreamsStackPanel.Children.Add(new Expander
                                {
                                    Header = pair.Key,
                                    Content = new TextBlock {Text = pair.Value.Link.OriginalString}
                                }));
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

            if (!lChapter.IstInitialisiert)
            {
                if (!(await lChapter.Init()).Success)
                {
                    MessageBox.Show("Es ist ein Fehler beim Initialisieren des Kapitels aufgetreten!", "Fehler",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            this.KapitelSeitenListView.Children.Clear();
            if (!lChapter.Verfuegbar)
                this.KapitelSeitenListView.Children.Add(new TextBlock
                {
                    Text = "Nicht verfügbar!",
                    Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0))
                });
            this.KapitelSeitenListView.Children.Add(new TextBlock {Text = "Titel: " + lChapter.Titel});
            this.KapitelSeitenListView.Children.Add(new TextBlock
            {
                Text =
                    "Scanlator-Gruppe: " + lChapter.ScanlatorGruppe.Name + " [ID:" + lChapter.ScanlatorGruppe.Id + "]"
            });
            this.KapitelSeitenListView.Children.Add(new TextBlock { Text = "Datum: " + lChapter.Datum });
            this.KapitelSeitenListView.Children.Add(new TextBlock { Text = "Uploader: " + lChapter.UploaderName });

            TextBlock lSeitenLinks = new TextBlock();
            lChapter.Seiten.ToList().ForEach(uri => lSeitenLinks.Text += uri.OriginalString + "\n");
            this.KapitelSeitenListView.Children.Add(new Expander {Header = "Seiten", Content = lSeitenLinks});
        }
    }
}