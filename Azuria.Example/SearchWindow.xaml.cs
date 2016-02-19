using Azuria.ErrorHandling;
using Azuria.Main.Search;
using System.Collections.Generic;
using System.Windows;
using Azuria.Example.Utilities;
using System.Windows.Input;
using System.Windows.Controls;
using Azuria.Main.Minor;
using Azuria.Example.Controls.Search;
using Azuria.Main;

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
            else if (this.AnimeMangaRadioButton.IsChecked.Value)
            {
                this.AnimeMangaSearch();
            }
        }

        private async void AnimeMangaSearch()
        {
            Language? lLanguage = null;
            if (this.LanguageGermanRadioButton.IsChecked.Value)
            {
                lLanguage = Main.Minor.Language.Deutsch;
            }
            else if (this.LanguageEnglishRadioButton.IsChecked.Value)
            {
                lLanguage = Main.Minor.Language.Englisch;
            }

            SearchHelper.AnimeMangaType? lAnimeMangaType = null;
            foreach (AnimeMangaTypeRadioButton lCurRadioButton in AnimeMangaTypeWrapPanel.FindVisualChildren<AnimeMangaTypeRadioButton>())
            {
                if (lCurRadioButton.IsChecked.Value)
                {
                    lAnimeMangaType = lCurRadioButton.AnimeMangaType;
                    break;
                }
            }

            List<GenreObject> lGenreContains = new List<GenreObject>();
            foreach (GenreCheckBox lCurCheckBox in GenreContainsWrapPanel.FindVisualChildren<GenreCheckBox>())
            {
                if (lCurCheckBox.IsChecked.Value)
                {
                    lGenreContains.Add(new GenreObject(lCurCheckBox.Genre));
                }
            }

            List<GenreObject> lGenreExcludes = new List<GenreObject>();
            foreach (GenreCheckBox lCurCheckBox in GenreExcludesWrapPanel.FindVisualChildren<GenreCheckBox>())
            {
                if (lCurCheckBox.IsChecked.Value)
                {
                    lGenreExcludes.Add(new GenreObject(lCurCheckBox.Genre));
                }
            }

            List<Fsk> lFskIncludes = new List<Fsk>();
            foreach (FskCheckBox lCurCheckBox in FskContainsWrapPanel.FindVisualChildren<FskCheckBox>())
            {
                if (lCurCheckBox.IsChecked.Value)
                {
                    lFskIncludes.Add(lCurCheckBox.Fsk);
                }
            }

            SearchHelper.SortAnimeManga? lSortBy = null;
            foreach (SortByRadionButton lCurRadioButton in SortByStackPanel.FindVisualChildren<SortByRadionButton>())
            {
                if (lCurRadioButton.IsChecked.Value)
                {
                    lSortBy = lCurRadioButton.SortBy;
                }
            }

            ProxerResult<SearchResult<IAnimeMangaObject>> lResult = await SearchHelper.SearchAnimeManga<IAnimeMangaObject>(this.SearchTextBox.Text, this._senpai, lAnimeMangaType
                , lGenreContains, lGenreExcludes, lFskIncludes, lLanguage, lSortBy);
            if (lResult.Success)
            {
                IEnumerable<IAnimeMangaObject> lSearchResult = lResult.Result.SearchResults;
                this.AnimeMangaSearchResultListBox.Items.Clear();
                this.UserSearchResultListBox.Items.Clear();
                foreach (IAnimeMangaObject lUser in lSearchResult)
                {
                    this.AnimeMangaSearchResultListBox.Items.Add(lUser);
                }
            }
            else
            {
                MessageBox.Show("Es ist ein Fehler bei der Anfrage aufgetreten, bitte versuche es später erneut!");
            }
        }

        private async void UserSearch()
        {
            ProxerResult<SearchResult<User>> lResult = await SearchHelper.Search<User>(this.SearchTextBox.Text, this._senpai);
            if (lResult.Success)
            {
                IEnumerable<User> lSearchResult = lResult.Result.SearchResults;
                this.UserSearchResultListBox.Items.Clear();
                this.AnimeMangaSearchResultListBox.Items.Clear();
                foreach (User lUser in lSearchResult)
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
            if (((ListViewItem)sender).Content is User)
            {
                new UserWindow(((ListViewItem)sender).Content as User, this._senpai).Show();
            }
            else if (((ListViewItem)sender).Content is IAnimeMangaObject)
            {
                new AnimeMangaWindow(((ListViewItem)sender).Content as IAnimeMangaObject, this._senpai).Show();
            }
        }
    }
}
