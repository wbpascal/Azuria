using Azuria.ErrorHandling;
using Azuria.Main.Search;
using System.Collections.Generic;
using System.Windows;
using System;
using System.Windows.Input;
using System.Windows.Controls;

namespace Azuria.Example
{
    public partial class SearchWindow : Window
    {
        private readonly Senpai _senpai;

        public SearchWindow(Senpai senpai)
        {
            this._senpai = senpai;
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.UserRadioButton.IsChecked.Value)
            {
                this.UserSearch();
            }
        }

        private async void UserSearch()
        {
            ProxerResult<SearchResult<User>> lResult = await SearchHelper.Search<User>(this.SearchTextBox.Text, this._senpai);
            if (lResult.Success)
            {
                IEnumerable<User> lSearchResult = lResult.Result.SearchResults;
                this.UserSearchResultListBox.Items.Clear();
                foreach(User lUser in lSearchResult) 
                {
                    this.UserSearchResultListBox.Items.Add(lUser);
                }
            }
            else
            {
                MessageBox.Show("Es ist ein Fehler bei der Anfrage aufgetreten, bitte versuche es später erneut!");
            }
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(((ListViewItem) sender).Content is User) 
            {
                new UserWindow(((ListViewItem)sender).Content as User, this._senpai).Show();
            }
        }
    }
}
