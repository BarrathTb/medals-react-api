
using Microsoft.AspNetCore.Mvc;
using Medals.Model;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.JsonPatch;


[ApiController]
[Route("[controller]")]

public class CountriesController : ControllerBase
{
  private readonly DataContext _context;

  public CountriesController(DataContext context)
  {
    _context = context;
  }

  // GET: Api/Countries
  [HttpGet, SwaggerOperation(Summary = "Get all the countries")]
  public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
  {
    return await _context.Countries.ToListAsync();
  }


  // GET: Api/Countries/5
  [HttpGet("{id}")]
  public async Task<ActionResult<Country>> GetCountry(int id)
  {
    var country = await _context.Countries.FindAsync(id);
    if (country == null)
    {
      return NotFound();
    }
    return country;
  }


  // POST: Api/AddCountry
  [HttpPost, SwaggerOperation(Summary = "Create a new country")]
  public async Task<ActionResult<Country>> AddCountry(Country country)
  {
    _context.Countries.Add(country);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetCountry), new { id = country.Id }, country);
  }

  // PUT: Api/Countries/5 update a country
  [HttpPut("{id}", Name = "UpdateCountry"), SwaggerOperation(Summary = "Update a country")]
  public async Task<IActionResult> PutCountry(int id, Country country)
  {
    if (id != country.Id)
    {
      return BadRequest();
    }

    _context.Entry(country).State = EntityState.Modified;
    try
    {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
      if (!_context.Countries.Any(e => e.Id == id))
      {
        return NotFound();
      }
      else
      {
        throw;
      }
    }

    return Ok();
  }

  // DELETE: Api/Countries/5 delete a country
  [HttpDelete("{id}", Name = "DeleteCountry"), SwaggerOperation(Summary = "Delete a country")]
  public async Task<ActionResult<Country>> DeleteCountry(int id)
  {
    var country = await _context.Countries.FindAsync(id);
    if (country == null)
    {
      return NotFound();
    }

    _context.Countries.Remove(country);
    await _context.SaveChangesAsync();

    return country;
  }

  // PATCH: Api/Countries/5/Medals add a medal to a country or remove
  // http patch member of collection
  [HttpPatch("{id}"), SwaggerOperation(summary: "update member from collection", null), ProducesResponseType(typeof(Country), 204), SwaggerResponse(204, "No Content")]
  // update country (specific fields)
  public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<Country> patch)
  {
    Country country = await _context.Countries.FindAsync(id);
    if (country == null)
    {
      return NotFound();
    }
    patch.ApplyTo(country);
    await _context.SaveChangesAsync();
    return Ok();
  }

  private bool CountryExists(int id)
  {
    return _context.Countries.Any(e => e.Id == id);
  }
}
