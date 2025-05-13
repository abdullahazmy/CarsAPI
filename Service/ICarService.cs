using CarsAPI.DTOs.CarDTOs;
using CarsAPI.Enums;

namespace CarsAPI.Service
{
    public interface ICarService
    {
        Task<List<ReadCarDTO>> GetAllCars();
        Task<ReadCarById> GetCarById(string id);
        Task<IEnumerable<ReadCarDTO>> GetCarsByCategory(Category category);
        Task<ReadCarDTO> AddCar(CreateCarDTO carDTO);
        Task<ReadCarDTO> AddSpecial(string carId);

        Task<IEnumerable<ReadCarDTO>> GetSpecialCarsAsync();
        Task<ReadCarById> UpdateCar(string id, UpdateCarDTO carDTO);
        Task<bool> DeleteCar(string id);

    }
}
