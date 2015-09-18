using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Proxer.API.Example
{
    /// <summary>
    /// 
    /// </summary>
    public partial class LoginForm : Form
    {
        private readonly Senpai _senpai;

        /// <summary>
        /// 
        /// </summary>
        public LoginForm()
        {
            this._senpai = new Senpai();

            this.InitializeComponent();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.userNameTextBox.Text) || string.IsNullOrEmpty(this.passwordBox.Text))
            {
                MessageBox.Show("Bitte gib etwas als Benutzernamen und Passwort ein!");
                return;
            }

            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += (o, args) =>
            {
                args.Result = this._senpai.Login(this.userNameTextBox.Text, this.passwordBox.Text);
            };
            backgroundWorker.RunWorkerCompleted += (o, args) =>
            {
                if ((bool) args.Result) this.groupBox1.Enabled = true;
                else MessageBox.Show("Die Benutzername/Passwort-Kombination konnte nicht erkannt werden!");
                this.loginButton.Enabled = true;
            };
            this.loginButton.Enabled = false;
            backgroundWorker.RunWorkerAsync();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Interaction.InputBox("Bitte gib die ID der Konferenz an, die aufgerufen werden soll!", "Konferenz-ID", "-1");
        }
    }
}
