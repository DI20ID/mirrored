using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Diagnostics;


namespace HappyTwitchBot
{
    public partial class MainWindow : Window
    {
        internal ircClient irc = new ircClient();
        internal string sUsername = "";
        internal string sPassword = "";
        public string logging="";


        // entry point
        public MainWindow()
        {
            InitializeComponent();
            l_passwordlink.Content = ircPatterns.passwordlink;
        }



        #region EVENTHANDLER
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        // connect button click
        private void b_connect_Click(object sender, RoutedEventArgs e)
        {

            g_connect.IsEnabled = false;


            sUsername = "secrethappyliooon";        //temporary username - DELETE ON FINAL RELEASE
            sPassword = "oauth:fb54elmxy4hfrcasoph8rxux7blv4h";         //temporary password - DELETE ON FINAL RELEASE
            //sUsername = tb_password.Text;
            //sPassword = tb_password.Text;
            
            irc = new ircClient("irc.twitch.tv", 6667, sUsername, sPassword);
            irc.RequestMembership();
            irc.joinRoom("nikolarntv");
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
            irc.sendChatMessage("It works!");
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
