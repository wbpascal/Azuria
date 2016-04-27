using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Azuria.Example.Controls.Search;
using Azuria.Example.Models.Search;
using Azuria.Example.Utilities;
using Azuria.Main;
using Azuria.Main.Minor;
using Azuria.Main.Search;
using Azuria.Utilities.ErrorHandling;

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

            List<GenreObject.GenreType> lGenreContains = new List<GenreObject.GenreType>();
            foreach (GenreCheckBox lCurCheckBox in this.GenreContainsWrapPanel.FindVisualChildren<GenreCheckBox>())
            {
                if (lCurCheckBox.IsChecked.Value)
                {
                    lGenreContains.Add(lCurCheckBox.Genre);
                }
            }

            List<GenreObject.GenreType> lGenreExcludes = new List<GenreObject.GenreType>();
            foreach (GenreCheckBox lCurCheckBox in this.GenreExcludesWrapPanel.FindVisualChildren<GenreCheckBox>())
            {
                if (lCurCheckBox.IsChecked.Value)
                {
                    lGenreExcludes.Add(lCurCheckBox.Genre);
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
                foreach (
                    IAnimeMangaObject lAnimeMangaObject in
                        this._animeMangaSearchResults?.SearchResults ?? new IAnimeMangaObject[0])
                {
                    this.AnimeMangaSearchResultListBox.Items.Add(
                        await new AnimeMangaSearchModel(lAnimeMangaObject).InitProperties());
                }
            }
            else
            {
                MessageBox.Show("Es ist ein Fehler bei der Anfrage aufgetreten, bitte versuche es später erneut!");
            }
        }

        private async void AnimeMangaSearch_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            //Wenn bis an das Ende gescrollt wurde lade die nächsten Ergebnisse

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
                        this.AnimeMangaSearchResultListBox.Items.Add(
                            await new AnimeMangaSearchModel(lCurAnimeManga).InitProperties());
                    }
                }
            }
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (((ListViewItem) sender).Content is UserSearchModel)
            {
                new UserWindow((((ListViewItem) sender).Content as UserSearchModel)?.UserObject, this._senpai).Show();
            }
            else if (((ListViewItem) sender).Content is AnimeMangaSearchModel)
            {
                new AnimeMangaWindow((((ListViewItem) sender).Content as AnimeMangaSearchModel)?.AnimeMangaObject,
                    this._senpai).Show();
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
                    this.UserSearchResultListBox.Items.Add(await new UserSearchModel(lUser).InitProperties());
                }
            }
            else
            {
                MessageBox.Show("Es ist ein Fehler bei der Anfrage aufgetreten, bitte versuche es später erneut!");
            }
        }

        private async void UserSearch_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            //Wenn bis an das Ende gescrollt wurde lade die nächsten Ergebnisse

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
                        this.UserSearchResultListBox.Items.Add(await new UserSearchModel(lCurUser).InitProperties());
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