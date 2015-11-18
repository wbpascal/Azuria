using System.Windows.Controls;
using Proxer.API.Main.User;

namespace Proxer.API.Example.Controls
{
    public partial class AnimeMangaProgressControl : UserControl
    {
        public AnimeMangaProgressObject AnimeMangaProgressObject { get; set; }

        public AnimeMangaProgressControl(AnimeMangaProgressObject animeMangaProgressObject)
        {
            //Schreibt das AnimeMangaProgressObject in die zugehörige Eigenschaft
            //Die Werte der Controls werden mit Bindings gelöst
            this.AnimeMangaProgressObject = animeMangaProgressObject;
            this.InitializeComponent();
        }

        private void StackPanel_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
