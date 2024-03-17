using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext _context;
        public ReviewerRepository(DataContext context)
        {
            _context = context;
        }
        public Reviewer GetReviewerByID(int id)
        {
            return _context.Reviewers
                .Where(reviewer => reviewer.Id == id)
                .FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _context.Reviewers
                .OrderBy(reviewer =>reviewer.Id)
                .ToList();
        }

        public ICollection<Review> GetReviewsByReviewerId(int id)
        {
            return _context.Reviews
                .Where(review => review.Reviewer.Id == id)
                .ToList();
        }

        public bool IsReviewerExists(int id)
        {
            return _context.Reviewers.Any(reviewer => reviewer.Id == id);
        }
    }
}
