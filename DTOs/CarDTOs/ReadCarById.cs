using CarsAPI.Enums;

namespace CarsAPI.DTOs.CarDTOs
{
    public class ReadCarById
    {
        public string Id { get; set; }
        public string Make { get; set; }

        public string Model { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public DateOnly Year { get; set; }
        public decimal Price { get; set; }

        public string Description { get; set; }
        public Category Category { get; set; }

        public List<ImageReadDTO> ImageCars { get; set; } = new List<ImageReadDTO>();
    }
}
