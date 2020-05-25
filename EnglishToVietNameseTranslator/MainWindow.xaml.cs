using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Amazon.Translate;
using Amazon.Translate.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using Image = Amazon.Rekognition.Model.Image;
using Label = Amazon.Rekognition.Model.Label;

namespace EnglishToVietNameseTranslator
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
            
            var client = new AmazonTranslateClient();
            var request = new TranslateTextRequest()
            {
                Text = sourceText,
                SourceLanguageCode = "en",
                TargetLanguageCode = "vi"
            };

            try
            {
                var response = client.TranslateText(request);
                txtInfo.Text = string.Empty;
                txtInfo.Text = response.TranslatedText;
            }
            catch (Exception)
            {
            }

        }
    }
}
