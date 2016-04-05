using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Azuria.Community;

namespace Azuria.Example
{
    public partial class ConferenceWindow : Window
    {
        private readonly Conference _conference;
        private readonly Senpai _senpai;

        public ConferenceWindow(Conference conference, Senpai senpai)
        {
            this._conference = conference;
            this._senpai = senpai;
            this.InitializeComponent();

            this._conference.ErrorDuringPmFetchRaised += this.ConferenceOnErrorDuringPmFetchRaised;
            this._conference.NeuePmRaised += this.ConferenceOnNeuePmRaised;
        }

        #region

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //Versuche die Nachricht zu senden
            if (!(await this._conference.SendMessage(this.InputBox.Text)).Success)
            {
                //Falls ein Fehler beim Senden der Nachricht aufgetreten ist
                MessageBox.Show("Die Nachricht konnte nicht gesendet werden!", "Fehler",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                this.InputBox.Clear();
            }
        }

        private void ConferenceOnErrorDuringPmFetchRaised(Conference sender, IEnumerable<Exception> exceptions)
        {
            //Falls beim Abrufen der Nachrichten im Hintergrund ein Fehler auftritt wird dieses Event ausgelöst
            MessageBox.Show("Es ist ein Fehler beim Abrufen der Nachrichten aufgetreten!", "Fehler", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        private async void ConferenceOnNeuePmRaised(Conference sender, IEnumerable<Conference.Message> messages,
            bool alleNachrichten)
        {
            //Hier werden die Nachrichten verarbeitet

            //WICHTIG, wenn auf Elemente des Fensters zugegriffen werden will, 
            //da das Event von einem anderen Thread ausgelöst wird
            if (!this.Dispatcher.CheckAccess())
            {
                this.Dispatcher.Invoke(() => this.ConferenceOnNeuePmRaised(sender, messages, alleNachrichten));
                return;
            }

            //Falls alle Nachrichten abgerufen wurden setze den Text der ChatBox zurück
            //Alle Nachrichten sind die 15 aktuellsten Nachrichten. Dies ist eine Limitation der Funktionen von Proxer selbst
            if (alleNachrichten) this.ChatBox.Text = "";

            foreach (Conference.Message message in messages)
            {
                this.ChatBox.Text += "[" + message.TimeStamp + "] " + await message.Sender.UserName.GetObject("ERROR") +
                                     ": " +
                                     message.Content + "\n";
            }
        }

        private async Task InitComponents()
        {
            this.Title = "Konferenz: " + (await this._conference.Title.GetObject()).OnError("ERROR");

            foreach (User teilnehmer in (await this._conference.Participants.GetObject()).OnError(new User[0]))
            {
                TextBlock lTeilnehmerBlock = new TextBlock {DataContext = teilnehmer, Text = teilnehmer.ToString()};
                lTeilnehmerBlock.MouseLeftButtonDown += this.UserTextBlock_MouseLeftButtonDown;
                this.TeilnehmerBox.Items.Add(lTeilnehmerBlock);
            }

            TextBlock lLeaderBlock = new TextBlock
            {
                DataContext = (await this._conference.Leader.GetObject()).OnError(User.System),
                Text = (await this._conference.Leader.GetObject()).OnError(User.System).ToString()
            };
            lLeaderBlock.MouseLeftButtonDown += this.UserTextBlock_MouseLeftButtonDown;
            this.LeiterBox.Items.Add(lLeaderBlock);
        }

        private void UserTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock lTeilnehmerBlock = sender as TextBlock;
            new UserWindow(lTeilnehmerBlock?.DataContext as User ?? User.System, this._senpai).Show();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await this.InitComponents();

            //Abrufen der Nachrichten im Hintergrund wird aktiviert
            //Durch setzen auf false kann diese Funktion wieder deaktiviert werden
            this._conference.Active = true;
        }

        #endregion
    }
}