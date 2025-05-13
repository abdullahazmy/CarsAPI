using CarsAPI.Models;
using EdufyAPI.Models.Roles;

namespace CarsAPI.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        GenericRepository<Car> CarRepository { get; }

        GenericRepository<AppUser> UserRepository { get; }

        GenericRepository<FavoriteCar> FavoriteCarRepository { get; }


        Task<int> SaveChangesAsync();
    }
}
