﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Models.User
{
    public struct User
    {
        public string email { get; set; }
        public UserType usertype { get; set; }
    }
}
