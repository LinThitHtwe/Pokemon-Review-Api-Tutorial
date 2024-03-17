using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReviewById(int id);
        bool IsReviewExist(int id);
        ICollection<Review> GetReviewsByPokemonId(int pokemonId);
        bool CreateReview(Review review);
        bool Save();

    }
}
