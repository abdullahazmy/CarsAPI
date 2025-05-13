using CarsAPI.Models;
using CarsAPI.Repository.Interfaces;
using EdufyAPI.Models.Roles;

namespace CarsAPI.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context) => _context = context;


        private readonly GenericRepository<Car> _car;
        private readonly GenericRepository<AppUser> _user;
        private readonly GenericRepository<FavoriteCar> _favoriteCar;

        #region Repositories
        // Add your repositories here, assign Geters

        public GenericRepository<Car> CarRepository => _car ?? new GenericRepository<Car>(_context);
        public GenericRepository<AppUser> UserRepository => _user ?? new GenericRepository<AppUser>(_context);

        public GenericRepository<FavoriteCar> FavoriteCarRepository => _favoriteCar ?? new GenericRepository<FavoriteCar>(_context);

        #region example
        /*    public GenericRepository<Book> BookRepo
        {
            get
            {
                if (BookRepository == null)
                    BookRepository = new GenericRepository<Book>(_context);
                return BookRepository;
            }
        }
        */
        #endregion
        #endregion

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
