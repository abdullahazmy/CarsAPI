using CarsAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace CarsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouritesController : ControllerBase
    {
        private readonly IFavouriteService _favouritesService;
        public FavouritesController(IFavouriteService favouritesService)
        {
            _favouritesService = favouritesService;
        }

        [HttpPost("{carId}/{userId}")]
        public async Task<IActionResult> AddCarToFavourites(string carId, string userId)
        {
            var result = await _favouritesService.AddCarToFavourites(carId, userId);
            if (result)
            {
                return Ok("Car added to favourites");
            }
            return BadRequest("Failed to add car to favourites");
        }


        [HttpGet("{userId}")]
        public async Task<IActionResult> GetFavouritesByUserId(string userId)
        {
            var favourites = await _favouritesService.GetFavouritesByUserId(userId);
            if (favourites == null || !favourites.Any())
            {
                return NotFound("No favourites found for this user");
            }
            return Ok(favourites);
        }
    }
}
