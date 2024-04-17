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
    private readonly IConfiguration _configuration;
    private readonly IAnimalRepository _animalRepository;
    
    public AnimalsController(IConfiguration configuration, IAnimalRepository animalRepository)
    {
        _configuration = configuration;
        _animalRepository = animalRepository;
    }
    
    [HttpGet]
    public IActionResult GetAnimals(string? orderBy)
    {
        // Open connection
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        
        // Create command
        string commandText = orderBy switch
        {
            "description" => "SELECT * FROM Animal ORDER BY Description ASC",
            "category"    => "SELECT * FROM Animal ORDER BY Category ASC",
            "area"        => "SELECT * FROM Animal ORDER BY Area ASC",
            _             => "SELECT * FROM Animal ORDER BY Name ASC",
        };
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = commandText;
        
        // Execute command
        var reader = command.ExecuteReader();
        
        var animals = new List<Animal>();
        
        int idAnimalOrdinal = reader.GetOrdinal("IdAnimal");
        int nameOrdinal = reader.GetOrdinal("Name");
        int descriptionOrdinal = reader.GetOrdinal("Description");
        int categoryOrdinal = reader.GetOrdinal("Category");
        int areaOrdinal = reader.GetOrdinal("Area");
        
        while (reader.Read())
        {
            animals.Add(new Animal()
            {
                IdAnimal = reader.GetInt32(idAnimalOrdinal),
                Name = reader.GetString(nameOrdinal),
                Description = reader.GetString(descriptionOrdinal),
                Category = reader.GetString(categoryOrdinal),
                Area = reader.GetString(areaOrdinal),
        
            });
        }
        return Ok(animals);
        
        var newAnimals = _animalRepository.GetAnimal();
    }

    [HttpPost]
    public IActionResult AddAnimal(AddAnimalRequest animal)
    {
        // Open connection
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        
        // Create command
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "insert into Animal values (@Name, @Description, @Category, @Area);";
        command.Parameters.AddWithValue("@Name", animal.Name);
        command.Parameters.AddWithValue("@Description", animal.Description);
        command.Parameters.AddWithValue("@Category", animal.Category);
        command.Parameters.AddWithValue("@Area", animal.Area);
        
        // Execute command
        command.ExecuteNonQuery();
        
        return Created("api/animals", animal);
    }
    
    [HttpPut]
    public IActionResult UpdateAnimal(int idAnimal, UpdateAnimalRequest animal)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        var command = new SqlCommand("UPDATE Animal SET Name = @Name, Description = @Description, Category = @Category, Area = @Area WHERE IdAnimal = @IdAnimal;", connection);
        command.Parameters.AddWithValue("@IdAnimal", idAnimal);
        command.Parameters.AddWithValue("@Name", animal.Name);
        command.Parameters.AddWithValue("@Description", animal.Description);
        command.Parameters.AddWithValue("@Category", animal.Category);
        command.Parameters.AddWithValue("@Area", animal.Area);

        command.ExecuteNonQuery();

        return Ok(animal);
    }
    
    [HttpDelete]
    public IActionResult DeleteAnimal(int idAnimal)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        var command = new SqlCommand("DELETE FROM Animal WHERE IdAnimal = @IdAnimal;", connection);
        command.Parameters.AddWithValue("@IdAnimal", idAnimal);

        return Ok();
    }
}