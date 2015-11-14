using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Proxer.API.Community;
using Proxer.API.Utilities;

namespace Proxer.API.Example
{
    public partial class ConferenceWindow : Window
    {
        private readonly Conference _conference;

        public ConferenceWindow(Conference conference)
        {
            this._conference = conference;
            this.InitializeComponent();

            this._conference.ErrorDuringPmFetchRaised += this.ConferenceOnErrorDuringPmFetchRaised;
            this._conference.NeuePmRaised += this.ConferenceOnNeuePmRaised;
        }

        private void ConferenceOnNeuePmRaised(Conference sender, List<Conference.Message> messages, bool alleNachrichten)
        {
            //Hier werden die Nachrichten verarbeitet

            //Falls alle Nachrichten abgerufen wurden setze den Text der ChatBox zurück
            if (alleNachrichten) this.ChatBox.Text = "";

            //Gehe alle Nachrichten durch und füge sie der ChatBox hinzu
            foreach (Conference.Message message in messages)
            {
                this.ChatBox.Text += "[" + message.TimeStamp.ToShortTimeString() + "] " + message.Sender.UserName + ": " +
                                     message.Nachricht + "\n";
            }
        }

        private void ConferenceOnErrorDuringPmFetchRaised(Conference sender, IEnumerable<Exception> exceptions)
        {
            //Falls beim Abrufen der Nachrichten im Hintergrund ein Fehler auftritt
            MessageBox.Show("Es ist ein Fehler beim Abrufen der Nachrichten aufgetreten!", "Fehler", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Eigenschaften der Konferenz initialisieren
            //Nachrichten werden noch nicht abgerufen
            await this._conference.InitConference();

            //Schreibt die Infos der Konferenz in die zugehörigen Controls
            this.InitComponents();

            //Abrufen der Nachrichten im Hintergrund wird aktiviert
            //Durch setzen auf false kann diese Funktion wieder deaktiviert werden
            this._conference.Aktiv = true;
        }

        private void InitComponents()
        {
            //Titel der Konferenz als Fenstertitel
            this.Title = this._conference.Titel;

            //Gehe durch die Teilnehmerliste
            foreach (User teilnehmer in this._conference.Teilnehmer)
            {
                //Füge den Teilnehmer zur ListBox hinzu
                //ListBox stellt den Teilnehmer da, indem es die ToString()-Methode aufruft
                this.TeilnehmerBox.Items.Add(teilnehmer);
            }

            //Scheibe den Leiter in die LeiterBox
            //Darstellung wie bei der TeilnehmerBox
            this.LeiterBox.Items.Add(this._conference.Leiter);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //Versuche die Nachricht zu senden
            if (!(await this._conference.SendeNachricht(this.InputBox.Text)).OnError(false))
            {
                //Falls ein Fehler beim Senden der Nachricht aufgetreten ist
                MessageBox.Show("Die Nachricht konnte nicht gesendet werden!", "Fehler",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                //Falls die Aktion erfolgreich war setzte den Text in der InputBox zurück
                this.InputBox.Clear();
            }
        }
    }
}
