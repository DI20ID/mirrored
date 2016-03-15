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


        // entry point
        public MainWindow()
        {
            InitializeComponent();
        }


        #region EVENTHANDLER

        // connect button click
        private void b_connect_Click(object sender, RoutedEventArgs e)
        {
            /*
            b_connect.IsEnabled = false;
            tb_username.IsEnabled = false;
            tb_password.IsEnabled = false;
            tb_channel.IsEnabled = false;
            cb_remember.IsEnabled = false;
            */
            g_connect.IsEnabled = false;


            sUsername = "secrethappyliooon";
            sPassword = "oauth:fb54elmxy4hfrcasoph8rxux7blv4h";
            //sUsername = tb_password.Text;
            //sPassword = tb_password.Text;
            
            irc = new ircClient("irc.twitch.tv", 6667, sUsername, sPassword);
            irc.joinRoom("happyliooon");
            int sleep = 0;
            int connected = 0;
            int fail = 0;

            while (connected == 0 && sleep <= 1000)
            {
                if(irc.tcpClient.Available!=0)
                { connected = 1; }
                else
                {

                    sleep++;
                    Thread.Sleep(5);
                }
            }

            if (connected == 1)
            {

                tb_test.Text = "";

                for (int i = 0;i<10;i++)
                {
                   
                    tb_test.Text += "\n" + irc.readMessage();
                }

            }
            else
            {
                MessageBox.Show("Connection Failed", "Error");
                g_connect.IsEnabled = true;
            }
           
            /*while (true)
            {
                string message = irc.readMessage();
                if (message == "!hello")
                {
                    irc.sendChatMessage("It works!");
                }
            }*/
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
            
            TextBox tb_username = (TextBox)sender;
            tb_username.Text = string.Empty;
            tb_username.GotFocus -= tb_username_GotFocus;
            
        }


        // textbox password GotFocus
        private void tb_password_GotFocus(object sender, RoutedEventArgs e)
        {
            
            TextBox tb_password = (TextBox)sender;
            tb_password.Text = string.Empty;
            tb_password.GotFocus -= tb_password_GotFocus;
            
        }


        // textbox channel GotFocus
        private void tb_channel_GotFocus(object sender, RoutedEventArgs e)
        {
            
            TextBox tb_channel = (TextBox)sender;
            tb_channel.Text = string.Empty;
            tb_channel.GotFocus -= tb_channel_GotFocus;
            
        }

        #endregion


        #region FUNCTIONS



        #endregion
    }
}
