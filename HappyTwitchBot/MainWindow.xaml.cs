using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Threading;


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

        }

        public void WriteLog(string log)
        {
            tb_test.AppendText(log);
            tb_test.AppendText("\n");
        }

        #region EVENTHANDLER

        // connect button click
        private void b_connect_Click(object sender, RoutedEventArgs e)
        {

            g_connect.IsEnabled = false;


            sUsername = "secrethappyliooon";        //temporary username - DELETE ON FINAL RELEASE
            sPassword = "oauth:fb54elmxy4hfrcasoph8rxux7blv4h";         //temporary password - DELETE ON FINAL RELEASE
            //sUsername = tb_password.Text;
            //sPassword = tb_password.Text;
            
            irc = new ircClient("irc.twitch.tv", 6667, sUsername, sPassword);
            irc.joinRoom("lirik");
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
                Thread ircThread = new Thread(irc.ContinousRead);
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
            
        }


        // textbox password GotFocus
        private void tb_password_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }


        // textbox channel GotFocus
        private void tb_channel_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }

        #endregion

        #region FUNCTIONS



        #endregion

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

    }
    }
}
