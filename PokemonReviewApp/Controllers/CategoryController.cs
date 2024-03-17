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
                return BadRequest(ModelState);
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
                return BadRequest(ModelState);
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
                return BadRequest(ModelState);

            return Ok(pokemons);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateCategory([FromBody] CategoryDTO categoryDTO)        
        {
            if(categoryDTO == null)
            {
                return BadRequest(ModelState);
            }
            var category = _categoryRepository.GetCategories()
               .Where(category => category.Name.Trim().ToUpper() == categoryDTO.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if(category != null)
            {
                ModelState.AddModelError("", "Category Already Exists");
                return StatusCode(422, ModelState);
            }
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var categoryMap = _mapper.Map<Category>(categoryDTO);

            if (!_categoryRepository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }


    }
}
            