using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository;

public interface IOwnerRepository
{
    bool CreateOwner(Owner owner);
    bool DeleteOwner(int ownerId);
    Owner GetOwnerById(int id);
    ICollection<Owner> GetOwnerOfAPokemon(int pokemonId);
    ICollection<Owner> GetOwners();
    ICollection<Pokemon> GetPokemonByOwner(int ownerId);
    bool IsOwnerExists(int id);
    bool Save();
    bool UpdateOwner(Owner owner);
}