using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository;

public interface ICountryRepository
{
    bool CreateCountry(Country country);
    bool DeleteCountry(int countryId);
    ICollection<Country> GetCountries();
    Country GetCountryByID(int id);
    Country GetCountryByOwner(int ownerId);
    ICollection<Owner> GetOwnersByCountryId(int countryId);
    bool IsCountryExist(int id);
    bool Save();
    bool UpdateCountry(Country country);
}