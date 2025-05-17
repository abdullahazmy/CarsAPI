using CarsAPI.DTOs.CarDTOs;
using CarsAPI.Enums;
using CarsAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace CarsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;
        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCars()
        {
            var cars = await _carService.GetAllCars();
            return Ok(cars);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarById(string id)
        {
            var car = await _carService.GetCarById(id);
            if (car == null)
            {
                return NotFound();
            }
            return Ok(car);
        }


        // Get Car By Category
        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetCarsByCategory(Category category)
        {
            var car = await _carService.GetCarsByCategory(category);
            if (car == null)
            {
                return NotFound();
            }
            return Ok(car);
        }

        [HttpPost]
        public async Task<IActionResult> AddCar([FromForm] CreateCarDTO carDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdCar = await _carService.AddCar(carDTO);
            return CreatedAtAction(nameof(GetCarById), new { id = createdCar.Id }, createdCar);
        }


        // Add to Specials
        [HttpPost]
        [Route("specials/{id}")]
        public async Task<IActionResult> AddSpecial(string id)
        {
            var specialCar = await _carService.AddSpecial(id);
            if (specialCar == null)
            {
                return NotFound();
            }
            return Ok(specialCar);
        }

        // Get Special Cars
        [HttpGet("specials")]
        public async Task<IActionResult> GetSpecialCars()
        {
            try
            {
                var result = await _carService.GetSpecialCarsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar(string id, [FromBody] UpdateCarDTO carDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedCar = await _carService.UpdateCar(id, carDTO);
            if (updatedCar == null)
            {
                return NotFound();
            }
            return Ok(updatedCar);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(string id)
        {
            var result = await _carService.DeleteCar(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
