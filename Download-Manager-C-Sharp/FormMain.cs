using System;
using System.IO;
using System.Windows.Forms;

namespace Download_Manager_C_Sharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void tsSetting_Click(object sender, EventArgs e)
        {
            using (Setting setting = new Setting())
            {
                setting.ShowDialog();
            }
        }

        private void tsAdd_Click(object sender, EventArgs e)
        {
            using (Add_Url addUrl = new Add_Url())
            {
                if (addUrl.ShowDialog() == DialogResult.OK)
                {
                    Download download = new Download(this);
                    download.url = addUrl.url;
                    download.Show();

                }
            }
        }

        private void tsRemove_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure Want To Delete This Record ?", "Notice !!",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
            {
                for(int i = listViewItem.SelectedItems.Count; i > 0; --i)
                {
                    ListViewItem item = listViewItem.SelectedItems[i - 1];
                    App.DB.Files.Rows[item.Index].Delete();
                    listViewItem.Items[item.Index].Remove();
                }

                // Save
                App.DB.Files.AcceptChanges();
                App.DB.Files.WriteXml(string.Format("{0}/data.dat", Application.StartupPath));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string fileName = string.Format("{0}/data.dat", Application.StartupPath);
            if (File.Exists(fileName))
                App.DB.ReadXml(fileName);

            foreach(Database.FilesRow row in App.DB.Files)
            {
                ListViewItem item = new ListViewItem(row.Id.ToString());
                item.SubItems.Add(row.Url);
                item.SubItems.Add(row.FileName);
                item.SubItems.Add(row.FileSize);
                item.SubItems.Add(row.DateTime.ToLongDateString());
                listViewItem.Items.Add(item);
            }
        }
    }
}
