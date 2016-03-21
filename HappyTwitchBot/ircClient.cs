using System;
using System.IO;
using System.Net.Sockets;
using System.Security;
using System.Threading;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Markup.Localizer;


namespace HappyTwitchBot
{
    public class ircClient                 //class to create connection to the twitch irc server
    {
        #region VARIABLES

        private string username;        //irc connect username
        private string channel;         //irc channel to connect to (gets assigned in "public void joinRoom(string channel)" - on initialization channel = username
        private string ircString;       //string for continously reading irc message

        private string api_ClientID;
        private string api_ClientSecret;

  

        public string logfilepath;

        public TcpClient tcpClient;         //tcpClient for TCP connection
        public bool ReadStreamEnabled;       //bool to enable continous Stream Reading
        public bool Initialization;            // bool for initial stream read

        private StreamReader inputStream;      //irc input Stream (Read)
        private StreamWriter outputStream;      //irc output Stream (Write)
        private Dictionary<string, twitchuser> userdic; // temporary userlist
        private Dictionary<string, twitchcommand> commanddic; //command list 

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
            userdic = new Dictionary<string, twitchuser>();
            commanddic = new Dictionary<string, twitchcommand>();
            api_ClientID = "5wz089y0qgk0nkb5r0mw5228ksfccnv";
            api_ClientSecret = "3z84zuu59wuotl8lozyowr0ggo70c5j";

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

                //Not included for now since it only works for smaller channels
                /*
                if (ircString.Contains(ircPatterns.mod) && !ircString.Contains(ircPatterns.chatmessage))           //mod add / delete
                {
                    ircString = capIRCString(ircString);

                    if (ircString.Substring(0, 2) == "+o")
                    {
                        ircString.Replace("+o ", "");
                        userdic.Add(ircString,new twitchuser(false,true,false));
                    }
                    else
                    {
                        if (ircString.Substring(0, 2) == "-o")
                        {
                            ircString.Replace("-o ", "");
                            userdic.Add(ircString, new twitchuser(false, true, false));
                        }
                    }

                    continue;
                }
                */

                //Not included for now since it only works for smaller channels
                /*
                if (ircString.Contains(ircPatterns.userlist))           //get userlist
                {
                    ircString = capIRCString(ircString);

                    string[] users;
                    users = ircString.Split(' ');
                    foreach (string user in users)
                    {
                        if (user == username)
                        {
                            userdic.Add(user, new twitchuser(true, true, true));
                        }
                        else
                        {
                            userdic.Add(user, new twitchuser(true, false, true));
                        }
                    }

                    continue;
                }
                */



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

