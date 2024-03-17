using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private DataContext _context;
        public CountryRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Country> GetCountries()
        {
            return _context.Countries
                .OrderBy(country => country.Id)
                .ToList();
        }

        public Country GetCountryByID(int id)
        {
            return _context.Countries
                .Where(country => country.Id == id)
                .FirstOrDefault();
        }

        public bool IsCountryExist(int id)
        {
            return _context.Countries.Any(country => country.Id == id);
        }

        public Country GetCountryByOwner(int ownerId)
        {
            return _context.Owners
                .Where(owner => owner.Id == ownerId)
                .Select(country => country.Country)
                .FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersByCountryId(int countryId)
        {
            return _context.Owners.Where(country => country.Country.Id == countryId).ToList();
        }
    }
}
