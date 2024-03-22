namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewController : Controller
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IPokemonRepository _pokemonRepository;
    private readonly IReviewerRepository _reviewerRepository;
    private readonly IMapper _mapper;

    public ReviewController(IReviewRepository reviewRepository,
                            IPokemonRepository pokemonRepository,
                            IReviewerRepository reviewerRepository,
                            IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _pokemonRepository = pokemonRepository;
        _reviewerRepository = reviewerRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200,Type = typeof(ICollection<Review>))]
    [ProducesResponseType(400)]
    public IActionResult GetReviews()
    {
        var reviews = _mapper.Map<List<ReviewDTO>>(_reviewRepository.GetReviews());
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(reviews);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(Review))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult GetReviewById(int id)
    {
        if (!_reviewRepository.IsReviewExist(id))
        {
            return NotFound();
        }
        var review = _mapper.Map<ReviewDTO>(_reviewRepository.GetReviewById(id));
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok(review);
    }

    [HttpGet("pokemon/{id}")]
    [ProducesResponseType(200, Type = typeof(ICollection<Review>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult GetReviewsByPokemonId(int id)
    {
        if(!_pokemonRepository.IsPokemonExists(id))
        {
            return NotFound();
        }
        var reviews = _mapper.Map<List<ReviewDTO>>(_reviewRepository.GetReviewsByPokemonId(id));
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok(reviews);
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public IActionResult CreateReview([FromQuery] int reviewerId, [FromQuery]int pokemonId, [FromBody]ReviewDTO reviewDTO)
    {
        if (reviewDTO == null)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!_pokemonRepository.IsPokemonExists(pokemonId))
        {
            return NotFound("Pokemon Not Found");
        }
        if (!_reviewerRepository.IsReviewerExists(reviewerId))
        {
            return NotFound("Reviewer Not Found");
        }

        var reviewMap = _mapper.Map<Review>(reviewDTO);

        reviewMap.Pokemon = _pokemonRepository.GetPokemonById(pokemonId);
        reviewMap.Reviewer = _reviewerRepository.GetReviewerByID(reviewerId);

        if (!_reviewRepository.CreateReview(reviewMap))
        {
            ModelState.AddModelError("", "Something went wrong");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }

    [HttpPut("{reviewId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDTO reviewDto)
    {
        if (reviewDto == null)
            return BadRequest(ModelState);

        if (reviewId != reviewDto.Id)
            return BadRequest(ModelState);

        if (!_reviewRepository.IsReviewExist(reviewId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var reviewMap = _mapper.Map<Review>(reviewDto);
        if(reviewMap.Id != reviewId)
        {
            return BadRequest(ModelState);
        }

        if (!_reviewRepository.UpdateReview(reviewMap))
        {
            ModelState.AddModelError("", "Something went wrong");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public IActionResult DeleteReview(int id)
    {

        if (!_reviewRepository.IsReviewExist(id))
        {
            return NotFound();
        }
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        if (!_reviewRepository.DeleteReview(id))
        {
            ModelState.AddModelError("", "Something went wrong");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully Deleted");
    }
}           
