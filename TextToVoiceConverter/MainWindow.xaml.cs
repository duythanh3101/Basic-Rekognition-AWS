using Amazon.Polly;
using Amazon.Polly.Model;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace TextToVoiceConverter
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var sourceText = txtSoure.Text;
            
            var client = new AmazonPollyClient();
            var request = new SynthesizeSpeechRequest()
            {
                Text = sourceText,
                OutputFormat = OutputFormat.Mp3,
                VoiceId = VoiceId.Emma
                
            };

      
            var response = client.SynthesizeSpeech(request);

            var folder = AppDomain.CurrentDomain.BaseDirectory;
            var fileName = $"{folder}{Guid.NewGuid()}.mp3";

            using (var fs = File.Create(fileName))
            {
                response.AudioStream.CopyTo(fs);
                fs.Flush();
                fs.Close();
            }

            var player = new MediaPlayer();
            player.Open(new Uri(fileName, UriKind.Absolute));
            player.MediaEnded += (s2, e2) => Title = "Player ended";
            player.Play();

            Title = "Playing synthesized audio";
        }
    }
}
