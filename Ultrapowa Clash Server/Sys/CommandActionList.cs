using System;

namespace UCS.Sys
{
    public class CommandActionList
    {
        public bool StartWithString = false;
        public string Command;
        public Action ExecuteCommand;
    }
}
