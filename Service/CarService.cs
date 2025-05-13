using AutoMapper;
using CarsAPI.DTOs.CarDTOs;
using CarsAPI.Enums;
using CarsAPI.Helpers;
using CarsAPI.Models;
using CarsAPI.Repository.Interfaces;

namespace CarsAPI.Service
{
    public class CarService : ICarService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CarService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<ReadCarDTO>> GetAllCars()
        {
            var cars = await _unitOfWork.CarRepository.GetAllAsync("Images");
            var result = _mapper.Map<List<ReadCarDTO>>(cars);

            // Add full URL for the single image
            var request = _httpContextAccessor.HttpContext.Request;
            foreach (var car in result)
            {
                if (!string.IsNullOrEmpty(car.ImageUrl))
                {
                    car.ImageUrl = ConstructFileUrlHelper.ConstructFileUrl(
                        request,
                        "images",
                        car.ImageUrl);
                }
            }

            return result;
        }

        public async Task<ReadCarById> GetCarById(string id)
        {
            var car = await _unitOfWork.CarRepository.GetByIdAsync(id, "Images");
            if (car == null)
                throw new Exception("Car not found");

            var dto = _mapper.Map<ReadCarById>(car);
            var request = _httpContextAccessor.HttpContext.Request;

            // Generate full image URLs for all images
            foreach (var img in dto.ImageCars)
            {
                img.ImageUrl = ConstructFileUrlHelper.ConstructFileUrl(
                    request,
                    "images",
                    img.ImageUrl);
            }

            return dto;
        }

        public async Task<ReadCarDTO> AddCar(CreateCarDTO carDTO)
        {
            var car = _mapper.Map<Car>(carDTO);

            if (carDTO.ImageCars != null && carDTO.ImageCars.Any())
            {
                foreach (var file in carDTO.ImageCars)
                {
                    var relativeUrl = await FileUploadHelper.UploadFileAsync(file, "images");
                    if (!string.IsNullOrEmpty(relativeUrl))
                    {
                        car.Images.Add(new ImageCar
                        {
                            Id = Guid.NewGuid().ToString(),
                            ImageUrl = relativeUrl, // Store full relative path
                            CarId = car.Id
                        });
                    }
                }
            }

            var createdCar = await _unitOfWork.CarRepository.AddAsync(car);
            return _mapper.Map<ReadCarDTO>(createdCar);
        }

        public async Task<ReadCarDTO> AddSpecial(string carId)
        {
            var car = await _unitOfWork.CarRepository.GetByIdAsync(carId);
            if (car == null)
                throw new Exception("Car not found");

            car.IsSpecial = true;
            _unitOfWork.CarRepository.UpdateAsync(car); // if your repo requires explicit update

            var readCarDto = _mapper.Map<ReadCarDTO>(car); // assuming you're using AutoMapper
            return readCarDto;
        }

        public async Task<IEnumerable<ReadCarDTO>> GetSpecialCarsAsync()
        {
            var specialCars = await _unitOfWork.CarRepository
                .GetByCondition(c => c.IsSpecial);
            if (!specialCars.Any())
                throw new Exception("No special cars found");
            var result = _mapper.Map<IEnumerable<ReadCarDTO>>(specialCars);
            return result;
        }





        public async Task<ReadCarById> UpdateCar(string id, UpdateCarDTO carDTO)
        {
            var car = await _unitOfWork.CarRepository.GetByIdAsync(id);
            if (car == null)
                throw new Exception("Car not found");

            _mapper.Map(carDTO, car);

            // Optionally update images if you later support that in UpdateCarDTO

            var updatedCar = await _unitOfWork.CarRepository.UpdateAsync(car);
            return _mapper.Map<ReadCarById>(updatedCar);
        }

        public async Task<bool> DeleteCar(string id)
        {
            var car = await _unitOfWork.CarRepository.GetByIdAsync(id);
            if (car == null)
                throw new Exception("Car not found");

            // Delete associated images from server
            foreach (var img in car.Images)
            {
                FileUploadHelper.DeleteFile($"/uploads/images/{img.ImageUrl}");
            }

            return await _unitOfWork.CarRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ReadCarDTO>> GetCarsByCategory(Category category)
        {
            var cars = await _unitOfWork.CarRepository
                .GetByCondition(c => c.Category == category);

            if (!cars.Any())
                throw new Exception("No cars found for this category");

            var result = _mapper.Map<IEnumerable<ReadCarDTO>>(cars);
            return result;
        }


    }
}
