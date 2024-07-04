using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoScript.Server
{
    public class Script
    {
        public static List<string> AllCommands = new List<string>();

        public static List<string> ReLoad(string cmdText)
        {
           AllCommands=  cmdText.Split("\r\n").Select(x => x.Trim()).ToList();
           return AllCommands;
        }
    }
}