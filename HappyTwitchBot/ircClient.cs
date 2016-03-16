using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows;


namespace HappyTwitchBot
{
    public class ircClient                 //class to create connection to the twitch irc server
    {
        private string username;        //irc connect username
        private string channel;         //irc channel to connect to (gets assigned in "public void joinRoom(string channel)"
        private string ircString;       //string for continously reading irc messages
        

        public TcpClient tcpClient;         //tcpClient for TCP connection
        public bool ReadStreamEnabled;       //bool to enable continous Stream Reading
        public bool Initialization;            // bool for initial stream read
        private StreamReader inputStream;      //irc input Stream (Read)
        private StreamWriter outputStream;      //irc output Stream (Write)

        public ircClient()
        {
        }


        public ircClient(string ip, int port, string username, string password)     //initial function for new ircClient
        {
            ReadStreamEnabled = false;      //set continous Stream Reading to false on initialization
            Initialization = true;
            this.username = username;                                       //save username for later use
            tcpClient = new TcpClient(ip, port);                            // create TCP Client
            inputStream = new StreamReader(tcpClient.GetStream());          //create input irc stream
            outputStream = new StreamWriter(tcpClient.GetStream());         //create output irc stream

            outputStream.WriteLine("PASS " + password);                     //send Login data with the correct irc syntax to irc output stream
            outputStream.WriteLine("NICK " + username);
            outputStream.WriteLine("USER " + username + " 8 * :" + username);
        }

        public void joinRoom(string channel)        //function to join a channel on twitch
        {
            this.channel = channel;                 // save channelname for later use
            outputStream.WriteLine("JOIN #" + channel);     // send correct syntax to irc stream to join the channel
            outputStream.Flush();                      // flush the stream
        }

        public void sendIrcMessage(string message)      //send irc message with tcpclient (not chat message - use sendChatMessage(string message) for that
        {
            outputStream.WriteLine(message);            //send message to irc stream
            outputStream.Flush();                       //flush the stream
        }

        public void sendChatMessage(string message)     //send chat message to channel
        {
            sendIrcMessage(":" + username + "!" + username + "@" + username + ".tmi.twitch.tv PRIVMSG #" + channel + " :" + message);       //correct syntax to send a chat message with the irc stream
        }

        public string readMessage()                     //read irc input stream - 1 line each time
        {
            string message = inputStream.ReadLine();    //read line from input stream
            return message;                             //return the line
        }



        public void ContinousRead()                     // continously read irc input stream as long as ReadStream Enabled == true
        {
            ircString = "";

            while(Initialization == true)
            {
                Thread.Sleep(5);
                ircString = readMessage();
                /*


                ADD NAMES LIST INITIALIZATION


                */
                if(ircString.Contains(ircPatterns.loginerror))
                {
                    MessageBox.Show("Login Failed. \nWrong password and/or username!\n\nDetails: " + ircString, "Error");
                    Initialization = false;
                    ReadStreamEnabled = false;
                }
                if (ircString.Contains(ircPatterns.nameslistend))
                {
                    ReadStreamEnabled = true;
                    Initialization = false;
                }
                
            }


            while (ReadStreamEnabled == true)
            {
                Thread.Sleep(5);
                ircString = readMessage();

                /*


                ADD CONTINOUS CHAT PATTERN CHECKS


                */
            }

            Initialization = false;
        }
    }
}
