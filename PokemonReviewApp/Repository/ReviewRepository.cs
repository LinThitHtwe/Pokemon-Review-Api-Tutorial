using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;
        public ReviewRepository(DataContext context)
        {
            _context = context;
        }

        public Review GetReviewById(int id)
        {
            return _context.Reviews
                .Where(review => review.Id == id)
                .FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.OrderBy(review=>review.Id).ToList();
        }

        public ICollection<Review> GetReviewsByPokemonId(int pokemonId)
        {
            return _context.Reviews.Where(review => review.Pokemon.Id == pokemonId).ToList();
        }

        public bool IsReviewExist(int id)
        {
           return _context.Reviews.Any(review => review.Id == id);
        }
    }
}
