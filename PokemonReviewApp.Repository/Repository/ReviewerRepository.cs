using PokemonReviewApp.Data;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository;

public class ReviewerRepository : IReviewerRepository
{
    private readonly DataContext _context;
    public ReviewerRepository(DataContext context)
    {
        _context = context;
    }

    public bool CreateReviewer(Reviewer reviewer)
    {
        _context.Add(reviewer);
        return Save();
    }

    public bool DeleteReviewer(int id)
    {
        Reviewer reviewer = GetReviewerByID(id);
        _context.Remove(reviewer);
        return Save();
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
            .OrderBy(reviewer => reviewer.Id)
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

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0;
    }

    public bool UpdateReviewer(Reviewer reviewer)
    {
        _context.Update(reviewer);
        return Save();
    }
}
