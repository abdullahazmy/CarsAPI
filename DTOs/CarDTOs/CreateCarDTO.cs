using CarsAPI.Enums;

namespace CarsAPI.DTOs.CarDTOs
{
    public class CreateCarDTO
    {
        public string Make { get; set; }
        public string Model { get; set; } = string.Empty;
        public DateOnly Year { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public Category Category { get; set; }

        public List<IFormFile> ImageCars { get; set; } = new List<IFormFile>();
    }
}
