using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _context;

        public OwnerRepository(DataContext context)
        {
            _context = context;
        }

        public Owner GetOwnerById(int id)
        {
            return _context.Owners.Where(owner => owner.Id == id).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerOfAPokemon(int pokemonId)
        {
            return _context.PokemonOwners
                .Where(pokemon => pokemon.PokemonId == pokemonId)
                .Select(owner => owner.Owner)
                .ToList();
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.OrderBy(owner=>owner.Id).ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
            return _context.PokemonOwners
                .Where(owner=>owner.OwnerId == ownerId)
                .Select(pokemon=>pokemon.Pokemon)
                .ToList();
        }

        public bool IsOwnerExists(int id)
        {
            return _context.Owners.Any(owner => owner.Id == id);
        }
    }
}
