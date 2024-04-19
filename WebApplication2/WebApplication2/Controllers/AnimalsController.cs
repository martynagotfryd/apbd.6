using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebApplication2.Models;
using WebApplication2.Models.DTOs;
using WebApplication2.Repositories;

namespace WebApplication2.Controllers;

[ApiController]
// [Route("api/animals")]
[Route("api/[controller]")]
public class AnimalsController : ControllerBase
{
    private readonly IAnimalRepository _animalRepository;
    
    public AnimalsController(IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
    }
    
    [HttpGet]
    public IActionResult GetAnimals(string? orderBy)
    {
        var repo = _animalRepository.GetAnimal(orderBy);

        if (repo != null)
        {
            return Ok(repo);
        }

        return NotFound();
    }

    [HttpPost]
    public IActionResult AddAnimal(AddAnimalRequest animal)
    {
        _animalRepository.AddAnimal(animal);
        
        return Created();
    }
    
    [HttpPut]
    public IActionResult UpdateAnimal(int idAnimal, UpdateAnimalRequest animal)
    {
        _animalRepository.UpdateAnimal(idAnimal, animal);

        return Ok();
    }
    
    [HttpDelete]
    public IActionResult DeleteAnimal(int idAnimal)
    {
        _animalRepository.DeleteAnimal(idAnimal);

        return Ok();
    }
}