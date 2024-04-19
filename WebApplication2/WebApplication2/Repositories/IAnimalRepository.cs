using WebApplication2.Models;
using WebApplication2.Models.DTOs;

namespace WebApplication2.Repositories;

public interface IAnimalRepository
{
    IEnumerable<Animal> GetAnimal(string? orderBy);
    void AddAnimal(AddAnimalRequest animal);
    void UpdateAnimal(int idAnimal, UpdateAnimalRequest animal);
    void DeleteAnimal(int idAnimal);
}