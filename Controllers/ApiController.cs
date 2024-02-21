
using Microsoft.AspNetCore.Mvc;
using Medals.Model;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using Swashbuckle.AspNetCore.Annotations;


[ApiController]
[Route("api/[controller]")]

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
  [HttpPatch("{id}/AddMedal"), SwaggerOperation(Summary = "Add a medal to a country")]
  public async Task<IActionResult> AddMedals(int id, string metalType)
  {
    var country = await _context.Countries.FindAsync(id);
    if (country == null)
    {
      return NotFound();
    }
    switch (metalType.ToLower())
    {
      case "gold":
        country.Gold += 1;
        break;
      case "silver":
        country.Silver += 1;
        break;
      case "bronze":
        country.Bronze += 1;
        break;
      default:
        return BadRequest("Invalid medal type.");
    }


    await _context.SaveChangesAsync();

    return Ok(); // or return Ok(existingMedal);
  }

  [HttpPatch("{id}/RemoveMedal"), SwaggerOperation(Summary = "Remove a medal from a country")]
  public async Task<IActionResult> RemoveMedals(int id, string metalType)
  {
    var country = await _context.Countries.FindAsync(id);
    if (country == null)
    {
      return NotFound();
    }
    switch (metalType.ToLower())
    {
      case "gold":
        country.Gold -= 1;
        break;
      case "silver":
        country.Silver -= 1;
        break;
      case "bronze":
        country.Bronze -= 1;
        break;
      default:
        return BadRequest("Invalid medal type.");

    }
    await _context.SaveChangesAsync();
    return Ok();
  }

  private bool CountryExists(int id)
  {
    return _context.Countries.Any(e => e.Id == id);
  }
}
