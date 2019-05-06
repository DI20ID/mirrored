using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyTwitchBot
{
    public class twitchcommand
    {
        internal bool user;
        internal bool moderator;
        internal bool subscriber;
        internal bool host;
        internal string description;


        public twitchcommand()
        {
            
        }


        public twitchcommand(bool user, bool subscriber, bool moderator, bool host)
        {
            this.user = user;
            this.moderator = moderator;
            this.subscriber = subscriber;
            this.host = host;
            this.description = "";
        }
        public twitchcommand(bool user, bool subscriber, bool moderator, bool host, string description)
        {
            this.user = user;
            this.moderator = moderator;
            this.subscriber = subscriber;
            this.host = host;
            this.description = description;
        }
    }
}
