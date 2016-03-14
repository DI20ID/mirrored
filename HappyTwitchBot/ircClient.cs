using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace HappyTwitchBot
{
    public class ircClient
    {
            private string username;
            private string channel;

            private TcpClient tcpClient;
            private StreamReader inputStream;
            private StreamWriter outputStream;

            public ircClient()
            { }

            public ircClient(string ip, int port, string username, string password)
            {
                this.username = username;
                tcpClient = new TcpClient(ip, port);
                inputStream = new StreamReader(tcpClient.GetStream());
                outputStream = new StreamWriter(tcpClient.GetStream());

                outputStream.WriteLine("PASS " + password);
                outputStream.WriteLine("NICK " + username);
                outputStream.WriteLine("USER " + username + " 8 * :" + username);

            }

            public void joinRoom(string channel)
            {
                this.channel = channel;
                outputStream.WriteLine("JOIN #" + channel);
                outputStream.Flush();
            }

            public void sendIrcMessage(string message)
            {
                outputStream.WriteLine(message);
                outputStream.Flush();
            }

            public void sendChatMessage(string message)
            {
                sendIrcMessage(":" + username + "!" + username + "@" + username + ".tmi.twitch.tv PRIVMSG #" + channel + " :" + message);
            }

            public string readMessage()
            {
                string message = inputStream.ReadLine();
                return message;
            }
        
}
}
