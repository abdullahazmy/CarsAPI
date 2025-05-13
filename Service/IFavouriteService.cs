using CarsAPI.DTOs;

namespace CarsAPI.Service
{
    public interface IFavouriteService
    {

        Task<bool> AddCarToFavourites(string carId, string userId);
        Task<List<ReadFavDTO>> GetFavouritesByUserId(string userId);

    }
}
