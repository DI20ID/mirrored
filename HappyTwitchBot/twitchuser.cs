using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyTwitchBot
{
    public class twitchuser
    {

        internal bool user;
        internal bool moderator;
        internal bool subscriber;

        public twitchuser(bool user, bool moderator, bool subscriber)
        {
            user = this.user;
            moderator = this.moderator;
            subscriber = this.subscriber;
        }
    }
}
