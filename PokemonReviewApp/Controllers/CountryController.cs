using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dtos;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController:Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IOwnerRepository ownerRepository,IMapper mapper)
        {
            _countryRepository = countryRepository;
            _ownerRepository = ownerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Country>))]
        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDTO>>(_countryRepository.GetCountries());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(countries);

        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(404)]
        public IActionResult GetCountryId(int id)
        {
            if(!_countryRepository.IsCountryExist(id))
            {
                return NotFound();
            }
            var country = _mapper.Map<CountryDTO>(_countryRepository.GetCountryByID(id));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(country);
        }

        [HttpGet("owners/{id}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult GetCountryByOwnerId(int id)
        {
            if (!_ownerRepository.IsOwnerExists(id))
            {
                return NotFound();
            }

            var country = _mapper.Map<CountryDTO>(
                _countryRepository.GetCountryByOwner(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(country);
        }

        [HttpGet("{id}/owners")]
        [ProducesResponseType(200,Type= typeof(ICollection<Owner>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult GetOwnersByCountry(int id)
        {
            if (!_countryRepository.IsCountryExist(id))
            {
                return NotFound();
            }

            var owners = _mapper.Map<List<Owner>>(_countryRepository.GetOwnersByCountryId(id));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owners);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateCountry([FromBody]CountryDTO countryDto)
        {
            if(countryDto == null)
            {
                return BadRequest(ModelState);
            }
            var country = _countryRepository
                                .GetCountries()
                                .Where(country => country.Name.Trim().ToUpper() == countryDto.Name.TrimEnd().ToUpper())
                                .FirstOrDefault();

            if(country != null)
            {
                ModelState.AddModelError("", "Country Already Exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var countryMap = _mapper.Map<Country>(countryDto);
            if (!_countryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went Wrong");
                return StatusCode(500,ModelState);
            }
            return Ok("Successfully created");
        }

        [HttpPut("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int countryId, [FromBody] CountryDTO countryDto)
        {
            if (countryDto == null)
                return BadRequest(ModelState);

            if (countryId != countryDto.Id)
                return BadRequest(ModelState);

            if (!_countryRepository.IsCountryExist(countryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var countryMap = _mapper.Map<Country>(countryDto);

            if (countryMap.Id != countryId)
            {
                return BadRequest();
            }

            if (!_countryRepository.UpdateCountry(countryMap))
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
        public IActionResult DeleteCountry(int id)
        {

            if (!_countryRepository.IsCountryExist(id))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (!_countryRepository.DeleteCountry(id))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Deleted");
        }

    }
}
