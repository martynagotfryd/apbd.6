using Microsoft.Data.SqlClient;
using WebApplication2.Models;
using WebApplication2.Models.DTOs;

namespace WebApplication2.Repositories;

public class AnimalRepository : IAnimalRepository
{
    public IEnumerable<Animal> GetAnimal()
    {
        throw new NotImplementedException();
    }

    public void AddAnimal(AddAnimalRequest animal)
    {
        throw new NotImplementedException();
    }
}