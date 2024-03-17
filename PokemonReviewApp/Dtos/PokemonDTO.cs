using PokemonReviewApp.Models;

namespace PokemonReviewApp.Dtos
{
    public class PokemonDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
       // public ICollection<Review> Reviews { get; set; }
       // public ICollection<PokemonOwner> PokemonOwners { get; set; }
       // public ICollection<PokemonCategory> PokemonCategories { get; set; }
    }
}
