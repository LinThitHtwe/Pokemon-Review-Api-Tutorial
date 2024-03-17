using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dtos;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerRepository ownerRepository, IPokemonRepository pokemonRepository, IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwners() 
        {
            var owners = _mapper.Map<List<OwnerDTO>>(_ownerRepository.GetOwners());


            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(owners);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetOwnerById(int id)
        {
            if(!_ownerRepository.IsOwnerExists(id))
            {
                return NotFound();
            }

            var owner = _mapper.Map<OwnerDTO>(_ownerRepository.GetOwnerById(id));
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(owner);
        }

        [HttpGet("{id}/pokemons")]
        [ProducesResponseType(200, Type = typeof(ICollection<Pokemon>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPokemonsByOwnerId(int id)
        {

            if (!_ownerRepository.IsOwnerExists(id))
            {
                return NotFound();
            }

            var pokemons = _mapper.Map<List<PokemonDTO>>(_ownerRepository.GetPokemonByOwner(id));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(pokemons);
        }

        [HttpGet("pokemon/{id}")]
        [ProducesResponseType(200, Type = typeof(ICollection<Pokemon>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetOwnerByPokemonId(int id)
        {
            if (!_pokemonRepository.IsPokemonExists(id))
            {
                return NotFound();
            }

            var owner = _ownerRepository.GetOwnerOfAPokemon(id);
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(owner);
        }
        


    }
}
