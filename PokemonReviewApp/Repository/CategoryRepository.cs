using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.Linq;

namespace PokemonReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private DataContext _context;

        public CategoryRepository(DataContext context) 
        {
            _context = context;
        }
        public ICollection<Category> GetCategories()
        {
            return _context.Categories
                .OrderBy(category=> category.Id)
                .ToList();
        }

        public Category GetCategoryById(int id)
        {
            return _context.Categories
                .Where(category => category.Id == id)
                .FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonsByCategory(int categoryId)
        {
            return _context.PokemonCategories
                .Where(pokemonCategory => pokemonCategory.CategoryId == categoryId)
                .Select(category=>category.Pokemon)
                .ToList();
        }

        public bool IsCategoryExists(int id)
        {
            return _context.Categories
                .Any(category => category.Id == id);
        }
    }
}
