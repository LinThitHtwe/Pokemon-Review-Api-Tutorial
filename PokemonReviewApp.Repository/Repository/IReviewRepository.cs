using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository;

public interface IReviewRepository
{
    bool CreateReview(Review review);
    bool DeleteReview(int id);
    Review GetReviewById(int id);
    ICollection<Review> GetReviews();
    ICollection<Review> GetReviewsByPokemonId(int pokemonId);
    bool IsReviewExist(int id);
    bool Save();
    bool UpdateReview(Review review);
}