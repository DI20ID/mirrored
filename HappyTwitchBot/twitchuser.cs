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

        internal bool moderator;
        internal bool subscriber;
        internal bool host;
        internal bool user;

        public twitchuser()
        {
            
        }
         

        public twitchuser(bool user, bool subscriber, bool moderator, bool host)
        {
            this.user = user;
            this.moderator = moderator;
            this.subscriber = subscriber;
            this.host = host;
        }
    }
}
