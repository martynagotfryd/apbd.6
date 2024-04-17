using WebApplication2.Models;
using WebApplication2.Models.DTOs;

namespace WebApplication2.Repositories;

public interface IAnimalRepository
{
    IEnumerable<Animal> GetAnimal();
    void AddAnimal(AddAnimalRequest animal);
}