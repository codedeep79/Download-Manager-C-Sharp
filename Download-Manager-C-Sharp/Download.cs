using System;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;


namespace Download_Manager_C_Sharp
{
    public partial class Download : Form
    {
        private Form1 _formMain;
        public Download(Form1 formMain)
        {
            InitializeComponent();
            _formMain = formMain;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "Select Your Path"})
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtPath.Text = fbd.SelectedPath;
                    Properties.Settings.Default.Path = txtPath.Text;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            // Download file
            Uri uri = new Uri(this.url);
            fileName = System.IO.Path.GetFileName(uri.AbsolutePath);
            client.DownloadFileAsync(uri, Properties.Settings.Default.Path + "/" + fileName);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            client.CancelAsync();
        }

        WebClient client;

        // Set value in AddUrl
        public string url { get; set; }
        public string fileName { get; set; }
        public double fileSize { get; set; }
        public double percentage { get; set; }

        

        private void Download_Load(object sender, EventArgs e)
        {
            client = new WebClient();
            client.DownloadProgressChanged += clientDownloadProgressBarChanged;
            client.DownloadFileCompleted += clientDownloadFileCompleted;
            txtAddress.Text = url;
        }

        private void clientDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Database.FilesRow row = App.DB.Files.NewFilesRow();
            row.Url = url;
            row.FileName = fileName;
            row.FileSize = (string.Format("{0:0.##} KB", fileSize / 1024));
            row.DateTime = DateTime.Now;
            App.DB.Files.AddFilesRow(row);
            App.DB.AcceptChanges();
            App.DB.WriteXml(string.Format("{0}/data.dat", Application.StartupPath));

            ListViewItem item = new ListViewItem(row.Id.ToString());
            item.SubItems.Add(row.Url);
            item.SubItems.Add(row.FileName);
            item.SubItems.Add(row.FileSize);
            item.SubItems.Add(row.DateTime.ToLongDateString());

            _formMain.listViewItem.Items.Add(item);
            this.Close();
        }

        private void clientDownloadProgressBarChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Minimum = 0;
            double receive = double.Parse(e.BytesReceived.ToString());
            fileSize = double.Parse(e.TotalBytesToReceive.ToString());
            percentage = receive / (fileSize * 100);
            lbDownload.Text = $"Download {string.Format("{0:0.## %}", percentage)}";
            progressBar.Value = int.Parse(Math.Truncate(percentage).ToString());
            progressBar.Update();

        }
    }
}
