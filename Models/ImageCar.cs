using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsAPI.Models
{
    public class ImageCar
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        [ForeignKey("Car")]
        public string CarId { get; set; }
        public virtual Car Car { get; set; } = new Car();
    }
}
