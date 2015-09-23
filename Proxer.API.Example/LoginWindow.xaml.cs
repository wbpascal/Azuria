using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Proxer.API.Example
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Senpai _senpai;

        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            this._senpai = new Senpai();

            this.InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.TextBox1.Text) || string.IsNullOrEmpty(this.PasswordBox1.Password))
            {
                MessageBox.Show("Bitte gib ein Benutzernamen und ein Password ein!");
                return;
            }

            (sender as Button).IsEnabled = false;

            if (await this._senpai.Login(this.TextBox1.Text, this.PasswordBox1.Password))
                MessageBox.Show("Du wurdest erfolgreich eingeloggt!");
            else
                MessageBox.Show("Die Benutzername/Passwort-Kombination konnte nicht erkannt werden!");

            (sender as Button).IsEnabled = true;
        }
    }
}
