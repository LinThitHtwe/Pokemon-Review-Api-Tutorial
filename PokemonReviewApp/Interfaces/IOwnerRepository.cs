using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IOwnerRepository
    {
        ICollection<Owner> GetOwners();

        Owner GetOwnerById(int id);

        bool IsOwnerExists(int id);

        ICollection<Owner> GetOwnerOfAPokemon(int pokemonId);
        ICollection<Pokemon> GetPokemonByOwner(int ownerId);
        bool CreateOwner(Owner owner);
        bool Save();

    }
}
