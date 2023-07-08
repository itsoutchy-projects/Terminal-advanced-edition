using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminal_advanced_edition
{
    public class Command
    {
        public string cmd;
        public string desc;

        /// <summary>
        /// Makes a new command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="description"></param>
        public Command(string command, string description)
        {
            cmd = command;
            desc = description;
        }
    }
}
