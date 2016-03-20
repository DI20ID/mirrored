using System;
using System.IO;
using System.Net.Sockets;
using System.Security;
using System.Threading;
using System.Windows;
using System.Collections.Generic;


namespace HappyTwitchBot
{
    public class ircClient                 //class to create connection to the twitch irc server
    {
        #region VARIABLES

        private string username;        //irc connect username
        private string channel;         //irc channel to connect to (gets assigned in "public void joinRoom(string channel)" - on initialization channel = username
        private string ircString;       //string for continously reading irc message

  

        public string logfilepath;

        public TcpClient tcpClient;         //tcpClient for TCP connection
        public bool ReadStreamEnabled;       //bool to enable continous Stream Reading
        public bool Initialization;            // bool for initial stream read

        private StreamReader inputStream;      //irc input Stream (Read)
        private StreamWriter outputStream;      //irc output Stream (Write)
        private Dictionary<string,string> userdic;     //userlist with <username,rank>
        private Dictionary<string, twitchuser> userdic2; // temporary userlist

        #endregion

        #region CONSTRUCTORS
        public ircClient()
        {

        }

        
        public ircClient(string ip, int port, string username, string password)     //initial function for new ircClient
        {
            ReadStreamEnabled = false;      //set continous Stream Reading to false on initialization
            Initialization = true;
            this.username = username;                                       //save username for later use
            channel = username;                                             //initialize channel so it's not NULL
            tcpClient = new TcpClient(ip, port);                            // create TCP Client
            inputStream = new StreamReader(tcpClient.GetStream());          //create input irc stream
            outputStream = new StreamWriter(tcpClient.GetStream());         //create output irc stream
            ircString = "";                                                 //initialize ircString so it's not NULL
            logfilepath = Path.GetTempPath() + "_HappyTwitchBot.log";                               //default log file path
            Login(username,password);
            userdic = new Dictionary<string,string>();
            userdic2 = new Dictionary<string, twitchuser>();
        }
        #endregion

        #region FUNCTIONS

        public void RequestMembership()
        {
            sendIrcMessage(ircPatterns.req_membership);
        }

        public void RequestTags()
        {
            sendIrcMessage(ircPatterns.req_tags);
        }

        public void Login(string username, string password)                         //function to login to an account on twitch using irc.password and irc.username
        {
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
        {                                               //WARNING: This function will wait on "inputStream.ReadLine()" until data is received
            string message = inputStream.ReadLine();    //read line from input stream
            return message;                             //return the line
        }

        public string capIRCString(string StrToCap)
        {
            int capmark = StrToCap.IndexOf("#" + channel);
            StrToCap = StrToCap.Substring(capmark + channel.Length + 3);
            return StrToCap;
        }

        public void WatchDog()                     // continously read irc input stream as long as ReadStream Enabled == true 
        {                                               // WARNING: if ReadStreamEnabled or Initialization is "true" - This function will wait on "inputStream.ReadLine()" until data is received
            while (Initialization)                      //initial stream read to get userlist
            {
                ircString = inputStream.ReadLine();

                if (!File.Exists(logfilepath))              //check if logfile exists - otherwise create
                {
                    try
                    {
                        var log = File.Create(logfilepath);
                        log.Close();
                    }
                    catch (Exception)
                    {

                    }
                }
                
                if (File.Exists(logfilepath))               //write logfile
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@logfilepath, true))
                        //write log file
                    {
                        file.WriteLine(ircString);
                        file.Close();
                    }
                    
                }

                if (ircString.Contains(ircPatterns.userlist))           //get userlist
                {
                    ircString = capIRCString(ircString);

                    string[] users;
                    users = ircString.Split(' ');
                    foreach (string user in users)
                    {
                        if (user == username)
                        {
                            userdic.Add(user, ircPatterns.rank_owner);
                            // or something like that
                            userdic2.Add(user, new twitchuser(true, true, true));
                        }
                        else
                        {
                            userdic.Add(user, ircPatterns.rank_peasant);
                            // or something like that
                            userdic2.Add(user, new twitchuser(true, false, true));
                        }
                    }

                    continue;
                }


                if (ircString.Contains(ircPatterns.loginerror))
                {
                    MessageBox.Show("Login Failed. \nWrong password and/or username!\n\nDetails: " + ircString, "Error");
                    Initialization = false;
                    ReadStreamEnabled = false;

                    continue;
                }
                if (ircString.Contains(ircPatterns.userlistend))
                {
                    ReadStreamEnabled = true;
                    Initialization = false;
                }
            }

            if (ReadStreamEnabled)      //Continous reading of irc stream
            {
                string pattern = inputStream.ReadLine();    //wait for data on the ircClient inputStream

                Thread WatchDogThread = new Thread(WatchDog);       //start a new thread to wait for the next line (better reaction time)
                WatchDogThread.Start();

                PatternCheck(pattern);

                if (!File.Exists(logfilepath))
                {
                    try
                    {
                        var log = File.Create(logfilepath);
                        log.Close();
                    }
                    catch (Exception)
                    {

                    }
                }
                if (File.Exists(logfilepath))
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@logfilepath, true))
                    //write log file
                    {
                        file.WriteLine(pattern);
                    }
                }


            }
        }


        public void breakpoint()
        {
            return;
        }
        public void PatternCheck(string pattern)
        {
            /*
            if (pattern.Contains("!hello"))
            {
                sendChatMessage("*waves*");
            }



            ADD CHAT PATTERN CHECKS


            */
        }

        #endregion
    }
}
