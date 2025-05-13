using CarsAPI.DTOs.CarDTOs;

namespace CarsAPI.DTOs
{
    public class ReadFavDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string AppUserId { get; set; }
        public string CarId { get; set; }
        public virtual ReadCarDTO Car { get; set; }
    }
}
