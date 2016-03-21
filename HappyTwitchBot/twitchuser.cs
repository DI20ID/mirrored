using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HappyTwitchBot
{
    public class twitchuser
    {

        internal string moderator;
        internal string subscriber;
        internal string host;
        internal string user;

        public twitchuser()
        {
            
        }
         

        public twitchuser(string user, string subscriber, string moderator, string host)
        {
            this.user = user;
            this.moderator = moderator;
            this.subscriber = subscriber;
            this.host = host;
        }
    }
}
