﻿using PokemonReviewApp.Data;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository;

public class CategoryRepository : ICategoryRepository
{
    private DataContext _context;

    public CategoryRepository(DataContext context)
    {
        _context = context;
    }

    public bool CreateCategory(Category category)
    {
        _context.Add(category);
        return Save();
    }

    public bool DeleteCategory(int id)
    {
        Category category = GetCategoryById(id);
        _context.Remove(category);
        return Save();
    }

    public ICollection<Category> GetCategories()
    {
        return _context.Categories
            .OrderBy(category => category.Id)
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
            .Select(category => category.Pokemon)
            .ToList();
    }

    public bool IsCategoryExists(int id)
    {
        return _context.Categories
            .Any(category => category.Id == id);
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0;
    }

    public bool UpdateCategory(Category category)
    {
        _context.Update(category);
        return Save();
    }
}
