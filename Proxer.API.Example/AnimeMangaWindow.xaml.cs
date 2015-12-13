using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Proxer.API.Main;

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
    }
}