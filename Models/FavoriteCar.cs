using EdufyAPI.Models.Roles;
using System.ComponentModel.DataAnnotations;

namespace CarsAPI.Models
{
    public class FavoriteCar
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }

        public string CarId { get; set; }
        public virtual Car Car { get; set; }
    }

}
