using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Azuria.ErrorHandling;
using Azuria.Example.Controls.Search;
using Azuria.Example.Utilities;
using Azuria.Main;
using Azuria.Main.Minor;
using Azuria.Main.Search;

namespace Azuria.Example
{
    public partial class SearchWindow : Window
    {
        private readonly Senpai _senpai;
        private SearchResult<IAnimeMangaObject> _animeMangaSearchResults;
        private SearchResult<User> _userSearchResults;

        public SearchWindow(Senpai senpai)
        {
            this._senpai = senpai;
            this.InitializeComponent();
        }

        #region

        private async void AnimeMangaSearch()
        {
            Language? lLanguage = null;
            if (this.LanguageGermanRadioButton.IsChecked.Value)
            {
                lLanguage = Main.Minor.Language.German;
            }
            else if (this.LanguageEnglishRadioButton.IsChecked.Value)
            {
                lLanguage = Main.Minor.Language.English;
            }

            SearchHelper.AnimeMangaType? lAnimeMangaType = null;
            foreach (
                AnimeMangaTypeRadioButton lCurRadioButton in
                    this.AnimeMangaTypeWrapPanel.FindVisualChildren<AnimeMangaTypeRadioButton>())
            {
                if (lCurRadioButton.IsChecked.Value)
                {
                    lAnimeMangaType = lCurRadioButton.AnimeMangaType;
                    break;
                }
            }

            List<GenreObject> lGenreContains = new List<GenreObject>();
            foreach (GenreCheckBox lCurCheckBox in this.GenreContainsWrapPanel.FindVisualChildren<GenreCheckBox>())
            {
                if (lCurCheckBox.IsChecked.Value)
                {
                    lGenreContains.Add(new GenreObject(lCurCheckBox.Genre));
                }
            }

            List<GenreObject> lGenreExcludes = new List<GenreObject>();
            foreach (GenreCheckBox lCurCheckBox in this.GenreExcludesWrapPanel.FindVisualChildren<GenreCheckBox>())
            {
                if (lCurCheckBox.IsChecked.Value)
                {
                    lGenreExcludes.Add(new GenreObject(lCurCheckBox.Genre));
                }
            }

            List<Fsk> lFskIncludes = new List<Fsk>();
            foreach (FskCheckBox lCurCheckBox in this.FskContainsWrapPanel.FindVisualChildren<FskCheckBox>())
            {
                if (lCurCheckBox.IsChecked.Value)
                {
                    lFskIncludes.Add(lCurCheckBox.Fsk);
                }
            }

            SearchHelper.SortAnimeManga? lSortBy = null;
            foreach (
                SortByRadionButton lCurRadioButton in this.SortByStackPanel.FindVisualChildren<SortByRadionButton>())
            {
                if (lCurRadioButton.IsChecked.Value)
                {
                    lSortBy = lCurRadioButton.SortBy;
                }
            }

            ProxerResult<SearchResult<IAnimeMangaObject>> lResult =
                await
                    SearchHelper.SearchAnimeManga<IAnimeMangaObject>(this.SearchTextBox.Text, this._senpai,
                        lAnimeMangaType
                        , lGenreContains, lGenreExcludes, lFskIncludes, lLanguage, lSortBy);
            if (lResult.Success)
            {
                this._animeMangaSearchResults = lResult.Result;
                this._userSearchResults = null;
                this.AnimeMangaSearchResultListBox.Items.Clear();
                this.UserSearchResultListBox.Items.Clear();
                foreach (IAnimeMangaObject lUser in this._animeMangaSearchResults.SearchResults)
                {
                    this.AnimeMangaSearchResultListBox.Items.Add(lUser);
                }
            }
            else
            {
                MessageBox.Show("Es ist ein Fehler bei der Anfrage aufgetreten, bitte versuche es später erneut!");
            }
        }

        private async void AnimeMangaSearch_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            //Wenn bis auf an das Ende gescrollt wurde lade die nächsten Ergebnisse

            ScrollViewer lScrollViewer = (ScrollViewer) sender;
            if (lScrollViewer.VerticalOffset == lScrollViewer.ScrollableHeight && this._animeMangaSearchResults != null)
            {
                //Wenn die es keine Ergebnisse mehr gibt gibt dem Benutzer bescheid
                if (this._animeMangaSearchResults.SearchFinished)
                {
                    MessageBox.Show("Es sind keine weiteren Suchergebnisse mehr vorhanden!");
                    return;
                }

                ProxerResult<IEnumerable<IAnimeMangaObject>> lResult =
                    await this._animeMangaSearchResults.GetNextSearchResults();
                if (lResult.Success)
                {
                    foreach (IAnimeMangaObject lCurAnimeManga in lResult.Result)
                    {
                        this.AnimeMangaSearchResultListBox.Items.Add(lCurAnimeManga);
                    }
                }
            }
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (((ListViewItem) sender).Content is User)
            {
                new UserWindow(((ListViewItem) sender).Content as User, this._senpai).Show();
            }
            else if (((ListViewItem) sender).Content is IAnimeMangaObject)
            {
                new AnimeMangaWindow(((ListViewItem) sender).Content as IAnimeMangaObject, this._senpai).Show();
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.UserRadioButton.IsChecked.Value)
            {
                this.UserSearch();
            }
            else if (this.AnimeMangaRadioButton.IsChecked.Value)
            {
                this.AnimeMangaSearch();
            }
        }

        private async void UserSearch()
        {
            ProxerResult<SearchResult<User>> lResult =
                await SearchHelper.Search<User>(this.SearchTextBox.Text, this._senpai);
            if (lResult.Success)
            {
                this._userSearchResults = lResult.Result;
                this._animeMangaSearchResults = null;
                this.UserSearchResultListBox.Items.Clear();
                this.AnimeMangaSearchResultListBox.Items.Clear();
                foreach (User lUser in this._userSearchResults.SearchResults)
                {
                    this.UserSearchResultListBox.Items.Add(lUser);
                }
            }
            else
            {
                MessageBox.Show("Es ist ein Fehler bei der Anfrage aufgetreten, bitte versuche es später erneut!");
            }
        }

        private async void UserSearch_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            //Wenn bis auf an das Ende gescrollt wurde lade die nächsten Ergebnisse

            ScrollViewer lScrollViewer = (ScrollViewer) sender;
            if (lScrollViewer.VerticalOffset == lScrollViewer.ScrollableHeight && this._userSearchResults != null)
            {
                //Wenn die es keine Ergebnisse mehr gibt gibt dem Benutzer bescheid
                if (this._userSearchResults.SearchFinished)
                {
                    MessageBox.Show("Es sind keine weiteren Suchergebnisse mehr vorhanden!");
                    return;
                }

                ProxerResult<IEnumerable<User>> lResult = await this._userSearchResults.GetNextSearchResults();
                if (lResult.Success)
                {
                    foreach (User lCurUser in lResult.Result)
                    {
                        this.UserSearchResultListBox.Items.Add(lCurUser);
                    }
                }
            }
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            ((VisualTreeHelper.GetChild(this.UserSearchResultListBox, 0) as Decorator).Child as ScrollViewer)
                .ScrollChanged += this.UserSearch_ScrollChanged;
            ((VisualTreeHelper.GetChild(this.AnimeMangaSearchResultListBox, 0) as Decorator).Child as ScrollViewer)
                .ScrollChanged += this.AnimeMangaSearch_ScrollChanged;
        }

        #endregion
    }
}