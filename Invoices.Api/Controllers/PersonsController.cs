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
using Invoices.Api.Managers;
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


    [HttpGet]
    public IEnumerable<PersonDto> GetPersons()
    {
        return personManager.GetAllPersons();
    }

    //api/person/1
    [HttpGet("{personId}")]           //pridano + nad tridu [ApiController] / pro vypis jedne osoby dle Id person/1
    public IActionResult GetPerson(ulong personId)
    {
        PersonDto? person = personManager.GetPerson(personId);

        if (person is null)
        {
            return NotFound();
        }

        return Ok(person);
    }

    [HttpPost]
    public IActionResult AddPerson([FromBody] PersonDto person)
    {
        PersonDto? createdPerson = personManager.AddPerson(person);
        return StatusCode(StatusCodes.Status201Created, createdPerson);
    }

    [HttpDelete("{personId}")]
    public IActionResult DeletePerson(uint personId)
    {
        personManager.DeletePerson(personId);
        return NoContent();
    }

    [HttpPut("{personId}")]
    public IActionResult UpdatePerson(ulong personId, [FromBody] PersonDto updatePersonDto) 
    {
            PersonDto? updatePerson = personManager.UpdatePerson(personId, updatePersonDto);
            if (updatePerson is null)
            {
                return NotFound();
            }

            return StatusCode(StatusCodes.Status202Accepted, updatePerson);
    }

    [HttpGet("statistics")]
    public IActionResult GetStatisticsPerson()
    {
        List<StatisticsPersonDto?> statistics = personManager.GetStatisticsPerson();
		return Ok(statistics);
	}
}      
