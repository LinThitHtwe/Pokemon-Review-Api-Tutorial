namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewerController : Controller
{
    private readonly IReviewerRepository _reviewerRepository;
    private readonly IMapper _mapper;

    public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
    {
        _reviewerRepository = reviewerRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200,Type = typeof(ICollection<Reviewer>))]
    [ProducesResponseType(400)]
    public IActionResult GetReviewers() {

        var reviewers = _mapper.Map<List<ReviewerDTO>>(_reviewerRepository.GetReviewers());
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok(reviewers);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(Reviewer))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult GetReviewerById(int id)
    {
        if (!_reviewerRepository.IsReviewerExists(id))
        {
            return NotFound();
        }
        var review = _mapper.Map<ReviewerDTO>(_reviewerRepository.GetReviewerByID(id));
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok(review);
    }

    [HttpGet("{id}/reviews")]
    [ProducesResponseType(200, Type = typeof(ICollection<Review>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult GetReviewsByReviewerId(int id)
    {

        if (!_reviewerRepository.IsReviewerExists(id))
        {
            return NotFound();
        }
        var reviews = _reviewerRepository.GetReviewsByReviewerId(id);
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
    public IActionResult CreateReviewer([FromBody] ReviewDTO reviewDTO)
    {
        if (reviewDTO == null)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var reviewerMap = _mapper.Map<Reviewer>(reviewDTO);

        if (!_reviewerRepository.CreateReviewer(reviewerMap))
        {
            ModelState.AddModelError("", "Something went wrong");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }

    [HttpPut("{reviewerId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewerDTO reviewerDto)
    {
        if (reviewerDto == null)
            return BadRequest(ModelState);

        if (reviewerId != reviewerDto.Id)
            return BadRequest(ModelState);

        if (!_reviewerRepository.IsReviewerExists(reviewerId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var reviewerMap = _mapper.Map<Reviewer>(reviewerDto);
        if(reviewerMap.Id != reviewerId)
        {
            return BadRequest(ModelState);
        }

        if (!_reviewerRepository.UpdateReviewer(reviewerMap))
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
    public IActionResult DeleteReviewer(int id)
    {

        if (!_reviewerRepository.IsReviewerExists(id))
        {
            return NotFound();
        }
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        if (!_reviewerRepository.DeleteReviewer(id))
        {
            ModelState.AddModelError("", "Something went wrong");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully Deleted");
    }
}
