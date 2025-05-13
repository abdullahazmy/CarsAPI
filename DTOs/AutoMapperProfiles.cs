using AutoMapper;
using CarsAPI.DTOs.CarDTOs;
using CarsAPI.Models;

namespace CarsAPI.DTOs
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Car -> ReadCarDTO
            CreateMap<Car, ReadCarDTO>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Images.FirstOrDefault().ImageUrl))
                .ReverseMap();

            // Car -> ReadCarById
            CreateMap<Car, ReadCarById>()
                .ForMember(dest => dest.ImageCars, opt => opt.MapFrom(src => src.Images));

            // CreateCarDTO -> Car
            CreateMap<CreateCarDTO, Car>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.Images, opt => opt.Ignore()); // We'll handle this manually

            // UpdateCarDTO -> Car
            CreateMap<UpdateCarDTO, Car>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // ImageCar -> ImageReadDTO
            CreateMap<ImageCar, ImageReadDTO>();

            // For GetAll - show only one image
            CreateMap<Car, ReadCarDTO>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src =>
                    src.Images.FirstOrDefault().ImageUrl))
                .ReverseMap();

            // For GetById - show all images
            CreateMap<Car, ReadCarById>()
                .ForMember(dest => dest.ImageCars, opt => opt.MapFrom(src => src.Images));

            CreateMap<FavoriteCar, ReadFavDTO>().ReverseMap();
        }
    }
}
