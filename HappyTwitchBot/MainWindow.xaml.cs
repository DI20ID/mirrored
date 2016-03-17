using System;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Diagnostics;
using System.Net;


namespace HappyTwitchBot
{
    public partial class MainWindow : Window
    {
        internal ircClient irc = new ircClient();
        internal string sUsername = "";
        internal string sPassword = "";
        internal string channel = "";
        public string logging="";


        // entry point
        public MainWindow()
        {
            InitializeComponent();
            l_passwordlink.Content = ircPatterns.passwordlink;
            tb_username.Text = "secrethappyliooon";        //temporary username - DELETE ON FINAL RELEASE
            tb_password.Text = "oauth:fb54elmxy4hfrcasoph8rxux7blv4h";         //temporary password - DELETE ON FINAL RELEASE
            tb_channel.Text = "riotgames";
            l_username.Opacity = 0;
            l_password.Opacity = 0;
            l_channel.Opacity = 0;
        }

        #region EVENTHANDLER
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        // connect button click
        private void b_connect_Click(object sender, RoutedEventArgs e)
        {

            g_connect.IsEnabled = false;



            sUsername = tb_username.Text;
            sPassword = tb_password.Text;
            channel = tb_channel.Text;


            // irc = new ircClient("irc.twitch.tv", 443, sUsername, sPassword);
            WebClient serverapi = new WebClient();
            string serverlist = serverapi.DownloadString("http://tmi.twitch.tv/servers?channel=" + channel);
            serverlist = serverlist.Replace(@"{""cluster"":""event"",""servers"":[""","");
            serverlist = serverlist.Replace(@"{""cluster"":""main"",""servers"":[""", "");
            serverlist = serverlist.Split('"')[0];
            string ip = serverlist.Split(':')[0];
            int port = Int32.Parse(serverlist.Split(':')[1]);



            irc = new ircClient(ip, port, sUsername, sPassword);
            irc.RequestMembership();
            irc.RequestTags();
            irc.joinRoom("riotgames");
            int sleep = 0;
            bool connected = false;

            while (connected == false && sleep <= 1000)
            {
                if(irc.tcpClient.Available!=0)
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
        private void testbutton_Click(object sender, RoutedEventArgs e)
        {
            irc.breakpoint();
        }

          


        // textbox username TextChanged
        private void tb_username_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        // textbox username GotFocus
        private void tb_username_GotFocus(object sender, RoutedEventArgs e)
        {
            l_username.Opacity = 0;
        }


        // textbox password GotFocus
        private void tb_password_GotFocus(object sender, RoutedEventArgs e)
        {
            l_password.Opacity = 0;
        }


        // textbox channel GotFocus
        private void tb_channel_GotFocus(object sender, RoutedEventArgs e)
        {
            l_channel.Opacity = 0;
        }

        //textbox username LostFocus
        private void tb_username_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tb_username.Text == "")
            {
                l_username.Opacity = 0.5;
            }
        }

        //textbox password LostFocus
        private void tb_password_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tb_password.Text == "")
            {
                l_password.Opacity = 0.5;
            }
        }

        //textbox channel LostFocus
        private void tb_channel_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tb_channel.Text == "")
            {
                l_channel.Opacity = 0.5;
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

        public void WriteLog(string log)
        {
            tb_test.AppendText(log);
            tb_test.AppendText("\n");
        }



        #endregion


    }
}
