using System;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;


namespace HappyTwitchBot
{
    public partial class MainWindow : Window
    {
        internal ircClient irc = new ircClient();
        internal string sUsername = "";
        internal string sPassword = "";
        internal string channel = "";
        public string logging = "";

        internal LED led = new LED();



        // entry point
        public MainWindow()
        {
            InitializeComponent();
            l_passwordlink.Content = ircPatterns.passwordlink;
            tb_username.Text = "CHANGEME";        //temporary username - DELETE ON FINAL RELEASE
            tb_password.Password = "CHANGEME";         //temporary password - DELETE ON FINAL RELEASE
            tb_channel.Text = "CHANGEME";
            
        }

        #region EVENTHANDLER
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        // connect button click
        private void b_connect_Click(object sender, RoutedEventArgs e)
        {

            g_connect.IsEnabled = false;



            sUsername = tb_username.Text;
            sPassword = tb_password.Password;
            channel = tb_channel.Text;


            // irc = new ircClient("irc.twitch.tv", 443, sUsername, sPassword);
            WebClient serverapi = new WebClient();
            string serverlist = serverapi.DownloadString("http://tmi.twitch.tv/servers?channel=" + channel);
            serverlist = serverlist.Replace(@"{""cluster"":""event"",""servers"":[""", "");
            serverlist = serverlist.Replace(@"{""cluster"":""main"",""servers"":[""", "");
            serverlist = serverlist.Replace(@"{""cluster"":""aws"",""servers"":[""", "");
            serverlist = serverlist.Split('"')[0];
            //string ip = serverlist.Split(':')[0];
            string ip = "irc.chat.twitch.tv";
            //int port = Int32.Parse(serverlist.Split(':')[1]);
            int port = 80;



            irc = new ircClient(ip, port, sUsername, sPassword);
            irc.RequestMembership();
            irc.RequestTags();
            irc.joinRoom(channel);
            int sleep = 0;
            bool connected = false;

            while (connected == false && sleep <= 1000)
            {
                if (irc.tcpClient.Available != 0)
                { connected = true; }
                else
                {

                    sleep++;
                    Thread.Sleep(5);
                }
            }

            if (connected == true)
            {
                Thread ircThread = new Thread(irc.WatchDog);
                ircThread.Start();
            }
            else
            {
                MessageBox.Show("Connection Failed", "Error");
                g_connect.IsEnabled = true;
            }
        }


        // button test Click





        // textbox username TextChanged
        private void tb_username_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        // textbox username GotFocus
        private void tb_username_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }


