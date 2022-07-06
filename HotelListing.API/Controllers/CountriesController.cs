﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using AutoMapper;
using HotelListing.API.Models.Country;
using HotelListing.API.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace HotelListing.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CountriesController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly ICountriesRepository _countriesRepository;
		private readonly ILogger<CountriesController> _logger;

		public CountriesController(IMapper mapper, ICountriesRepository countriesRepository, ILogger<CountriesController> logger)
		{
			_mapper = mapper;
			_countriesRepository = countriesRepository;
			_logger = logger;
		}

		// GET: api/Countries
		[HttpGet]
		public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
		{
			var countries = await _countriesRepository.GetAllAsync();
			var countriesDto = _mapper.Map<List<GetCountryDto>>(countries);
			return Ok(countriesDto);
		}

		// GET: api/Countries/5
		[HttpGet("{id}")]
		public async Task<ActionResult<CountryDto>> GetCountry(int id)
		{
			var country = await _countriesRepository.GetDetails(id);

			if (country == null)
			{
				_logger.LogWarning("Record not found in {NameOfMethod} with id {Id}.", nameof(GetCountry), id)
				return NotFound();
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
