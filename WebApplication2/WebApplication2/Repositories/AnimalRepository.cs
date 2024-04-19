using Microsoft.Data.SqlClient;
using WebApplication2.Models;
using WebApplication2.Models.DTOs;

namespace WebApplication2.Repositories;

public class AnimalRepository : IAnimalRepository
{
    private readonly IConfiguration _configuration;

    public AnimalRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IEnumerable<Animal> GetAnimal(string? orderBy)
    {
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        // Create command
        string commandText = orderBy switch
        {
            "description" => "SELECT * FROM Animal ORDER BY Description ASC",
            "category" => "SELECT * FROM Animal ORDER BY Category ASC",
            "area" => "SELECT * FROM Animal ORDER BY Area ASC",
            _ => "SELECT * FROM Animal ORDER BY Name ASC",
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

        return animals;
    }

    public void AddAnimal(AddAnimalRequest animal)
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
    }

    public void UpdateAnimal(int idAnimal, UpdateAnimalRequest animal)
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
    }

    public void DeleteAnimal(int idAnimal)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        var command = new SqlCommand("DELETE FROM Animal WHERE IdAnimal = @IdAnimal;", connection);
        command.Parameters.AddWithValue("@IdAnimal", idAnimal);

        command.ExecuteNonQuery();
    }
}