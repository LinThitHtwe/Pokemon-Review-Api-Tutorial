using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository;

public interface ICategoryRepository
{
    bool CreateCategory(Category category);
    bool DeleteCategory(int id);
    ICollection<Category> GetCategories();
    Category GetCategoryById(int id);
    ICollection<Pokemon> GetPokemonsByCategory(int categoryId);
    bool IsCategoryExists(int id);
    bool Save();
    bool UpdateCategory(Category category);
}