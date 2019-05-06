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
            InitialCommands();
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
            if (pattern != null)
            {
                if (pattern == ircPatterns.ping)            //ping reply to twitch (every 5 minutes)
                {
                    sendIrcMessage(ircPatterns.pong);
                    return;
                }

                if (pattern.Contains(ircPatterns.chatmessage + channel))        //chatmessages
                {
                    twitchuser userfocus = AddUser(pattern);
                    string messagefocus = capIRCString(pattern);

                    if (messagefocus[0].ToString() == ircPatterns.trigger)
                    {
                        twitchcommand commandfocus;
                        messagefocus = messagefocus.Substring(1);
                        string[] messagearguments = messagefocus.Split(' ');

                        if (commanddic.ContainsKey(messagearguments[0]))
                        {
                            commanddic.TryGetValue(messagearguments[0], out commandfocus);

                            if (messagearguments[0] == ircPatterns.led || messagearguments[0] == ircPatterns.onair)
                            {
                                bool newsettings = false;
                                if (messagearguments.Length > 1)
                                {
                                    if (messagearguments.Length > 3)
                                    {
                                        LED.R = messagearguments[1]; LED.G = messagearguments[2]; LED.B = messagearguments[3];
                                        newsettings = true;
                                        LED.led = "null";
                                    }
                                    if (messagearguments[0] == ircPatterns.led)
                                    {
                                        LED.H = "255";
                                        LED.ip = "10.0.0.136";
                                    }
                                    if (messagearguments[0] == ircPatterns.onair)
                                    {
                                        LED.H = "100";
                                        LED.ip = "10.0.0.137";
                                    }

                                    LED led = new LED(LED.ip, LED.port);

                                    switch (messagearguments[1])
                                    {
                                        case ircPatterns.led_red:
                                            LED.R = "255"; LED.G = "0"; LED.B = "0";
                                            LED.led = "null";
                                            newsettings = true;
                                            break;
                                        case ircPatterns.led_green:
                                            LED.R = "0"; LED.G = "255"; LED.B = "0";
                                            LED.led = "null";
                                            newsettings = true;
                                            break;
                                        case ircPatterns.led_blue:
                                            LED.R = "0"; LED.G = "0"; LED.B = "255";
                                            LED.led = "null";
                                            newsettings = true;
                                            break;
                                        case ircPatterns.led_white:
                                            LED.R = "255"; LED.G = "255"; LED.B = "255";
                                            LED.led = "null";
                                            newsettings = true;
                                            break;
                                        case ircPatterns.led_pink:
                                            LED.R = "255"; LED.G = "20"; LED.B = "147";
                                            LED.led = "null";
                                            newsettings = true;
                                            break;
                                        case ircPatterns.led_orange:
                                            LED.R = "255"; LED.G = "140"; LED.B = "0";
                                            LED.led = "null";
                                            newsettings = true;
                                            break;
                                        case ircPatterns.led_yellow:
                                            LED.R = "255"; LED.G = "255"; LED.B = "0";
                                            LED.led = "null";
                                            newsettings = true;
                                            break;
                                        case ircPatterns.led_cyan:
                                            LED.R = "0"; LED.G = "255"; LED.B = "255";
                                            LED.led = "null";
                                            newsettings = true;
                                            break;
                                        case ircPatterns.led_purple:
                                            LED.R = "102"; LED.G = "51"; LED.B = "153";
                                            LED.led = "null";
                                            newsettings = true;
                                            break;
                                    }

                                    if (newsettings)
                                    {
                                        led.sendSettings();
                                    }
                                }
                                return;
                            }
                            if (messagearguments[0] == ircPatterns.hello)
                            {
                                sendChatMessage("*waves*");
                                //sendIrcMessage("GET / channel");
                                return;
                            }
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
            string user = "";       //user - if no moderator/host or subscriber

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

            if (!(subscriber == "1" || moderator == "1" || host == "1"))            //set user if no moderator/host or subscriber
            {
                user = "1";
            }
            else
            {
                user = "0";
            }
               
            if (!userdic.ContainsKey(name))         //add user entry in userdic if it doesn't exist already
            {
                userdic.Add(name, new twitchuser(user == "1", subscriber == "1", moderator == "1", host == "1"));
            }

            twitchuser returnuser;
            userdic.TryGetValue(name, out returnuser);
            return returnuser;                  //return user entry from userdic

        }

        public void InitialCommands()           //Initial commanddic with default permissions
        {
            if(commanddic != null ) commanddic.Clear();
            commanddic = new Dictionary<string, twitchcommand>();
            commanddic.Add(ircPatterns.hello, new twitchcommand(false, false, false, true, ircPatterns.d_hello));
            commanddic.Add(ircPatterns.pause,new twitchcommand(false,false,false,true,ircPatterns.d_pause));
            commanddic.Add(ircPatterns.unpause, new twitchcommand(false, false, false, true, ircPatterns.d_unpause));
            commanddic.Add(ircPatterns.title, new twitchcommand(false, false, true, true, ircPatterns.d_title));
            commanddic.Add(ircPatterns.title_set, new twitchcommand(false, false, true, true, ircPatterns.d_title_set));
            commanddic.Add(ircPatterns.uptime, new twitchcommand(true, true, true, true, ircPatterns.d_uptime));
            commanddic.Add(ircPatterns.bothelp, new twitchcommand(true, true, true, true, ircPatterns.d_bothelp));
            commanddic.Add(ircPatterns.game, new twitchcommand(false, false, true, true, ircPatterns.d_game));
            commanddic.Add(ircPatterns.game_set, new twitchcommand(false, false, true, true, ircPatterns.d_game_set));
            commanddic.Add(ircPatterns.game_steam, new twitchcommand(false, false, true, true, ircPatterns.d_game_steam));
            commanddic.Add(ircPatterns.steamid, new twitchcommand(false, false, false, true, ircPatterns.d_steamid));
            commanddic.Add(ircPatterns.viewerstats, new twitchcommand(true, true, true, true, ircPatterns.d_viewerstats));
            commanddic.Add(ircPatterns.ishere, new twitchcommand(true, true, true, true, ircPatterns.d_ishere));
            commanddic.Add(ircPatterns.hug, new twitchcommand(true, true, true, true, ircPatterns.d_hug));
            commanddic.Add(ircPatterns.led, new twitchcommand(true, true, true, true, ircPatterns.led));
            commanddic.Add(ircPatterns.onair, new twitchcommand(true, true, true, true, ircPatterns.led));
        }

        #endregion
    }
}
