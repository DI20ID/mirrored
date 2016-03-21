﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace HappyTwitchBot
{
    public static class ircPatterns
    {   //Twitch IRC Guide https://github.com/justintv/Twitch-API/blob/master/IRC.md
        //other constants
        internal static string passwordlink = "https://twitchapps.com/tmi/";

        #region TWITCH PATTERNS
        //input
        internal static string loginerror = ":tmi.twitch.tv NOTICE * :Error logging in";                                                                      //DONE
        internal static string userlist = ".tmi.twitch.tv 353 ";                                                                                              //DONE
        internal static string userlistend = ".tmi.twitch.tv 366 ";                                                                                           //DONE    
        internal static string ping = "PING :tmi.twitch.tv";                                                                                                   //DONE
        internal static string unknowncmd = ":tmi.twitch.tv 421";                                                                                             //DO IT
        internal static string mod = ":jtv MODE";                                                                                                           //DO IT
        internal static string chatmessage = ".tmi.twitch.tv PRIVMSG #";                                                                                       //DONE

        //output
        internal static string req_membership = "CAP REQ :twitch.tv/membership";                                                                              //DONE
        internal static string req_tags = "CAP REQ :twitch.tv/tags";                                                                                             //DONE
        internal static string pong = "PONG :tmi.twitch.tv";                                                                                                  //DONE
        #endregion

        #region CUSTOM PATTERNS
        //custom patterns
        internal static string trigger = "!";

        internal static string pause = "pause";                                                                                                      //DO IT
        internal static string d_pause = "Bot stops watching the chat until you type: " + unpause;
        internal static string unpause = "unpause";
        internal static string d_pause = "Bot starts watching the chat until you type: " + pause;

        internal static string title = "title";                                                                                                      //DO IT
        internal static string d_title = "Displays the title of the stream.";
        internal static string title_set = "title set";                                                                                                              //DO IT
        internal static string d_title_set = "Set the title with: '!title set <title>'";

        internal static string uptime = "uptime";                                                                                                    //DO IT
        internal static string d_uptime = "Displays the amount of time the stream has been online.";

        internal static string bothelp = "bothelp";                                                                                                  //DO IT
        internal static string d_bothelp = "Shows a link to the bot commands website or a download link.";

        internal static string game = "game";                                                                                                        //DO IT
        internal static string d_game = "Displays the current twitch channel game.";
        internal static string game_set = "game set";
        internal static string d_game_set = "Sets the game title:";                                                                                 //DO IT
        internal static string game_steam = "game steam";                                                                                                           //DO IT
        internal static string d_game_steam = "Sets the game title to the current steam game being played (requires '!steamid' first).";

        internal static string steamid = "steamid";                                                                                                  //DO IT
        internal static string d_steamid = "Sets the steamid. Must be in SteamID64 format. Profile must be public.";

        internal static string viewerstats = "viewerstats";                                                                                          //DO IT
        internal static string d_viewerstats = "Displays the current number of stream viewers and the highest viewer record for the channel.";

        internal static string ishere = "ishere";                                                                                                    //DO IT
        internal static string d_ishere = "Tells if the user is listed in the viewerlist.";

        internal static string hug = "hug";                                                                                                          //DO IT
        internal static string d_hug = "Hugs object with: '!hug <object>'";
        internal static string hug_user = "user";                                                                                                              //DO IT
        internal static string d_hug_user = "Hugs object with: '!hug <object>'";
        #endregion



    }
}
