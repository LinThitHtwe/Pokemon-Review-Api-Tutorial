using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();
        Country GetCountryByID(int id);
        bool IsCountryExist(int id);
        Country GetCountryByOwner(int ownerId);
        ICollection<Owner> GetOwnersByCountryId(int countryId);

    }
}
