using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Proxer.API.Community;

namespace Proxer.API.Example
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Chat : Form
    {
        private readonly Conference _conference;
        private readonly Senpai _senpai;

        /// <summary>
        /// 
        /// </summary>
        public Chat(int conferenceId, Senpai senpai)
        {
            this._conference = new Conference(conferenceId, senpai);
            this._senpai = senpai;
            this.InitializeComponent();
        }
    }
}
