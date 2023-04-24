﻿using Tradify.Identity.Domain.Enums;

namespace Tradify.Identity.Domain.Entities
{
    //Tradify user: Admin, Buyer, Seller, whatever
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        
        public string Email { get; set; }

        public UserRole Role { get; set; } = UserRole.User;

        public bool IsEmailConfirmed { get; set; } = false;
        
        public long UserDataId { get; set; }
        public UserData UserData { get; set; }
    }
}
