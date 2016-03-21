using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyTwitchBot
{
    public class twitchcommand
    {
        internal string user;
        internal string moderator;
        internal string subscriber;
        internal string host;
        internal string description;


        public twitchcommand()
        {
            
        }


        public twitchcommand(string user, string moderator, string subscriber, string host)
        {
            this.user = user;
            this.moderator = moderator;
            this.subscriber = subscriber;
            this.host = host;
            this.description = "";
        }
        public twitchcommand(string user, string moderator, string subscriber, string host, string description)
        {
            this.user = user;
            this.moderator = moderator;
            this.subscriber = subscriber;
            this.host = host;
            this.description = description;
        }
    }
}
