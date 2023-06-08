using System;
using System.Collections.Generic;
using System.Text;

namespace DoNotCopy.Core.Infrastructure
{
    public class EmailSettings
    {
        public string From { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
