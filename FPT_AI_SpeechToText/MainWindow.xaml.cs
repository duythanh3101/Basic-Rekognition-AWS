using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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

namespace FPT_AI_SpeechToText
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        class TTSData
        {
            public List<DictionaryEntry> hypotheses { get; set; }
        }

        class DictionaryEntry
        {
            public string utterance { get; set; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtURL.Text != null)
            {
                string file = txtURL.Text.ToString();

                if (!string.IsNullOrEmpty(file))
                {
                    var payload = File.ReadAllBytes(file);
                    string result = Task.Run(async () =>
                    {
                        var client = new HttpClient();
                        client.DefaultRequestHeaders.Add("api-key", "FEBGF6YdWXWxqGeq9oYJnArhHLr7aJZP");
                        var response = await client.PostAsync("https://api.fpt.ai/hmi/asr/general", new ByteArrayContent(payload));
                        return await response.Content.ReadAsStringAsync();
                    }).GetAwaiter().GetResult();

                    var data = JsonConvert.DeserializeObject<TTSData>(result);
                    if (data != null && data.hypotheses != null && data.hypotheses.Count > 0)
                    {
                        resultTextBox.Text = data.hypotheses[0].utterance;
                        MessageBox.Show(data.hypotheses[0].utterance);
                    }
                }

            }

        }

        private void OpenDialog_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.mp3";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (openFileDialog.ShowDialog() == true)
            {
                if (openFileDialog.FileName != string.Empty)
                {
                    txtURL.Text = openFileDialog.FileName;
                }
            }
        }

    }
}
