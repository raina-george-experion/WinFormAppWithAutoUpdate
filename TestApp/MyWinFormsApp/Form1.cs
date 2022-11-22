using MyWinFormsApp.Models;
using Newtonsoft.Json;
using System.Net;

namespace MyWinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        ///this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();  

        private DataGridView dataGridView1 = new DataGridView();

        private void button1_Click(object sender, EventArgs e)
        {
            Boolean uploadStatus = false;
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            DialogResult dr = this.openFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                foreach (String localFilename in openFileDialog1.FileNames)
                {
                    string url = "https://localhost:7069/files/upload-files";
                    string filePath = @"\";
                    Random rnd = new Random();
                    string uploadFileName = "Imag" + rnd.Next(9999).ToString();
                    uploadStatus = Upload(url, filePath, localFilename, uploadFileName);
                }
            }
            if (uploadStatus)
            {
                MessageBox.Show("File Uploaded");
            }
            else
            {
                MessageBox.Show("File Not Uploaded");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string url = "http://localhost:51389/Uploads/";
            string downloadFileName = textBox1.Text.Trim();
            string downloadPath = Application.StartupPath + @"\Downloads\";

            if (!Directory.Exists(downloadPath))
                Directory.CreateDirectory(downloadPath);

            Boolean isFileDownloaded = Download(url, downloadFileName, downloadPath);
            if (isFileDownloaded)
            {
                MessageBox.Show("File Downloaded");
            }
            else
            {
                MessageBox.Show("File Not Downloaded");
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        bool Upload(string url, string filePath, string localFilename, string uploadFileName)
        {
            Boolean isFileUploaded = false;

            try
            {
                HttpClient httpClient = new HttpClient();

                var fileStream = File.Open(localFilename, FileMode.Open);
                var fileInfo = new FileInfo(localFilename);
                UploadFile uploadResult = null;
                bool _fileUploaded = false;

                MultipartFormDataContent content = new MultipartFormDataContent();
                content.Headers.Add("filePath", filePath);
                content.Add(new StreamContent(fileStream), "\"file\"", string.Format("\"{0}\"", uploadFileName + fileInfo.Extension));

                Task taskUpload = httpClient.PostAsync(url, content).ContinueWith(task =>
                {
                    if (task.Status == TaskStatus.RanToCompletion)
                    {
                        var response = task.Result;

                        if (response.IsSuccessStatusCode)
                        {
                            uploadResult = response.Content.ReadAsAsync<UploadFile>().Result;
                            if (uploadResult != null)
                                _fileUploaded = true;
                        }
                    }
                    fileStream.Dispose();
                });

                taskUpload.Wait();

                if (_fileUploaded)
                    isFileUploaded = true;
                httpClient.Dispose();

            }
            catch (Exception ex)
            {
                isFileUploaded = false;
            }
            return isFileUploaded;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string URI = "http://localhost:51389/api/FileHandlingAPI/getFileInfo?Id=1";
            GetFileInformation(URI);
        }

        private async void GetFileInformation(string url)
        {
            List<ServerFileInformation> filesinformation = new List<ServerFileInformation>();
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var fileJsonString = await response.Content.ReadAsStringAsync();

                        dataGridView1.DataSource = JsonConvert.DeserializeObject<ServerFileInformation[]>(fileJsonString).ToList();
                    }
                }
            }
        }

        // url = http://localhost:51389/Uploads/"
        // downloadFileName = "new2.jpg"
        // downloadPath =  Application.StartupPath + "/Downloads/";
        bool Download(string url, string downloadFileName, string downloadFilePath)
        {
            string downloadfile = downloadFilePath + downloadFileName;
            string httpPathWebResource = null;
            Boolean ifFileDownoadedchk = false;
            ifFileDownoadedchk = false;
            WebClient myWebClient = new WebClient();
            httpPathWebResource = url + downloadFileName;
            myWebClient.DownloadFile(httpPathWebResource, downloadfile);

            ifFileDownoadedchk = true;

            return ifFileDownoadedchk;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}