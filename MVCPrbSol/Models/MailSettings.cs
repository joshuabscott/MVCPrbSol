﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MimeKit;
using MVCPrbSol.Models;

namespace MVCPrbSol.Models   //Namespace is the outermost , Inside is a class, than a method, than the logic
{
    public class MailSettings
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}//Data for Mail Objects when instance is called
//Sat
