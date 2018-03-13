using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Download_Manager_C_Sharp
{
    public partial class Add_Url : Form
    {
        public Add_Url()
        {
            InitializeComponent();
        }

        public string url { get; set; }
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.url = txtUrl.Text;
        }
    }
}
