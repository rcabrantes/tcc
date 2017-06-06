using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Gpio;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Windows.UI.Xaml.Shapes;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TCC
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static readonly HttpClient client = new HttpClient();


        private bool GPIOLoaded = false;

        private DispatcherTimer timer;
        private SolidColorBrush redBrush = new SolidColorBrush(Windows.UI.Colors.Red);
        private SolidColorBrush grayBrush = new SolidColorBrush(Windows.UI.Colors.LightGray);

        private List<Ellipse> Leds = new List<Ellipse>(); // initializing the list
        private List<int> portNums = new List<int>();  // initializing the list
        private List<GpioPin> ports = new List<GpioPin>(); // initializing the list


        public MainPage()
        {
            this.InitializeComponent();

            Leds = new List<Ellipse>();
            Leds.Add(Port1);
            Leds.Add(Port2);
            Leds.Add(Port3);
            Leds.Add(Port4);

            portNums = new List<int>();
            portNums.Add(5); //gpio 5
            portNums.Add(6); //gpio 6
            portNums.Add(13); //gpio 13
            portNums.Add(19); //gpio 19

            InitGPIO(); //initializing the GPIO

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += Timer_Tick;

            timer.Start();



        }


        private async void Timer_Tick(object sender, object e)
        {
            var response = await client.GetAsync("http://rsinohara.com/home/getGPIOPortStatus");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JArray.Parse(result);

                int i = 0;
                foreach (var port in data)
                {
                    if (bool.Parse(port.ToString()))
                    {
                        Leds[i].Fill = redBrush;
                        if (GPIOLoaded)
                        {
                            ports[i].Write(GpioPinValue.Low); //low state deactivate the relays
                        }
                    }
                    else
                    {
                        Leds[i].Fill = grayBrush;
                        if (GPIOLoaded)
                        {
                            ports[i].Write(GpioPinValue.High); //high state activate the relays
                        }

                    }
                    i++;
                }

            }
            else
            {
                GpioStatus.Text = "Connection to server failed.";

            }

        }


        private void InitGPIO()
        {
            var gpio = GpioController.GetDefault();


            // Show an error if there is no GPIO controller
            if (gpio == null)
            {
                GpioStatus.Text = "There is no GPIO controller on this device.";
                return;
            }

            foreach (var portNum in portNums)
            {
                var port = gpio.OpenPin(portNum);
                ports.Add(port);
                port.Write(GpioPinValue.High); //high state, deactivated relays
                port.SetDriveMode(GpioPinDriveMode.Output);
                GPIOLoaded = true;

            }

            GpioStatus.Text = "GPIO pins initialized correctly.";

        }

        private void DelayText_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
