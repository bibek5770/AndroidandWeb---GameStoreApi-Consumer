﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class LoginView
    {
       
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
