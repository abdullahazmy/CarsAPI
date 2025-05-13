using AutoMapper;
using CarsAPI.DTOs;
using CarsAPI.Models;
using CarsAPI.Repository.Interfaces;

namespace CarsAPI.Service
{
    public class FavouriteService : IFavouriteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FavouriteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        public async Task<List<ReadFavDTO>> GetFavouritesByUserId(string userId)
        {
            var favs = await _unitOfWork.FavoriteCarRepository.GetByCondition(f => f.AppUserId == userId);
            if (favs == null)
                throw new Exception("No favorites found for this user");
            var result = _mapper.Map<List<ReadFavDTO>>(favs);

            //var carIds = favs.Select(f => f.CarId).ToList();
            //var cars = await _unitOfWork.CarRepository.GetByCondition(c => carIds.Contains(c.Id));
            //if (cars == null)
            //    throw new Exception("No cars found for this user");
            //var carResult = _mapper.Map<List<ReadCarDTO>>(cars);
            //return carResult;
            return result;
        }

        public async Task<bool> AddCarToFavourites(string carId, string userId)
        {
            var car = await _unitOfWork.CarRepository.GetByIdAsync(carId);
            if (car == null)
                throw new Exception("Car not found");

            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");


            var alreadyFavorited = await _unitOfWork.FavoriteCarRepository
                .GetSingleByCondition(fc => fc.CarId == carId && fc.AppUserId == userId);

            if (alreadyFavorited != null)
                return false;


            var fav = new FavoriteCar
            {
                CarId = carId,
                AppUserId = userId
            };

            await _unitOfWork.FavoriteCarRepository.AddAsync(fav);


            return true;
        }
    }
}
