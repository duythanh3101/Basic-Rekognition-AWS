using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
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

namespace FPT_AI_TextToSpeech
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

        class ReturnData
        {
            public string async { get; set; }
            public string error { get; set; }
            public string message { get; set; }
            public string request_id { get; set; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ContentTextBox.Text != null)
            {
                string payload = ContentTextBox.Text.ToString();
                if (!string.IsNullOrEmpty(payload))
                {
                    string result = Task.Run(async () =>
                    {

                        var client = new HttpClient();
                        client.DefaultRequestHeaders.Add("api-key", "FEBGF6YdWXWxqGeq9oYJnArhHLr7aJZP");
                        client.DefaultRequestHeaders.Add("speed", "");
                        client.DefaultRequestHeaders.Add("voice", "thuminh");
                        var response = await client.PostAsync("https://api.fpt.ai/hmi/tts/v5", new StringContent(payload));
                        return await response.Content.ReadAsStringAsync();
                    }).GetAwaiter().GetResult();

                    var data = JsonConvert.DeserializeObject<ReturnData>(result);
                    var folder = AppDomain.CurrentDomain.BaseDirectory;
                    var fileName = $"{folder}{Guid.NewGuid()}.mp3";


                    using (var client = new WebClient())
                    {
                        client.DownloadFile(data.async, fileName);
                    }

                    Thread.Sleep(2000); // wait 2 seconds to download successfully
                    NotifyLabel.Content = "Download finished";

                    var player = new MediaPlayer();
                    player.Open(new Uri(fileName, UriKind.Absolute));

                    NotifyLabel.Content = "Playing";
                    player.MediaEnded += Player_MediaEnded;
                    player.Play();
                }
                else
                {
                    NotifyLabel.Content = "Text is invalid";
                }
            }
            
        }

        private void Player_MediaEnded(object sender, EventArgs e)
        {
            NotifyLabel.Content = "Play Finished";
        }
    }
}
