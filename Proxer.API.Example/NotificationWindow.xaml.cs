using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Proxer.API.EventArguments;
using Proxer.API.Exceptions;
using Proxer.API.Notifications;
using Proxer.API.Utilities;

namespace Proxer.API.Example
{
    /// <summary>
    ///     Interaktionslogik für NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow : Window
    {
        private readonly Senpai _senpai;

        internal NotificationWindow(Senpai senpai)
        {
            this._senpai = senpai;

            if (!this._senpai.LoggedIn)
            {
                MessageBox.Show("Du musst für diese Aktion eingeloggt sein!");
                return;
            }

            this.InitializeComponent();

            this._senpai.NotificationRaised += this.NotificationRaised;
            this._senpai.AmUpdateNotificationRaised += this.AmUpdateNotificationRaised;
            this._senpai.FriendNotificationRaised += this.FriendNotificationRaised;
            this._senpai.NewsNotificationRaised += this.NewsNotificationRaised;
            this._senpai.PmNotificationRaised += this.PmNotificationRaised;
            this._senpai.ErrorDuringNotificationFetch += this.ErrorDuringNotificationFetch;
        }

        #region

        private void ErrorDuringNotificationFetch(Senpai sender, Exception[] exceptions)
        {
            //Aktion, die ausgeführt wird, wenn ein Fehler bei dem Abrufen der Benachrichtigungs-Events aufgetreten ist
        }

        private void NotificationRaised(Senpai sender, IEnumerable<INotificationEventArgs> e)
        {
            if (!this.Dispatcher.CheckAccess())
            {
                this.Dispatcher.Invoke(() => this.NotificationRaised(sender, e));
                return;
            }

            if (this.IsFocused) return;

            IEnumerable<INotificationEventArgs> notificationEventArgses = e as IList<INotificationEventArgs> ??
                                                                          e.ToList();
            if (notificationEventArgses.Any())
            {
                int lAnzahl = 0;
                notificationEventArgses.ToList().ForEach(x => lAnzahl += x.NotificationCount);
                this.Title = "Benachrichtigungen (" + lAnzahl + ")";
            }
            else
            {
                this.Title = "Benachrichtigungen";
            }

            //man kann auch einen Ton abspielen
        }

        private void AmUpdateNotificationRaised(Senpai sender, AmNotificationEventArgs e)
        {
            this.LoadAmUpdateNotifications(e.Benachrichtigungen);
        }

        private void FriendNotificationRaised(Senpai sender, FriendNotificationEventArgs e)
        {
            this.ProcessFriendRequests(e.Benachrichtigungen);
        }

        private void NewsNotificationRaised(Senpai sender, NewsNotificationEventArgs e)
        {
            this.LoadNewsNotifications(e.Benachrichtigungen);
        }

        private void PmNotificationRaised(Senpai sender, PmNotificationEventArgs e)
        {
            this.LoadPmNotification(e.Benchrichtigungen);
        }

        private async void LoadAmUpdateNotifications(AnimeMangaUpdateCollection collection)
        {
            if (!this.Dispatcher.CheckAccess())
            {
                this.Dispatcher.Invoke(() => this.LoadAmUpdateNotifications(collection));
                return;
            }

            this.AmListBox.Items.Clear();

            ProxerResult<AnimeMangaUpdateObject[]> lResult = await collection.GetAllAnimeMangaUpdates();
            if (!lResult.Success)
            {
                MessageBox.Show(lResult.Exceptions.OfType<NotLoggedInException>().Any()
                    ? "Bitte logge dich ein bevor du fortfährst!"
                    : "Es ist ein Fehler während der Anfrage aufgetreten! (LoadAmUpdateNotifications)");

                return;
            }

            foreach (AnimeMangaUpdateObject notification in lResult.Result)
            {
                this.AmListBox.Items.Add(notification);
            }
        }

        private async void ProcessFriendRequests(FriendRequestCollection collection)
        {
            if (!this.Dispatcher.CheckAccess())
            {
                this.Dispatcher.Invoke(() => this.ProcessFriendRequests(collection));
                return;
            }

            ProxerResult<FriendRequestObject[]> lResult = await collection.GetAllFriendRequests();
            if (!lResult.Success)
            {
                MessageBox.Show(lResult.Exceptions.OfType<NotLoggedInException>().Any()
                    ? "Bitte logge dich ein bevor du fortfährst!"
                    : "Es ist ein Fehler während der Anfrage aufgetreten! (ProcessFriendRequests)");

                return;
            }

            foreach (FriendRequestObject requestObject in lResult.Result)
            {
                MessageBoxResult lBoxResult =
                    MessageBox.Show(
                        "Möchtest du die Freundschaftsanfrage von " + requestObject.UserName + " [ID:" +
                        requestObject.UserId + "] annehmen?", "Freundschaftsanfrage", MessageBoxButton.YesNoCancel);

                switch (lBoxResult)
                {
                    case MessageBoxResult.Yes:
                        ProxerResult<bool> lAcceptResult = await requestObject.AcceptRequest();
                        if (lAcceptResult.Success && lAcceptResult.Result)
                            MessageBox.Show("Die Freundschaftsanfrage wurde erfolgreich angenommen!");
                        else
                            MessageBox.Show("Es ist ein Fehler passiert!");
                        break;
                    case MessageBoxResult.No:
                        ProxerResult<bool> lDenyResult = await requestObject.DenyRequest();
                        if (lDenyResult.Success && lDenyResult.Result)
                            MessageBox.Show("Die Freundschaftsanfrage wurde erfolgreich abgelehnt!");
                        else
                            MessageBox.Show("Es ist ein Fehler passiert!");
                        break;
                }
            }
        }

        private async void LoadNewsNotifications(NewsCollection collection)
        {
            if (!this.Dispatcher.CheckAccess())
            {
                this.Dispatcher.Invoke(() => this.LoadNewsNotifications(collection));
                return;
            }

            this.NewsListBox.Items.Clear();

            ProxerResult<NewsObject[]> lResult = await collection.GetAllNews();
            if (!lResult.Success)
            {
                MessageBox.Show(lResult.Exceptions.OfType<NotLoggedInException>().Any()
                    ? "Bitte logge dich ein bevor du fortfährst!"
                    : "Es ist ein Fehler während der Anfrage aufgetreten! (LoadNewsNotifications)");

                return;
            }

            foreach (NewsObject notification in lResult.Result)
            {
                this.NewsListBox.Items.Add(notification);
            }
        }

        private async void LoadPmNotification(PmCollection collection)
        {
            if (!this.Dispatcher.CheckAccess())
            {
                this.Dispatcher.Invoke(() => this.LoadPmNotification(collection));
                return;
            }

            this.PmListBox.Items.Clear();

            ProxerResult<PmObject[]> lResult = await collection.GetAllPrivateMessages();
            if (!lResult.Success)
            {
                MessageBox.Show(lResult.Exceptions.OfType<NotLoggedInException>().Any()
                    ? "Bitte logge dich ein bevor du fortfährst!"
                    : "Es ist ein Fehler während der Anfrage aufgetreten! (LoadNewsNotifications)");

                return;
            }

            foreach (PmObject notification in lResult.Result)
            {
                this.PmListBox.Items.Add(notification);
            }
        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            this.Title = "Benachrichtigungen";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this._senpai.InitNotifications();

            this.LoadAmUpdateNotifications(this._senpai.AnimeMangaUpdates);
            this.ProcessFriendRequests(this._senpai.FriendRequests);
            this.LoadNewsNotifications(this._senpai.News);
            this.LoadPmNotification(this._senpai.PrivateMessages);
        }

        private void AmTextBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                MessageBox.Show(((sender as ListBox).SelectedItem as AnimeMangaUpdateObject).Name);
            }
        }

        private void NewsListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                MessageBox.Show(((sender as ListBox).SelectedItem as NewsObject).Nid.ToString());
            }
        }

        private void PmListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                MessageBox.Show(((sender as ListBox).SelectedItem as PmObject).TimeStamp.ToShortTimeString());
            }
        }

        #endregion
    }
}