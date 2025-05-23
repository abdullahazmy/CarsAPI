﻿using EdufyAPI.Models.Roles;

namespace CarsAPI.Models.Roles
{
    public class Admin : AppUser
    {
        // Add your custom properties here
        public DateTime? LastSupervisedLogin { get; set; } // Track admin logins
        public bool CanManageUsers { get; set; } = true; // Admin permissions
        public bool CanViewReports { get; set; } = true; // Example privilege
    }
}
