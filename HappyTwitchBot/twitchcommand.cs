using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyTwitchBot
{
    public class twitchcommand
    {
        internal string main;
        internal string parameter1;

        internal string d_main;
        internal string parameter2;

        public twitchcommand(string main)
        {
            main = this.main;
            this.parameter1 = "";
            this.parameter2 = "";
        }

        public twitchcommand(string main, string parameter1)
        {
            this.main = main;
            this.parameter1 = parameter1;
            this.parameter2 = "";
        }

        public twitchcommand(string main, string parameter1, string parameter2)
        {
            this.main = main;
            this.parameter1 = parameter1;
            this.parameter2 = parameter2;
        }
    }
}
