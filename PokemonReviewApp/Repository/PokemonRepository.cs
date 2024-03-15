using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class PokemonRepository : IPokemonRepository
    {

        private readonly DataContext _context;

        public PokemonRepository(DataContext context)
        {
            _context = context;
        }

        public Pokemon GetPokemonById(int id)
        {
            return _context.Pokemons.Where(pokemon => pokemon.Id == id).FirstOrDefault();
        }

        public Pokemon GetPokemonByName(string name)
        {
            return _context.Pokemons.Where(pokemon => pokemon.Name == name).FirstOrDefault();
        }

        public decimal GetPokemonRating(int id)
        {
            var reviews = _context.Reviews.Where(pokemon => pokemon.Id == id);
            if(reviews.Count() <= 0)
            {
                return 0;
            }
            return ((decimal)reviews.Sum(rating => rating.Rating) / reviews.Count());
        }

        public ICollection<Pokemon> GetPokemons()
        {

            return _context.Pokemons.OrderBy(pokemon => pokemon.Id).ToList();
        }

        public bool IsPokemonExists(int id)
        {
            return _context.Pokemons.Any(pokemon => pokemon.Id == id);
        }
    }
}
