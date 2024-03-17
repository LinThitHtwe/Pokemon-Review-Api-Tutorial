using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dtos;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
      
        public PokemonController(IPokemonRepository pokemonRepository,
                                    IOwnerRepository ownerRepository,
                                    ICategoryRepository categoryRepository,
                                    IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _ownerRepository = ownerRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemons()
        {
             var pokemons = _mapper.Map<List<PokemonDTO>>(_pokemonRepository.GetPokemons());
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            return Ok(pokemons);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonById(int id)
        {
            if (!_pokemonRepository.IsPokemonExists(id))
            {
                return NotFound();
            }
            var pokemon = _mapper.Map<PokemonDTO>(_pokemonRepository.GetPokemonById(id));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
        
            return Ok(pokemon);

        }

        [HttpGet("{id}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int id)
        {
            if (!_pokemonRepository.IsPokemonExists(id))
            {
                return NotFound();
            }

            decimal rating = _pokemonRepository.GetPokemonRating(id);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(rating);

        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(422)]
        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody] PokemonDTO pokemonDTO)
        {
            if(pokemonDTO == null)
            {
                return BadRequest(ModelState);
            }
            if (!_ownerRepository.IsOwnerExists(ownerId))
            {
                return NotFound("Owner Not Found");
            }
            if (!_categoryRepository.IsCategoryExists(categoryId))
            {
                return NotFound("Category Not Found");
            }
            var existingPokemon = _pokemonRepository.GetPokemons()
                                                            .Where(pokemon=>pokemon.Name.Trim().ToUpper()== pokemonDTO.Name.TrimEnd().ToUpper())
                                                            .FirstOrDefault();
            if(existingPokemon != null)
            {
                ModelState.AddModelError("", "Pokemon Alread Exist");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pokemonMap = _mapper.Map<Pokemon>(pokemonDTO);
            if(!_pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went Wrong");
                return StatusCode(500,ModelState);
            }  

            return Ok("Successfully created");
        }

        [HttpPut("{pokemonId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePokemon(int pokemonId,
            [FromBody] PokemonDTO pokemonDto)
        {
            if (pokemonDto == null)
            {
                return BadRequest(ModelState);
            }

            if (pokemonId != pokemonDto.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_pokemonRepository.IsPokemonExists(pokemonId))
                return NotFound();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pokemonMap = _mapper.Map<Pokemon>(pokemonDto);
            if(pokemonMap.Id != pokemonId)
            {
                return BadRequest(ModelState);
            }

            if (!_pokemonRepository.UpdatePokemon(pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Updated");
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeletePokemon(int id)
        {

            if (!_pokemonRepository.IsPokemonExists(id))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (!_pokemonRepository.DeletePokemon(id))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Deleted");
        }

    }
}
