using CarsAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace CarsAPI.Models
{
    public class Car
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Make { get; set; }
        public string Model { get; set; } = string.Empty;

        [DataType("date")]
        public DateOnly Year { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;

        public bool IsSpecial { get; set; } = false;

        public Category Category { get; set; }
        public virtual List<FavoriteCar> FavoritedBy { get; set; } = new List<FavoriteCar>();

        public virtual List<ImageCar> Images { get; set; } = new List<ImageCar>();
    }
}
