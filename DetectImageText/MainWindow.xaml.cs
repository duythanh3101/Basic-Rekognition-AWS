using Amazon.Rekognition;
using Amazon.Rekognition.Model;
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

namespace DectectImageText
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
            Console.WriteLine("show");
            string defaultURL = @"C:\Users\Admin\Desktop\images\anh-tra-sua.jpg";
            String photo = txtURL.Text == string.Empty ? defaultURL : txtURL.Text;

            var client = new AmazonRekognitionClient();
            var source = ToByteStream(photo);
            var request = new DetectTextRequest()
            {
                Image = source
            };

            try
            {
                var response = client.DetectText(request);
                Console.WriteLine("Detected labels for " + photo);
                int count = 1;
                txtInfo.Text = string.Empty;
                foreach (var label in response.TextDetections)
                {
                    txtInfo.Text += count++ + "/" + label.DetectedText + " - " + label.Confidence +"\n";
                }
            }
            catch (Exception)
            {
            }

        }

        static Image ToByteStream(string fileName)
        {
            var image = new Image();

            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                var data = new byte[fs.Length];
                fs.Read(data, 0, (int)fs.Length);
                image.Bytes = new MemoryStream(data);
            }

            return image;
        }

        public void RemoveRectangleChildren()
        {
            LayoutRoot.Children.Clear();
        }

        private void OpenDialog_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.png";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (openFileDialog.ShowDialog() == true)
            {
                if (openFileDialog.FileName != string.Empty)
                {
                    RemoveRectangleChildren();
                    txtURL.Text = openFileDialog.FileName;
                }
            }
        }

        private void TxtURL_TextChanged(object sender, TextChangedEventArgs e)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(txtURL.Text, UriKind.Absolute);
            bitmap.EndInit();

            imageSource.Source = bitmap;

            LayoutRoot.Children.Add(imageSource);
        }
    }
}
