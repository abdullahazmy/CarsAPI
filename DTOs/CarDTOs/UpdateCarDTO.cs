using CarsAPI.Enums;

namespace CarsAPI.DTOs.CarDTOs
{
    public class UpdateCarDTO
    {
        public string? Make { get; set; }
        public string? Model { get; set; }
        public DateOnly? Year { get; set; }
        public string? Color { get; set; }
        public decimal? Price { get; set; }

        public string? Description { get; set; }
        public Category? Category { get; set; }

        public List<ImageReadDTO>? ImageCars { get; set; }
    }

}
