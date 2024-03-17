using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dtos;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200,Type=typeof(ICollection<Category>))]
        public IActionResult GetCategories()
        {
            var categories = _mapper.Map<List<CategoryDTO>>(_categoryRepository.GetCategories());
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(categories);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetCategoryById(int id)
        {
            if (!_categoryRepository.IsCategoryExists(id))
            {
                return NotFound();
            }

            var category = _mapper.Map<CategoryDTO>(_categoryRepository.GetCategoryById(id));
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(category);
        }

        [HttpGet("{id}/pokemons")]
        [ProducesResponseType(200,Type = typeof(ICollection<Pokemon>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPokemonsByCategoryId(int id)
        {
            if (!_categoryRepository.IsCategoryExists(id))
            {
                return NotFound();
            }
            var pokemons = _mapper.Map<List<PokemonDTO>>(
                _categoryRepository.GetPokemonsByCategory(id));
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(pokemons);
        }


    }
}
            