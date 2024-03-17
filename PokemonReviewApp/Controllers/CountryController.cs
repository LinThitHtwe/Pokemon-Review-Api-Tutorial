using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dtos;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

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
                return BadRequest();
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
                return BadRequest();

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
                return BadRequest();
            return Ok(owners);
        }

    }
}
