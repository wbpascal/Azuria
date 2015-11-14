using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Proxer.API.Community;
using Proxer.API.Utilities;
using Proxer.API.Utilities.Net;

namespace Proxer.API.Example
{
    public partial class LoginWindow : Window
    {
        private Senpai _senpai;

        public LoginWindow()
        {
            //API-Anfragen Timeout auf 5 Sekunden festlegen
            HttpUtility.Timeout = 5000;

            this.InitializeComponent();
        }

        #region

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            //Dieser Check ist eigentlich unnötig, da die Login-Methode dies erledigt.
            //Die Methode gibt jedoch nur false zurück, also kann dies benutzt werden um den Benutzer 
            //eine bessere Rückmeldung zu geben
            if (string.IsNullOrEmpty(this.TextBox1.Text) || string.IsNullOrEmpty(this.PasswordBox1.Password))
            {
                MessageBox.Show("Bitte gib ein Benutzernamen und ein Password ein!");
                return;
            }

            (sender as Button).IsEnabled = false;

            //Es wird empfolen jedesmal, wenn ein Benutzer sich einloggt ein neues Senpai-Objekt zu erzeugen, 
            //da es momentan noch keine Reset-Methode gibt und einige Eigenschaften zurückgesetzt werden müssen.
            this._senpai = new Senpai();

            //Loggt den Benutzer mit den angegeben Daten ein.
            ProxerResult<bool> lResult = await this._senpai.Login(this.TextBox1.Text, this.PasswordBox1.Password);
            //Unterscheidet, ob der Benutzer eingeloggt wurde oder nicht
            if (lResult.Success && lResult.Result)
                //Benutzer wurder erfolgreich eingeloggt
                MessageBox.Show("Du wurdest erfolgreich eingeloggt!");
            else
            {
                //Es ist ein Fehler in der Methode aufgetreten
                //Entweder der Benutzername oder Passwort ist falsch oder es ist ein anderer Fehler passiert
                //Wenn Success = false, dann ist ein anderer Fehler aufgetreten
                //Weiter Infos über die Ausnahme kann in der Eigenschaft Exceptions von lResult gefunden werden 
                MessageBox.Show(lResult.Success
                    ? "Die Benutzername/Passwort-Kombination konnte nicht erkannt werden!"
                    : "Es ist ein Fehler während der Anfrage aufgetreten! (Login)");
            }

            (sender as Button).IsEnabled = true;
        }

        private void NotificationButton_Click(object sender, RoutedEventArgs e)
        {
            //Öffne des Benachrichtigungsfenster
            new NotificationWindow(this._senpai).Show();
        }

        #endregion

        private async void ConferenceButton_Click(object sender, RoutedEventArgs e)
        {
            //Gib alle Konferenzen zurück
            ProxerResult<List<Conference>> lResult = await this._senpai.GetAllConferences();

            if (lResult.Success && lResult.Result.Any())
            {
                //Öffne die erste Konferenz in der Liste
                new ConferenceWindow(lResult.Result.First()).Show();
            }
            else
            {
                //Die Methode hat eine Ausnahme ausgelöst oder der Benutzer hat keine Konferenzen
                MessageBox.Show(lResult.Success
                    ? "Der Benutzer hat keine Konferenzen!"
                    : "Es ist ein Fehler während der Anfrage aufgetreten! (Konferenzen)");
            }
        }
    }
}