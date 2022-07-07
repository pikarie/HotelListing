using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Exceptions;
using HotelListing.API.Helpers;
using HotelListing.API.Models.Country;
using HotelListing.API.Models.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	//[Route("v{version:apiVersion}/[controller]")]
	[ApiVersion("1.0")]
	public class CountriesController : ControllerBase
	{
		private readonly ILogger<CountriesController> _logger;
		private readonly IMapper _mapper;
		private readonly ICountriesRepository _countriesRepository;
		private readonly IUriService _uriService;

		public CountriesController(ILogger<CountriesController> logger, IMapper mapper, ICountriesRepository countriesRepository, IUriService uriService)
		{
			_logger = logger;
			_mapper = mapper;
			_countriesRepository = countriesRepository;
			this._uriService = uriService;
		}

		// GET: api/Countries
		[HttpGet]
		[EnableQuery]
		public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
		{
			var countries = await _countriesRepository.GetAllAsync();
			var countriesDto = _mapper.Map<List<GetCountryDto>>(countries);
			return Ok(countriesDto);
		}

		// GET: api/Countries/paged?PageNumber=1&PageSize=8
		[HttpGet("paged")]
		public async Task<ActionResult> GetPagedCountries([FromQuery] PaginationFilter filter)
		{
			var route = Request.Path.Value;
			var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
			var pagedCountries = await _countriesRepository.GetAllAsync<GetCountryDto>(validFilter);
			var totalRecords = await _countriesRepository.GetCountAsync();
			var pagedResponse = PaginationHelper.CreatePagedReponse(_mapper.Map<List<Country>>(pagedCountries), validFilter, totalRecords, _uriService, route);

			return Ok(pagedResponse);
		}

		// GET: api/Countries/5
		[HttpGet("{id}")]
		public async Task<ActionResult<CountryDto>> GetCountry(int id)
		{
			var country = await _countriesRepository.GetDetails(id);

			if (country == null)
			{
				throw new NotFoundException(nameof(GetCountry), id);
				//_logger.LogWarning("Record not found in {NameOfMethod} with id {Id}.", nameof(GetCountry), id);
				//return NotFound();
			}

			var countryDto = _mapper.Map<CountryDto>(country);
			return Ok(countryDto);
		}

		// PUT: api/Countries/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		[Authorize]
		public async Task<IActionResult> PutCountry(int id, ModifyCountryDto countryDto)
		{
			if (id != countryDto.Id)
			{
				return BadRequest();
			}

			var country = await _countriesRepository.GetAsync(id);
			if (country == null)
			{
				return NotFound();
			}

			_mapper.Map(countryDto, country);

			try
			{
				await _countriesRepository.UpdateAsync(country);
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!await _countriesRepository.Exists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// POST: api/Countries
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		[Authorize]
		public async Task<ActionResult<Country>> PostCountry(CreateCountryDto countryDto)
		{
			var country = _mapper.Map<Country>(countryDto);

			await _countriesRepository.AddAsync(country);

			return CreatedAtAction("GetCountry", new { id = country.Id }, country);
		}

		// DELETE: api/Countries/5
		[HttpDelete("{id}")]
		[Authorize(Roles = "Administrator")]
		public async Task<IActionResult> DeleteCountry(int id)
		{
			var country = await _countriesRepository.GetAsync(id);
			if (country == null)
			{
				return NotFound();
			}

			await _countriesRepository.DeleteAsync(id);

			return NoContent();
		}
	}
}
