﻿using System;
using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Auth
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
