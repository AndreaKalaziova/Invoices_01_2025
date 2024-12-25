/*  _____ _______         _                      _
 * |_   _|__   __|       | |                    | |
 *   | |    | |_ __   ___| |___      _____  _ __| | __  ___ ____
 *   | |    | | '_ \ / _ \ __\ \ /\ / / _ \| '__| |/ / / __|_  /
 *  _| |_   | | | | |  __/ |_ \ V  V / (_) | |  |   < | (__ / /
 * |_____|  |_|_| |_|\___|\__| \_/\_/ \___/|_|  |_|\_(_)___/___|
 *
 *                      ___ ___ ___
 *                     | . |  _| . |  LICENCE
 *                     |  _|_| |___|
 *                     |_|
 *
 *    REKVALIFIKAČNÍ KURZY  <>  PROGRAMOVÁNÍ  <>  IT KARIÉRA
 *
 * Tento zdrojový kód je součástí profesionálních IT kurzů na
 * WWW.ITNETWORK.CZ
 *
 * Kód spadá pod licenci PRO obsahu a vznikl díky podpoře
 * našich členů. Je určen pouze pro osobní užití a nesmí být šířen.
 * Více informací na http://www.itnetwork.cz/licence
 */

using Invoices.Api.Interfaces;
using Invoices.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Invoices.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonsController : ControllerBase
{
    private readonly IPersonManager personManager;

    public PersonsController(IPersonManager personManager)
    {
        this.personManager = personManager;
    }

	/// <summary>
	/// get all Persons (active/not hidden)
	/// </summary>
	/// <returns>list of all active persons</returns>
	[HttpGet]
	public IActionResult GetPersons()
	{
		var persons = personManager.GetAllPersons();

		// in case of empty results return Not Found
		if (persons == null || !persons.Any())
		    return NoContent();

		return Ok(persons); // if found, return 200 OK with the persons list
	}

	/// <summary>
	/// get a specific person by its Id
	/// </summary>
	/// <param name="personId"></param>
	/// <returns>person by Id</returns>
	[HttpGet("{personId}")]     // above class add [ApiController / for display one person per Id - api/person/1
    public IActionResult GetPerson(ulong personId)
    {
        if (personId == 0)
            return BadRequest("PersonId musí být větší než 0");

        PersonDto? person = personManager.GetPerson(personId);

        //chec if person with this Id exist
        if (person is null)
            return BadRequest("Osoba nenalezena");

        return Ok(person);
    }

    /// <summary>
    /// add new Person
    /// </summary>
    /// <param name="person"></param>
    /// <returns>added person</returns>
    [HttpPost]
    public IActionResult AddPerson([FromBody] PersonDto person)
    {
        //check if PersonDto inserted as per reqirements
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		PersonDto? createdPerson = personManager.AddPerson(person);
        return StatusCode(StatusCodes.Status201Created, createdPerson);
    }

    /// <summary>
    /// Person is not deleted permanently, only hidden 
    /// </summary>
    /// <param name="personId"></param>
    [HttpDelete("{personId}")]
    public IActionResult DeletePerson(uint personId)
    {
		if (personId == 0)
			return BadRequest("PersonId musí být větší než 0");

        try
        {
            personManager.DeletePerson(personId);
            return NoContent();
        }
        catch (Exception ex)
        {
			return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); // 500 Internal Server Error
		}
	}

    /// <summary>
    /// update person, hide original Person, return new person with new Id
    /// </summary>
    /// <param name="personId"></param>
    /// <param name="updatePersonDto"></param>
    /// <returns>Updated person with new details and new Id</returns>
    [HttpPut("{personId}")]
    public IActionResult UpdatePerson(ulong personId, [FromBody] PersonDto updatePersonDto) 
    {
        //check input requiremnts of DTO
        if (!ModelState.IsValid)
			return BadRequest(ModelState);

		if (personId == 0)
			return BadRequest("PersonId musí být větší než 0");

        try
        {
            PersonDto? updatePerson = personManager.UpdatePerson(personId, updatePersonDto);

            if (updatePerson is null)
                return NotFound();

            return StatusCode(StatusCodes.Status200OK, updatePerson);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message); //400 Bad Request with errir message
        }
        catch (Exception ex)
        {
			return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); // Handle unexpected errors
		}
	}

	/// <summary>
	/// get statistics for all persons
	/// </summary>
	/// <returns>PersonId - its name - its revenue - its turnover - its profit</returns>
	[HttpGet("statistics")]
    public IActionResult GetStatisticsPerson()
    {
        List<StatisticsPersonDto?> statistics = personManager.GetStatisticsPerson();
	
        if (statistics == null || !statistics.Any())
            return NoContent();

        return Ok(statistics);
	}
}      
