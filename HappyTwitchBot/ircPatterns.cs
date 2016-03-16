using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyTwitchBot
{
    public static class ircPatterns
    {
        public static string loginerror = ":tmi.twitch.tv NOTICE * :Error logging in";
        public static string nameslist = ".tmi.twitch.tv 353 ";
        public static string nameslistend = ".tmi.twitch.tv 366 ";
    }
}
