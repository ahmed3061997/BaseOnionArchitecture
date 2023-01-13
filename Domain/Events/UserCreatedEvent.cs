﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;
using Domain.Entities.Users;

namespace Domain.Events
{
    public class UserCreatedEvent : BaseEvent
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string ConfirmEmailUrl { get; set; }

        public UserCreatedEvent(string userId, string email, string role, string confirmEmailUrl)
        {
            UserId = userId;
            Email = email;
            Role = role;
            ConfirmEmailUrl = confirmEmailUrl;
        }
    }
}
