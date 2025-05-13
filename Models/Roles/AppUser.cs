using CarsAPI.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EdufyAPI.Models.Roles
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        public string FullName => $"{FirstName} {LastName}".Trim();

        public string? ProfilePictureUrl { get; set; } = string.Empty;

        public virtual List<FavoriteCar> FavoriteCars { get; set; } = new List<FavoriteCar>();


    }
}
