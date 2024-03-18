using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository;

public interface IReviewerRepository
{
    bool CreateReviewer(Reviewer reviewer);
    bool DeleteReviewer(int id);
    Reviewer GetReviewerByID(int id);
    ICollection<Reviewer> GetReviewers();
    ICollection<Review> GetReviewsByReviewerId(int id);
    bool IsReviewerExists(int id);
    bool Save();
    bool UpdateReviewer(Reviewer reviewer);
}