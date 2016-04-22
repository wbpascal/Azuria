using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Azuria.Main;
using Azuria.Main.User;

namespace Azuria.Example.Controls
{
    public partial class AnimeMangaProgressControl : UserControl
    {
        private readonly Senpai _senpai;

        public AnimeMangaProgressControl(AnimeMangaProgressObject<IAnimeMangaObject> animeMangaProgressObject,
            Senpai senpai)
        {
            this._senpai = senpai;
            //Die Werte der Controls werden mit Bindings gelöst
            this.AnimeMangaProgressObject = animeMangaProgressObject;
            this.InitializeComponent();
        }

        public AnimeMangaProgressControl(IAnimeMangaObject animeMangaObject, Senpai senpai)
        {
            this._senpai = senpai;
            //Die Werte der Controls werden mit Bindings gelöst
            //Dies ist eine Möglichkeit einen Favoriten darzustellen (nicht empfohlen)
            this.AnimeMangaProgressObject = new AnimeMangaProgressObject<IAnimeMangaObject>(User.System,
                animeMangaObject, -1, new AnimeMangaProgress(-1, -1), AnimeMangaProgressState.Finished, senpai);
            this.InitializeComponent();
        }

        #region Properties

        public AnimeMangaProgressObject<IAnimeMangaObject> AnimeMangaProgressObject { get; set; }

        #endregion

        #region

        private async void ProgressUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.AnimeNameTextBlock.Text =
                await this.AnimeMangaProgressObject?.AnimeMangaObject.Name.GetObject("ERROR") ?? "";
        }

        private void ProgressUserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new AnimeMangaWindow(this.AnimeMangaProgressObject.AnimeMangaObject, this._senpai).Show();
        }

        #endregion
    }
}