                /*
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
                */

            }
        }


        public void breakpoint()
        {
            return;
        }
        public void PatternCheck(string pattern)
        {
            if (pattern == ircPatterns.ping)            //ping reply to twitch (every 5 minutes)
            {
                sendIrcMessage(ircPatterns.pong);
            }

            if (pattern.Contains(ircPatterns.chatmessage + channel))        //chatmessages
            {
                twitchuser userfocus = AddUser(pattern);
                string messagefocus = capIRCString(pattern);

                if (messagefocus[0].ToString() == ircPatterns.trigger)
                {
                    twitchcommand commandfocus;
                    messagefocus = messagefocus.Substring(1);

                    if (commanddic.ContainsKey(messagefocus))
                    {
                        commanddic.TryGetValue(messagefocus, out commandfocus);


                        if (messagefocus == ircPatterns.title)
                        {
                            //sendIrcMessage("GET / channel");
                            return;
                        }
                    }
                }
            }


            /*
            if (pattern.Contains("!hello"))
            {
                sendChatMessage("*waves*");
            }



            ADD CHAT PATTERN CHECKS


            */
        }

        private twitchuser AddUser(string pattern)
        {
            string[] tags = (pattern.Substring(0, pattern.IndexOf(ircPatterns.chatmessage))).Split(';');

            string color = "";      //textcolor
            string name = "";       //username
            string emotes = "";     //emotes user has
            string moderator = "";        //moderator?
            string roomID = "";     //roomID user is in
            string subscriber = ""; //subscriber?
            string turbo = "";      //turbo user?
            string userID = "";     //userID
            string host = "";       //host of the channel?
            string user = "";

            foreach (string tag in tags)                //get different properties from string and make a userdic entry if it doesn't exist already
            {
                string prop = tag.Split('=')[0];
                switch (prop)
                {
                    case "color":
                        {
                            if (tag.Split('=').Length > 1)
                            {
                                color = tag.Split('=')[1];
                            }
                            continue;
                        }
                    case "display-name":
                        {
                            if (tag.Split('=').Length > 1)
                            {
                                name = tag.Split('=')[1];
                            }
                            continue;
                        }

                    case "emotes":
                        {
                            if (tag.Split('=').Length > 1)
                            {
                                emotes = tag.Split('=')[1];
                            }
                            continue;
                        }

                    case "mod":
                        {
                            if (tag.Split('=').Length > 1)
                            {
                                moderator = tag.Split('=')[1];
                            }
                            continue;
                        }

                    case "room-id":
                        {
                            if (tag.Split('=').Length > 1)
                            {
                                roomID = tag.Split('=')[1];
                            }
                            continue;
                        }

                    case "subscriber":
                        {
                            if (tag.Split('=').Length > 1)
                            {
                                subscriber = tag.Split('=')[1];
                            }
                            continue;
                        }

                    case "turbo":
                        {
                            if (tag.Split('=').Length > 1)
                            {
                                turbo = tag.Split('=')[1];
                            }
                            continue;
                        }
                    case "user-id":
                        {
                            if (tag.Split('=').Length > 1)
                            {
                                userID = tag.Split('=')[1];
                            }
                            continue;
                        }
                }
            }

            if (string.Equals(name, channel, StringComparison.CurrentCultureIgnoreCase))        //set host permissions if message from host
            {
                host = "1";
            }

            if (!(subscriber == "1" || moderator == "1" || host == "1"))
            {
                user = "1";
            }
            else
            {
                user = "0";
            }
               
            if (!userdic.ContainsKey(name))         //add user entry in userdic if it doesn't exist already
            {
                userdic.Add(name, new twitchuser(user, subscriber, moderator, host));
            }

            twitchuser returnuser;
            userdic.TryGetValue(name, out returnuser);
            return returnuser;                  //return user entry from userdic

        }

        public void InitialCommands()
        {
            commanddic.Clear();

            commanddic.Add(ircPatterns.pause,new twitchcommand("0","0","0","1",ircPatterns.d_pause));
            commanddic.Add(ircPatterns.unpause, new twitchcommand("0", "0", "0", "1", ircPatterns.d_unpause));
            commanddic.Add(ircPatterns.title, new twitchcommand("0", "0", "1", "1", ircPatterns.d_title));
            commanddic.Add(ircPatterns.title_set, new twitchcommand("0", "0", "1", "1", ircPatterns.d_title_set));
            commanddic.Add(ircPatterns.uptime, new twitchcommand("1", "1", "1", "1", ircPatterns.d_uptime));
            commanddic.Add(ircPatterns.bothelp, new twitchcommand("1", "1", "1", "1", ircPatterns.d_bothelp));
            commanddic.Add(ircPatterns.game, new twitchcommand("0", "0", "1", "1", ircPatterns.d_game));
            commanddic.Add(ircPatterns.game_set, new twitchcommand("0", "0", "1", "1", ircPatterns.d_game_set));
            commanddic.Add(ircPatterns.game_steam, new twitchcommand("0", "0", "1", "1", ircPatterns.d_game_steam));
            commanddic.Add(ircPatterns.steamid, new twitchcommand("0", "0", "0", "1", ircPatterns.d_steamid));
            commanddic.Add(ircPatterns.viewerstats, new twitchcommand("1", "1", "1", "1", ircPatterns.d_viewerstats));
            commanddic.Add(ircPatterns.ishere, new twitchcommand("1", "1", "1", "1", ircPatterns.d_ishere));
            commanddic.Add(ircPatterns.hug, new twitchcommand("1", "1", "1", "1", ircPatterns.d_hug));
        }

        #endregion
    }
}
