using Microsoft.EntityFrameworkCore;
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

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var owner = _context.Owners.Where(owner=>owner.Id==ownerId).FirstOrDefault();
            var category = _context.Categories.Where(category=>category.Id==categoryId).FirstOrDefault();
            var pokemonOwner = new PokemonOwner()
            {
                Owner = owner,
                Pokemon = pokemon,
            };

            _context.Add(pokemonOwner);

            var pokemonCategory = new PokemonCategory()
            {
                Category = category,
                Pokemon = pokemon
            };

            _context.Add(pokemonCategory);

            _context.Add(pokemon);
            return Save();
        }

        public bool DeletePokemon(int id)
        {
            Pokemon pokemon = GetPokemonById(id);
            _context.Remove(pokemon);
            return Save();
        }

        public Pokemon GetPokemonById(int id)
        {
            return _context.Pokemons
                .Where(pokemon => pokemon.Id == id)
                .FirstOrDefault();
        }

        public Pokemon GetPokemonByName(string name)
        {
            return _context.Pokemons
                .Where(pokemon => pokemon.Name == name)
                .FirstOrDefault();
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
            return _context.Pokemons
                    //.Include(pokemon => pokemon.Reviews)
                    //.Include(pokemon => pokemon.PokemonOwners)
                    //.Include(pokemon => pokemon.PokemonCategories)
                    .OrderBy(pokemon => pokemon.Id)
                    .ToList();
        }

        public bool IsPokemonExists(int id)
        {
            return _context.Pokemons.Any(pokemon => pokemon.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UpdatePokemon(Pokemon pokemon)
        {
            _context.Update(pokemon);
            return Save();
        }
    }
}
