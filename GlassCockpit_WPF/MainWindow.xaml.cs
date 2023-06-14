using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Windows.Markup;
using System.Linq.Expressions;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Device.Location;


namespace GlassCockpit_WPF
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        static SerialPort _serialport;
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer dispatcherTimer1 = new System.Windows.Threading.DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            define_serial();
            dispatcherTimer1.Start();
            dispatcherTimer1.Tick += new EventHandler(dispatcherTimer_Tick_read);
            dispatcherTimer1.Interval = TimeSpan.FromMilliseconds(1);
            this.SizeToContent = SizeToContent.WidthAndHeight;
        }
        #region Sliders

        // PRĘDKOŚCI If Velocity 800 - Indicator 53,33
        double X;
        public void Velocity(double Velocity)
        {

            RotateTransform rotationTransform = new RotateTransform(X * 3.6 + 175, 0, -0);
            Needle_AirSpeed.RenderTransform = rotationTransform;
            Rectangle_VSI.RenderTransform = new TranslateTransform(0, Velocity);
            X = Velocity / 15;
            TextBlock_VSI.Text = X.ToString();
        }


        private void Slider_Test_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //rotation(Needle_AirSpeed,10,10,180+e.NewValue);
            Velocity(e.NewValue);
        }
        public double roll_angle;
        // KĄTY POCHYLENIA I PRZECHYLENIA 48/49 - 25 (e)
        public void RotateAllPFDs(double roll_angle, double pitch_angle)
        {
            //Dobrze
            RotateTransform rotationTransformTop = new RotateTransform(roll_angle * 0.5, 0, 0);
            RotateTransform rotationTransformDown = new RotateTransform(roll_angle * 0.5, 0, 0);
            RotateTransform rotationTransformRuler = new RotateTransform(roll_angle * 0.5, 0, 0);
            RotateTransform rotationTransformTest = new RotateTransform(roll_angle * 0.5, 0, 0);
            RotateTransform rotationTransformMid = new RotateTransform(roll_angle * 0.5);

            Triangle_test.RenderTransform = rotationTransformTest;
            /*Rectangle_PFD_Top.RenderTransform = rotationTransformTop;
            Rectangle_PFD_Down.RenderTransform = rotationTransformDown;
            Rectangle_Ruler.RenderTransform = rotationTransformRuler;
            */
            pitch_angle = pitch_angle * 5;

            TranslateTransform translateTransformTop = new TranslateTransform(0, pitch_angle);
            TranslateTransform translateTransformDown = new TranslateTransform(0, pitch_angle);
            TranslateTransform translateTransformRuler = new TranslateTransform(0, pitch_angle * 1.5);
            TranslateTransform translateTransformMid = new TranslateTransform(0, pitch_angle);

            TransformGroup transformGroupTop = new TransformGroup();
            transformGroupTop.Children.Add(rotationTransformTop);
            transformGroupTop.Children.Add(translateTransformTop);
            Rectangle_PFD_Top.RenderTransform = transformGroupTop;

            TransformGroup transformGroupDown = new TransformGroup();
            transformGroupDown.Children.Add(rotationTransformDown);
            transformGroupDown.Children.Add(translateTransformDown);
            Rectangle_PFD_Down.RenderTransform = transformGroupDown;

            TransformGroup transformGroupRuler = new TransformGroup();
            transformGroupRuler.Children.Add(rotationTransformRuler);
            transformGroupRuler.Children.Add(translateTransformRuler);
            Rectangle_Ruler.RenderTransform = transformGroupRuler;


            TransformGroup transformGroupMid = new TransformGroup();
            transformGroupMid.Children.Add(rotationTransformMid);
            transformGroupMid.Children.Add(translateTransformMid);
            Rectangle_Mid_PFD.RenderTransform = transformGroupMid;

        }
        public void RotateAllPFDsUART(double roll_angle, double pitch_angle, double mnoznik_roll, double mnoznik_pitch)
        {
            //Dobrze
            RotateTransform rotationTransformTop = new RotateTransform(roll_angle * mnoznik_roll, 0, 0);
            RotateTransform rotationTransformDown = new RotateTransform(roll_angle * mnoznik_roll, 0, 0);
            RotateTransform rotationTransformRuler = new RotateTransform(roll_angle * mnoznik_roll, 0, 0);
            RotateTransform rotationTransformTest = new RotateTransform(roll_angle * mnoznik_roll, 0, 0);
            RotateTransform rotationTransformMid = new RotateTransform(roll_angle * mnoznik_roll);

            Triangle_test.RenderTransform = rotationTransformTest;
            /*Rectangle_PFD_Top.RenderTransform = rotationTransformTop;
            Rectangle_PFD_Down.RenderTransform = rotationTransformDown;
            Rectangle_Ruler.RenderTransform = rotationTransformRuler;
            */
            pitch_angle = pitch_angle * 5;

            TranslateTransform translateTransformTop = new TranslateTransform(0, pitch_angle * mnoznik_pitch);
            TranslateTransform translateTransformDown = new TranslateTransform(0, pitch_angle * mnoznik_pitch);
            TranslateTransform translateTransformRuler = new TranslateTransform(0, pitch_angle * mnoznik_pitch * 1.5);
            TranslateTransform translateTransformMid = new TranslateTransform(0, pitch_angle * mnoznik_pitch);

            TransformGroup transformGroupTop = new TransformGroup();
            transformGroupTop.Children.Add(rotationTransformTop);
            transformGroupTop.Children.Add(translateTransformTop);
            Rectangle_PFD_Top.RenderTransform = transformGroupTop;

            TransformGroup transformGroupDown = new TransformGroup();
            transformGroupDown.Children.Add(rotationTransformDown);
            transformGroupDown.Children.Add(translateTransformDown);
            Rectangle_PFD_Down.RenderTransform = transformGroupDown;

            TransformGroup transformGroupRuler = new TransformGroup();
            transformGroupRuler.Children.Add(rotationTransformRuler);
            transformGroupRuler.Children.Add(translateTransformRuler);
            Rectangle_Ruler.RenderTransform = transformGroupRuler;


            TransformGroup transformGroupMid = new TransformGroup();
            transformGroupMid.Children.Add(rotationTransformMid);
            transformGroupMid.Children.Add(translateTransformMid);
            Rectangle_Mid_PFD.RenderTransform = transformGroupMid;

            if (roll_angle > 0)
            {
                Ball.RenderTransform = new TranslateTransform(roll_angle, -roll_angle * 0.1);
            }
            else
            {
                Ball.RenderTransform = new TranslateTransform(roll_angle, roll_angle * 0.1);
            }
            rotate_rect(TurnCoorn_Aircraft, roll_angle * 0.5);



        }
        private void Slider_Roll_Angle_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            roll_angle = e.NewValue;

            RotateAllPFDs(roll_angle, pitch_angle);
            if (e.NewValue > 0)
            {
                Ball.RenderTransform = new TranslateTransform(e.NewValue, -e.NewValue * 0.1);
            }
            else
            {
                Ball.RenderTransform = new TranslateTransform(e.NewValue, e.NewValue * 0.1);
            }
            rotate_rect(TurnCoorn_Aircraft, roll_angle * 0.5);


        }
        public void rotate_rect(Rectangle rectangle, double angle)
        {
            RotateTransform rotation = new RotateTransform(angle);
            rectangle.RenderTransform = rotation;
        }

        double pitch_angle;
        public void TranslateAllPFDs(double pitch_angle)
        {

            Rectangle_PFD_Top.RenderTransform = new TranslateTransform(0, pitch_angle);
            Rectangle_PFD_Down.RenderTransform = new TranslateTransform(0, pitch_angle);
            //Rectangle_PFD_Down_bottom.RenderTransform = new TranslateTransform(0, -e.NewValue * 10);
            //Rectangle_PFD_Top_bottom.RenderTransform = new TranslateTransform(0, -e.NewValue * 10);
            Rectangle_Ruler.RenderTransform = new TranslateTransform(0, pitch_angle * 10);
            Rectangle_Mid_PFD.RenderTransform = new TranslateTransform(0, pitch_angle);
            //Rectangle_Ruler.Height = Rectangle_Ruler.Height - e.NewValue*0.01;

        }

        private void Slider_Roll_Angle_Copy2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            pitch_angle = e.NewValue;
            //TranslateAllPFDs(pitch_angle);
            RotateAllPFDs(roll_angle, pitch_angle);

        }



        private void Slider_Kurs_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Ellipse_Horizontal_Situation.RenderTransform = new RotateTransform(e.NewValue);
            double kat = 360 - e.NewValue;
            TextBlock_Horizontal.Text = kat.ToString();
        }
        double Alt;
        public void Altimeter(double Alti, double mnoznik)
        {
            Rectangle_Altimeter.RenderTransform = new TranslateTransform(0, Alti);
            Alt = Alti * mnoznik;

            TextBlock_Altimeter.Text = Convert.ToInt16(Alti * mnoznik).ToString();
            int Altitude = Convert.ToInt32(Alt);
            rotate_rect(Alt_Clas_Needle_Big, Alt * 0.36);
            rotate_rect(Alt_Clas_Needle_Small, Alt * 0.36 * 0.1);

            int Alt1 = Altitude % 10;
            int Alt10 = (Altitude % 100 - Alt1) / 10;
            int Alt100 = (Altitude % 1000 - Alt10 * 10 - Alt1) / 100;
            int Alt1000 = (Altitude - Alt100 * 100 - Alt10 * 10 - Alt1) / 1000;
            Band1000.RenderTransform = new TranslateTransform(0, -Alt1000 * 15);
            Band100.RenderTransform = new TranslateTransform(0, -Alt100 * 15);
            Band10.RenderTransform = new TranslateTransform(0, -Alt10 * 15);
            Band1.RenderTransform = new TranslateTransform(0, -Alt1 * 15);// Alt=15 to linijka idzie w dol
        }
        private void Slider_Altitude_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Altimeter(e.NewValue, 1.165);

        }
        int VerticalVelocity;
        private void Slider_Vertical_Velocity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Textblock_VV.RenderTransform = new TranslateTransform(0, e.NewValue * 10);
            Rectangle_VV_Indicate.RenderTransform = new TranslateTransform(0, e.NewValue * 10);
            VerticalVelocity = Convert.ToInt16(e.NewValue * -10000 / 36);
            Textblock_VV.Text = VerticalVelocity.ToString();
            rotate_rect(VV_Needle, VerticalVelocity / 45 - 90);
        }
        #endregion
        #region UART
        private void Button_UART_Click(object sender, RoutedEventArgs e)
        {
            if (Button_UART.Content == "Sliders")
            {
                Light_UART.Fill = new SolidColorBrush(Colors.Red);
                Light_serial.Fill = new SolidColorBrush(Colors.Red);
                Light_Sliders.Fill = new SolidColorBrush(Colors.Green);
                Button_UART.Content = "UART";
                Slider_Altitude.IsEnabled = true;
                Slider_Altitude.Visibility = Visibility.Visible;
                Label_Altitude.Visibility = Visibility.Visible;
                TextBlock_Altitude.Visibility = Visibility.Collapsed;

                Slider_Kurs.IsEnabled = true;
                Slider_Kurs.Visibility = Visibility.Visible;
                Label_Kurs.Visibility = Visibility.Visible;
                TextBlock_kurs.Visibility = Visibility.Collapsed;

                Slider_Pitch_Angle.IsEnabled = true;
                Slider_Pitch_Angle.Visibility = Visibility.Visible;
                Label_PitchAngle.Visibility = Visibility.Visible;
                TextBlock_PitchAngle.Visibility = Visibility.Collapsed;

                Slider_Roll_Angle.IsEnabled = true;
                Slider_Roll_Angle.Visibility = Visibility.Visible;
                Label_RollAngle.Visibility = Visibility.Visible;
                TextBlock_RollAngle.Visibility = Visibility.Collapsed;

                Slider_Vertical_Velocity.IsEnabled = true;
                Slider_Vertical_Velocity.Visibility = Visibility.Visible;
                Label_VV.Visibility = Visibility.Visible;
                TextBlock_VV.Visibility = Visibility.Collapsed;

                Slider_Test.IsEnabled = true;
                Slider_Test.Visibility = Visibility.Visible;
                Label_Velocity.Visibility = Visibility.Visible;
                TextBlock_Velocity.Visibility = Visibility.Collapsed;
            }
            else
            {
                Button_UART.Content = "Sliders";
                Light_UART.Fill = new SolidColorBrush(Colors.Green);
                Light_Sliders.Fill = new SolidColorBrush(Colors.Red);

                Slider_Altitude.IsEnabled = false;
                Slider_Altitude.Visibility = Visibility.Collapsed;
                TextBlock_Altitude.Visibility = Visibility.Visible;

                Slider_Kurs.IsEnabled = false;
                Slider_Kurs.Visibility = Visibility.Collapsed;
                TextBlock_kurs.Visibility = Visibility.Visible;

                Slider_Pitch_Angle.IsEnabled = false;
                Slider_Pitch_Angle.Visibility = Visibility.Collapsed;
                TextBlock_PitchAngle.Visibility = Visibility.Visible;

                Slider_Roll_Angle.IsEnabled = false;
                Slider_Roll_Angle.Visibility = Visibility.Collapsed;
                TextBlock_RollAngle.Visibility = Visibility.Visible;

                Slider_Vertical_Velocity.IsEnabled = false;
                Slider_Vertical_Velocity.Visibility = Visibility.Collapsed;
                TextBlock_VV.Visibility = Visibility.Visible;

                Slider_Test.IsEnabled = false;
                Slider_Test.Visibility = Visibility.Collapsed;
                TextBlock_Velocity.Visibility = Visibility.Visible;
                //////////////////////////////////////////////////////////
                
                dispatcherTimer.Start();
                dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1);
            }
        }

        public string pitch;
        public string roll;
        public double roll_d = 0.0;
        public double pitch_d = 0.0;
        private void dispatcherTimer_Tick_read(object sender, EventArgs e)
        {
            
            if(_serialport.ReadLine().Length>0)
            {
                
                string line = _serialport.ReadLine();
                string kine = _serialport.ReadLine();
                if (line[0] == 'a')
                {
                    roll = line;
                    pitch = kine;

                    roll = Regex.Replace(roll, @"^\s*$-.\n|\r", String.Empty, RegexOptions.Multiline).TrimEnd();
                    roll = roll.Remove(0, 1);

                    pitch = Regex.Replace(pitch, @"^\s*$-.\n|\r", String.Empty, RegexOptions.Multiline).TrimEnd();
                    pitch = pitch.Remove(0, 1);

                }
                else if (line[0] == 'b')
                {
                    pitch = line;
                    roll = kine;

                    pitch = Regex.Replace(pitch, @"^\s*$-.\n|\r", String.Empty, RegexOptions.Multiline).TrimEnd();
                    pitch = pitch.Remove(0, 1);

                    roll = Regex.Replace(roll, @"^\s*$-.\n|\r", String.Empty, RegexOptions.Multiline).TrimEnd();
                    roll = roll.Remove(0, 1);

                }
            }
            

            if (Double.TryParse(roll,NumberStyles.Number,CultureInfo.InvariantCulture, out double d))
            {
                if (roll.Length > 4)
                { roll = roll.Remove(4); }
                roll_d = Convert.ToDouble(roll, CultureInfo.InvariantCulture);
            }
            else { roll_d = 0.0; };

            if (Double.TryParse(pitch, NumberStyles.Number, CultureInfo.InvariantCulture, out double f))
            {
                if (pitch.Length > 4)
                { pitch = pitch.Remove(4); }
                pitch_d = Convert.ToDouble(pitch, CultureInfo.InvariantCulture);
            }
            else { pitch_d=0.0; };
            if (pitch_d > 60)
            {
                pitch_d = 60;
            }
            if (pitch_d < -60)
            {
                pitch_d = -60;
            }

            if (roll_d > 35)
            {
                roll_d = 35;
            }
            if (roll_d < -35)
            {
                roll_d = -35;
            }

        }
        public int serialport=0; 
       
        private void define_serial()
        {
            _serialport = new SerialPort();
            _serialport.BaudRate = 9600;
            _serialport.PortName = "COM7";
            _serialport.Parity = Parity.None;
            _serialport.DataBits = 8;
            _serialport.StopBits = StopBits.One;
            try
            {
                _serialport.Open();
                Light_serial.Fill = new SolidColorBrush(Colors.Green);
                serialport = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            TextBlock_PitchAngle.Text = pitch_d.ToString();
            TextBlock_RollAngle.Text = roll_d.ToString();
            RotateAllPFDsUART(roll_d, pitch_d, -0.65, 0.51);
            TextBlock_Altimeter.Text= "361";
            TextBlock_Altitude.Text = "361";
            TextBlock_kurs.Text = "No data";
            TextBlock_VV.Text = "No data";
            TextBlock_Velocity.Text = "No data";
            Altimeter(361, 1);

        }
            #endregion
        
    }
}
