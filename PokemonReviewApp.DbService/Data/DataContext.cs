using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Data;

public class DataContext : DbContext
{
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options): base(options)
    {

    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Owner> Owners { get; set; }
    public DbSet<Pokemon> Pokemons { get; set; }
    public DbSet<PokemonOwner> PokemonOwners {  get; set; } 
    public DbSet<PokemonCategory> PokemonCategories { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Reviewer> Reviewers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PokemonCategory>()
            .HasKey(pokemonCategory => new { pokemonCategory.PokemonId, pokemonCategory.CategoryId });
        modelBuilder.Entity<PokemonCategory>()
            .HasOne(pokemon => pokemon.Pokemon)
            .WithMany(pokemonCategory => pokemonCategory.PokemonCategories)
            .HasForeignKey(pokemon => pokemon.PokemonId);
        modelBuilder.Entity<PokemonCategory>()
            .HasOne(pokemon => pokemon.Category)
            .WithMany(pokemonCategory => pokemonCategory.PokemonCategories)
            .HasForeignKey(category => category.CategoryId);


        modelBuilder.Entity<PokemonOwner>()
            .HasKey(pokemonOwner => new { pokemonOwner.PokemonId, pokemonOwner.OwnerId });
        modelBuilder.Entity<PokemonOwner>()
            .HasOne(pokemon => pokemon.Pokemon)
            .WithMany(pokemonOwner => pokemonOwner.PokemonOwners)
            .HasForeignKey(pokemon => pokemon.PokemonId);
        modelBuilder.Entity<PokemonOwner>()
            .HasOne(pokemon => pokemon.Owner)
            .WithMany(pokemonOwner => pokemonOwner.PokemonOwner)
            .HasForeignKey(owner => owner.OwnerId);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            options.UseSqlServer("Data Source=.;Initial Catalog=pokemonreview;User ID=sa;Password=sasa@123;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }
    }
}
