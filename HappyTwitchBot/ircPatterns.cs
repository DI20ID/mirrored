using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace HappyTwitchBot
{
    public static class ircPatterns
    {
        //other constants
        public static string passwordlink = "https://twitchapps.com/tmi/";

        //twitch patterns
        public static string loginerror = ":tmi.twitch.tv NOTICE * :Error logging in";                                                                      //DONE
        public static string nameslist = ".tmi.twitch.tv 353 ";                                                                                             //DO IT
        public static string nameslistend = ".tmi.twitch.tv 366 ";                                                                                          //DO IT    

        //custom patterns
        public static string trigger = "!";

        public static string pause = trigger + "pause";                                                                                                      //DO IT
        public static string unpause = trigger + "unpause";                                                                                                 
        public static string d_pause = pause + " : Bot stops watching the chat until you type '" + unpause + "'.";                                                    

        public static string title = trigger + "title";                                                                                                      //DO IT
        public static string title_set = "set";                                                                                                              //DO IT
        public static string d_title = title + " : Displays the title of the stream.\n" +
                                       title + " " + title_set + " : Set the title: '!title set <title>'";

        public static string uptime = trigger + "uptime";                                                                                                    //DO IT
        public static string d_uptime = uptime + " : Displays the amount of time the stream has been online.";                                                  

        public static string bothelp = trigger + "bothelp";                                                                                                  //DO IT
        public static string d_bothelp = bothelp + " : Shows a link to the bot commands website or a download link.";                                  

        public static string game = trigger + "game";                                                                                                        //DO IT
        public static string game_set = "set";                                                                                                               //DO IT
        public static string game_steam = "steam";                                                                                                           //DO IT
        public static string d_game = game + " : Displays the current twitch channel game.\n" +                                                     
                                      game + " " + game_set + " <game> : Sets the game title: .\n" +
                                      game + " " + game_steam + " : Sets the game title to the current steam game being played (requires '!steamid' first).";

        public static string steamid = trigger + "steamid";                                                                                                  //DO IT
        public static string d_steamid = steamid + " <steamid> : Sets the steamid. Must be in SteamID64 format. Profile must be public.";

        public static string viewerstats = trigger + "viewerstats";                                                                                          //DO IT
        public static string d_viewerstats = viewerstats + " : Displays the current number of stream viewers and the highest viewer record for the channel.";        

        public static string ishere = trigger + "ishere";                                                                                                    //DO IT
        public static string d_ishere = ishere + " <user> : Tells if the user is listed in the viewerlist.";
            
        public static string hug = trigger + "hug";                                                                                                          //DO IT
        public static string hug_user = "user";                                                                                                              //DO IT
        public static string d_hug = hug + " <object> : Hugs object.\n" +
                                     hug + " " + hug_user + " : Gives hug to a user in whisper chat.";




    }
}
