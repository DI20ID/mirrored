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

        public twitchuser()
        {
            
        }
         

        public twitchuser(string moderator, string subscriber, string host)
        {
            this.moderator = moderator;
            this.subscriber = subscriber;
            this.host = host;
        }
    }
}
