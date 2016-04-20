using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
//using System.Runtime.InteropServices;
//using System.Timers;
//using System.Threading;
//using System.Threading.Tasks;



namespace MatchTheColors
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
            UpdateUserColor();
            UpdateUserTextFields();

            SetUpRandomColor();

            //this.Cursor = Cursors.Hand;
        }



        private void Slider_R_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateUserTextFields();

            UpdateUserColor();
        }

        private void Slider_G_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateUserTextFields();

            UpdateUserColor();
        }

        private void Slider_B_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateUserTextFields();

            UpdateUserColor();
        }


        void UpdateUserTextFields()
        {
            TextBox_R_Dec.Text = Slider_R.Value.ToString();
            TextBox_G_Dec.Text = Slider_G.Value.ToString();
            TextBox_B_Dec.Text = Slider_B.Value.ToString();

            TextBox_R_Hex.Text = String.Format("{0:X2}", (int)Slider_R.Value);
            TextBox_G_Hex.Text = String.Format("{0:X2}", (int)Slider_G.Value);
            TextBox_B_Hex.Text = String.Format("{0:X2}", (int)Slider_B.Value);

            TextBox_HTML_Code.Text = String.Format("#{0:X2}{1:X2}{2:X2}", (int)Slider_R.Value, (int)Slider_G.Value, (int)Slider_B.Value);
        }


        void RevealRandomTextFields()
        {
            TextBox_R_Dec_Random.Text = ((int)randomColor.R).ToString();
            TextBox_G_Dec_Random.Text = ((int)randomColor.G).ToString();
            TextBox_B_Dec_Random.Text = ((int)randomColor.B).ToString();

            TextBox_R_Hex_Random.Text = String.Format("{0:X2}", (int)randomColor.R);
            TextBox_G_Hex_Random.Text = String.Format("{0:X2}", (int)randomColor.G);
            TextBox_B_Hex_Random.Text = String.Format("{0:X2}", (int)randomColor.B);

            TextBox_HTML_Code_Random.Text = String.Format("#{0:X2}{1:X2}{2:X2}", (int)randomColor.R, (int)randomColor.G, (int)randomColor.B);

            var totalError = GetTotalError();

            if(totalError == 0)
                TotalError_Label.Content = "You've guessed perfectly!!!";
            else
                TotalError_Label.Content = "Total error = " + GetTotalError();

            TotalError_Label.Visibility = Visibility.Visible;
        }



        void HideRandomTextFields()
        {
            var tripleQM = "???";
            var hexQM = "0x??";
            var htmlQM = "#??????";

            TextBox_R_Dec_Random.Text = tripleQM;
            TextBox_G_Dec_Random.Text = tripleQM;
            TextBox_B_Dec_Random.Text = tripleQM;

            TextBox_R_Hex_Random.Text = hexQM;
            TextBox_G_Hex_Random.Text = hexQM;
            TextBox_B_Hex_Random.Text = hexQM;

            TextBox_HTML_Code_Random.Text = htmlQM;

            TotalError_Label.Visibility = Visibility.Hidden;
        }



        void UpdateUserColor()
        {
            Bitmap bmp = new Bitmap((int)UserColor.Width, (int)UserColor.Height);
            System.Drawing.Color color = System.Drawing.Color.FromArgb((byte)Slider_R.Value, (byte)Slider_G.Value, (byte)Slider_B.Value);
            
            for (int x = 0; x < bmp.Width; x++)
                for (int y = 0; y < bmp.Height; y++)
                    bmp.SetPixel(x, y, color);


            UserColor.Source = BitmapToImageSource(bmp);
        }


        void UpdateRandomColor()
        {
            Bitmap bmp = new Bitmap((int)RandomColor.Width, (int)RandomColor.Height);
            //System.Drawing.Color color = System.Drawing.Color.FromArgb((byte)Slider_R.Value, (byte)Slider_G.Value, (byte)Slider_B.Value);

            for (int x = 0; x < bmp.Width; x++)
                for (int y = 0; y < bmp.Height; y++)
                    bmp.SetPixel(x, y, randomColor);


            RandomColor.Source = BitmapToImageSource(bmp);
        }


        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        private void Answer_button_Click(object sender, RoutedEventArgs e)
        {
            RevealRandomTextFields();
        }

        static Random rnd = new Random();
        static System.Drawing.Color randomColor;
        void SetUpRandomColor()
        {
            randomColor = System.Drawing.Color.FromArgb((byte)rnd.Next(255), (byte)rnd.Next(255), (byte)rnd.Next(255));

            UpdateRandomColor();

            HideRandomTextFields();
        }

        private void NextRandomColor_button_Click(object sender, RoutedEventArgs e)
        {
            SetUpRandomColor();
        }

        int GetTotalError()
        {
            return (int)
                (
                    Math.Abs(Slider_R.Value - randomColor.R) +
                    Math.Abs(Slider_G.Value - randomColor.G) +
                    Math.Abs(Slider_B.Value - randomColor.B)
                );
        }
    }
}
