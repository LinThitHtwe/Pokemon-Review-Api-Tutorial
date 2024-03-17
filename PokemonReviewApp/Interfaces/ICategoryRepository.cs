using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategoryById(int id);
        ICollection<Pokemon> GetPokemonsByCategory(int categoryId);
        bool IsCategoryExists(int id);
        bool CreateCategory(Category category);
        bool Save();

    }
}
