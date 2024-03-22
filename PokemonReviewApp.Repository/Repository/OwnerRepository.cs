using PokemonReviewApp.Data;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository;

public class OwnerRepository : IOwnerRepository
{
    private readonly DataContext _context;

    public OwnerRepository(DataContext context)
    {
        _context = context;
    }

    public bool CreateOwner(Owner owner)
    {
        _context.Add(owner);
        return Save();
    }

    public bool DeleteOwner(int ownerId)
    {
        Owner owner = GetOwnerById(ownerId);
        _context.Remove(owner);
        return Save();
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
        return _context.Owners.OrderBy(owner => owner.Id).ToList();
    }

    public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
    {
        return _context.PokemonOwners
            .Where(owner => owner.OwnerId == ownerId)
            .Select(pokemon => pokemon.Pokemon)
            .ToList();
    }

    public bool IsOwnerExists(int id)
    {
        return _context.Owners.Any(owner => owner.Id == id);
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0;
    }

    public bool UpdateOwner(Owner owner)
    {
        _context.Update(owner);
        return Save();
    }
}
