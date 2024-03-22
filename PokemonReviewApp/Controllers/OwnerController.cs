namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OwnerController : Controller
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly IPokemonRepository _pokemonRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IMapper _mapper;

    public OwnerController(IOwnerRepository ownerRepository, 
                            IPokemonRepository pokemonRepository, 
                            ICountryRepository countryRepository,
                            IMapper mapper)
    {
        _ownerRepository = ownerRepository;
        _pokemonRepository = pokemonRepository;
        _countryRepository = countryRepository;
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

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public IActionResult CreateOwner([FromQuery] int countryId,[FromBody] OwnerDTO ownerDTO)
    {
        if(ownerDTO == null)
        {
            return BadRequest(ModelState);
        }
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var ownerMap = _mapper.Map<Owner>(ownerDTO);
        ownerMap.Country = _countryRepository.GetCountryByID(countryId);
        if (!_ownerRepository.CreateOwner(ownerMap))
        {
            ModelState.AddModelError("", "Something went Wrong");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }

    [HttpPut("{ownerId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateOwner(int ownerId, [FromBody] OwnerDTO ownerDto)
    {
        if (ownerDto == null)
            return BadRequest(ModelState);

        if (ownerId != ownerDto.Id)
            return BadRequest(ModelState);

        if (!_ownerRepository.IsOwnerExists(ownerId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var ownerMap = _mapper.Map<Owner>(ownerDto);

        if(ownerMap.Id != ownerId)
        {
            return BadRequest();
        }

        if (!_ownerRepository.UpdateOwner(ownerMap))
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
    public IActionResult DeleteOwner(int id)
    {

        if (!_ownerRepository.IsOwnerExists(id))
        {
            return NotFound();
        }
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        if (!_ownerRepository.DeleteOwner(id))
        {
            ModelState.AddModelError("", "Something went wrong");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully Deleted");
    }

}