        // textbox password GotFocus
        private void tb_password_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }


        // textbox channel GotFocus
        private void tb_channel_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }

        //textbox username LostFocus
        private void tb_username_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tb_username.Text == "")
            {
               
            }
        }

        //textbox password LostFocus
        private void tb_password_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tb_password.Password == "")
            {
               
            }
        }

        //textbox channel LostFocus
        private void tb_channel_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tb_channel.Text == "")
            {
                
            }
        }

        //link for password - Mouse Left Button Release - open browser
        private void l_passwordlink_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Process browser = new Process();
            browser.StartInfo.UseShellExecute = true;
            browser.StartInfo.FileName = ircPatterns.passwordlink;
            browser.Start();
        }

        #endregion

        #region FUNCTIONS



        public void controlLED(string ledIP, string ledPort, string ledR, string ledG, string ledB, string ledH, string led_context)
        {
            LED.ip = ledIP;
            LED.port = ledPort;
            LED.R = ledR;
            LED.G = ledG;
            LED.B = ledB;
            LED.H = ledH;
            LED.led = led_context;
            try
            {
                led = new LED(LED.ip, LED.port);
                Thread ControlLEDThread = new Thread(led.sendSettings);
                ControlLEDThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool IsValidIP(string addr)
        {
            //create our match pattern
            string pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
            //create our Regular Expression object
            Regex check = new Regex(pattern);
            //boolean variable to hold the status
            bool valid = false;
            //check to make sure an ip address was provided
            if (addr == "")
            {
                //no address provided so return false
                valid = false;
            }
            else
            {
                //address provided so use the IsMatch Method
                //of the Regular Expression object
                valid = check.IsMatch(addr, 0);
            }
            //return the results
            return valid;
        }

        public bool IsValidNumber(string str, int lowcap, int highcap)
        {
            int parsedValue;
            if (int.TryParse(str, out parsedValue))
            {
                if ((parsedValue >= lowcap) & (parsedValue <= highcap))
                {
                    return true;
                }
            }
            return false;
        }


        #endregion

        private void LEDconnect_Click(object sender, RoutedEventArgs e)
        {

        }



        private void led_apply_Click(object sender, RoutedEventArgs e)
        {
            controlLED(tb_ledIP.Text, tb_ledPort.Text, tb_ledR.Text, tb_ledG.Text, tb_ledB.Text, tb_ledH.Text, tb_led.Text);
        }

        // ----------- LED BUTTON START
        #region LED
        private void led_red_Click(object sender, RoutedEventArgs e)
        {
            tb_ledR.Text = "255";
            tb_ledG.Text = "0";
            tb_ledB.Text = "0";
            //led_apply_Click(sender,e);
        }
        private void led_green_Click(object sender, RoutedEventArgs e)
        {
            tb_ledR.Text = "0";
            tb_ledG.Text = "255";
            tb_ledB.Text = "0";
            //led_apply_Click(sender, e);
        }

        private void led_blue_Click(object sender, RoutedEventArgs e)
        {
            tb_ledR.Text = "0";
            tb_ledG.Text = "0";
            tb_ledB.Text = "255";
            //led_apply_Click(sender, e);
        }

        private void led_pink_Click(object sender, RoutedEventArgs e)
        {
            tb_ledR.Text = "255";
            tb_ledG.Text = "20";
            tb_ledB.Text = "147";
            //led_apply_Click(sender, e);
        }

        private void led_white_Click(object sender, RoutedEventArgs e)
        {
            tb_ledR.Text = "255";
            tb_ledG.Text = "255";
            tb_ledB.Text = "255";
            //led_apply_Click(sender, e);
        }

        private void led_orange_Click(object sender, RoutedEventArgs e)
        {
            tb_ledR.Text = "255";
            tb_ledG.Text = "140";
            tb_ledB.Text = "0";
            //led_apply_Click(sender, e);
        }

        private void led_yellow_Click(object sender, RoutedEventArgs e)
        {
            tb_ledR.Text = "255";
            tb_ledG.Text = "255";
            tb_ledB.Text = "0";
            //led_apply_Click(sender, e);
        }

        private void led_cyan_Click(object sender, RoutedEventArgs e)
        {
            tb_ledR.Text = "0";
            tb_ledG.Text = "255";
            tb_ledB.Text = "255";
            //led_apply_Click(sender, e);
        }

        private void led_purple_Click(object sender, RoutedEventArgs e)
        {
            tb_ledR.Text = "75";
            tb_ledG.Text = "0";
            tb_ledB.Text = "130";
            //led_apply_Click(sender, e);
        }
        #endregion



        private void tb_ledIP_TextChanged(object sender, TextChangedEventArgs e)
        {
            led_apply.IsEnabled = IsValidIP(tb_ledIP.Text);
        }

        private void tb_ledPort_TextChanged(object sender, TextChangedEventArgs e)
        {
            led_apply.IsEnabled = IsValidNumber(tb_ledPort.Text, 1, 65535);
        }

        private void tb_ledR_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool valid = IsValidNumber(tb_ledR.Text, 0, 255);
            led_apply.IsEnabled = valid;
            if (valid)
            {
                sl_R.Value = Int32.Parse(tb_ledR.Text);
            }
        }

        private void tb_ledG_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool valid = IsValidNumber(tb_ledG.Text, 0, 255);
            led_apply.IsEnabled = valid;
            if (valid)
            {
                sl_G.Value = Int32.Parse(tb_ledG.Text);
            }
        }

        private void tb_ledB_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool valid = IsValidNumber(tb_ledB.Text, 0, 255);
            led_apply.IsEnabled = valid;
            if (valid)
            {
                sl_B.Value = Int32.Parse(tb_ledB.Text);
            }
        }

        private void sl_led_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tb_ledH.Text = ((int)sl_led.Value).ToString();
        }

        private void tb_ledH_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool valid = IsValidNumber(tb_ledH.Text, 0, 255);
            led_apply.IsEnabled = valid;
            if (valid)
            {
                sl_led.Value = Int32.Parse(tb_ledH.Text);
            }
        }

        private void sl_R_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tb_ledR.Text = ((int)sl_R.Value).ToString();
        }

        private void sl_G_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tb_ledG.Text = ((int)sl_G.Value).ToString();
        }

        private void sl_B_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tb_ledB.Text = ((int)sl_B.Value).ToString();
        }

        private void cb_ledAll_Unchecked(object sender, RoutedEventArgs e)
        {
            tb_led.IsEnabled = true;
        }

        private void cb_ledAll_Checked(object sender, RoutedEventArgs e)
        {
            tb_led.IsEnabled = false;
        }

        private void cb_devices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_devices.SelectedValue.ToString().Contains("LED Wall"))
            {
                tb_ledH.Text = "255";
                tb_ledIP.Text = "10.0.0.136";
                tb_ledPort.Text = "80";
                tb_ledURL.Text = "/body";
            }
            if (cb_devices.SelectedValue.ToString().Contains("OnAir"))
            {
                tb_ledH.Text = "255";
                tb_ledIP.Text = "10.0.0.137";
                tb_ledPort.Text = "80";
                tb_ledURL.Text = "/body";
            }
        }

        private void b_lightsOn_Click(object sender, RoutedEventArgs e)
        {
            
            tb_ledIP.Text = "10.0.0.138";
            tb_ledPort.Text = "80";
            sl_R.Value = 0;
            sl_G.Value = 0;
            sl_B.Value = 0;
            tb_ledR.Text = "0";
            tb_ledG.Text = "0";
            tb_ledB.Text = "0";
            tb_ledH.Text = "50";
            tb_led.Text = "null";
            /*
            led_apply_Click(sender, e);
            */

            led_cyan_Click(sender, e);
            tb_ledH.Text = "100";
            tb_ledIP.Text = "10.0.0.137";
            led_apply_Click(sender, e);

            
            led_orange_Click(sender, e);
            tb_ledH.Text = "255";
            tb_ledIP.Text = "10.0.0.136";
            led_apply_Click(sender, e);

        }

        private void b_lightsOff_Click(object sender, RoutedEventArgs e)
        {
            
            tb_ledIP.Text = "10.0.0.136";

            tb_ledPort.Text = "80";
            sl_R.Value = 0;
            sl_G.Value = 0;
            sl_B.Value = 0;
            tb_ledH.Text = "0";
            tb_led.Text = "null";
            led_apply_Click(sender, e);

            tb_ledIP.Text = "10.0.0.137";
            led_apply_Click(sender, e);

            tb_ledIP.Text = "10.0.0.138";
            /*
            led_apply_Click(sender, e);
            */
            tb_ledIP.Text = "10.0.0.136";
        }
    }
}
