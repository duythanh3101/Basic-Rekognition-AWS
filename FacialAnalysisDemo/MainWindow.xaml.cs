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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Image = Amazon.Rekognition.Model.Image;
using Label = Amazon.Rekognition.Model.Label;

namespace FacialAnalysisDemo
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
            string defaultURL = @"C:\Users\Admin\Desktop\121-1217434_transparent-crowd-of-people-png-people-walking-illustration.png";
            String photo = txtURL.Text == string.Empty? defaultURL : txtURL.Text;
            

            Amazon.Rekognition.Model.Image image = new Amazon.Rekognition.Model.Image();
            try
            {
                using (FileStream fs = new FileStream(photo, FileMode.Open, FileAccess.Read))
                {
                    byte[] data = null;
                    data = new byte[fs.Length];
                    fs.Read(data, 0, (int)fs.Length);
                    image.Bytes = new MemoryStream(data);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load file " + photo);
                return;
            }

            AmazonRekognitionClient rekognitionClient = new AmazonRekognitionClient();

            DetectLabelsRequest detectlabelsRequest = new DetectLabelsRequest()
            {
                Image = image,
                MaxLabels = 30,
                MinConfidence = 40F
            };

            try
            {
                DetectLabelsResponse detectLabelsResponse = rekognitionClient.DetectLabels(detectlabelsRequest);
                //Console.WriteLine("Label " + detectLabelsResponse.Labels);
                //int count = 0;
                foreach (Label label in detectLabelsResponse.Labels)
                {
                    Console.WriteLine("{0}: {1}", label.Name, label.Confidence);
                    if (label.Instances != null && label.Instances.Count > 0)
                    {
                        foreach (var item in label.Instances)
                        {
                            if (item.BoundingBox != null)
                            {
                                double width = item.BoundingBox.Width;
                                double height = item.BoundingBox.Height;
                                double left = item.BoundingBox.Left;
                                double top = item.BoundingBox.Top;

                                LayoutRoot.Children.Add(CreateARectangle(width* imageSource.Width, height* imageSource.Height, left*imageSource.Width, top*imageSource.Height));
                                //Console.WriteLine("Bouding box: " + item.BoundingBox.Top);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                //Console.WriteLine(e.Message);
            }
            //LayoutRoot.Children.Add(CreateARectangle(0.5*300, 0.3*200, ));
            //LayoutRoot.Children.Add(CreateARectangle(0.1*300, 0.2*200));

        }

        public void RemoveRectangleChildren()
        {
            LayoutRoot.Children.Clear();
        }

        public static Rectangle CreateARectangle(double width, double height, double left, double top)
        {
            // Create a Rectangle  
            Rectangle blueRectangle = new Rectangle();
            blueRectangle.Height = height;
            blueRectangle.Width = width;
            // Create a blue and a black Brush  
            SolidColorBrush blueBrush = new SolidColorBrush();
            blueBrush.Color = Colors.Transparent;
            SolidColorBrush blackBrush = new SolidColorBrush();
            blackBrush.Color = Colors.Red;
            // Set Rectangle's width and color  
            blueRectangle.StrokeThickness = 1;
            blueRectangle.Stroke = blackBrush;
            // Fill rectangle with blue color  
            blueRectangle.Fill = blueBrush;
            // Add Rectangle to the Grid.  
            blueRectangle.Margin = new Thickness(left, top, 0, 0);
            blueRectangle.VerticalAlignment = VerticalAlignment.Center;
            blueRectangle.HorizontalAlignment = HorizontalAlignment.Center;
            
            //LayoutRoot.Children.Add(blueRectangle);
            return blueRectangle;
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
