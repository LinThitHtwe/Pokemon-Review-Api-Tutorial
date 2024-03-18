using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository;

public interface IPokemonRepository
{
    bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);
    bool DeletePokemon(int id);
    Pokemon GetPokemonById(int id);
    Pokemon GetPokemonByName(string name);
    decimal GetPokemonRating(int id);
    ICollection<Pokemon> GetPokemons();
    bool IsPokemonExists(int id);
    bool Save();
    bool UpdatePokemon(Pokemon pokemon);
}