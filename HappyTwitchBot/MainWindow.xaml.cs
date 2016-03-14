using System;
using System.Collections.Generic;
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

namespace HappyTwitchBot
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        
        public MainWindow()
        {
            InitializeComponent();
        }

        public ircClient irc = new ircClient();

        private void tb_username_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void tb_username_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb_username = (TextBox)sender;
            tb_username.Text = string.Empty;
            tb_username.GotFocus -= tb_username_GotFocus;
        }

        private void tb_password_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb_password = (TextBox)sender;
            tb_password.Text = string.Empty;
            tb_password.GotFocus -= tb_password_GotFocus;
        }

        private void b_connect_Click(object sender, RoutedEventArgs e)
        {
            irc = new ircClient("irc.twitch.tv", 6667, tb_username.Text, tb_password.Text);
            irc.joinRoom("happyliooon");
            /*while (true)
            {
                string message = irc.readMessage();
                if (message == "!hello")
                {
                    irc.sendChatMessage("It works!");
                }
            }*/
        }

        private void tb_channel_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb_channel = (TextBox)sender;
            tb_channel.Text = string.Empty;
            tb_channel.GotFocus -= tb_channel_GotFocus;
        }

        private void testbutton_Click(object sender, RoutedEventArgs e)
        {
            irc.sendChatMessage("It works!");
        }
    }
}
