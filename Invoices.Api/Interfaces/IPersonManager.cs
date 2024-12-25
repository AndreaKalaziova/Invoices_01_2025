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

using Invoices.Api.Models;

namespace Invoices.Api.Interfaces;

public interface IPersonManager
{
	/// <summary>
	/// get all Persons that are active (not hidden)
	/// </summary>
	/// <returns>List of non-hodden Persons mapped to PersonDto objects</returns>
	IList<PersonDto> GetAllPersons();
	/// <summary>
	/// add new Person to db
	/// </summary>
	/// <param name="personDto"></param>
	/// <returns>added Person as PersonDto object</returns>
	PersonDto AddPerson(PersonDto personDto);
	/// <summary>
	/// get a person by its Id
	/// </summary>
	/// <param name="personId"></param>
	/// <returns>PersonDto object of the found Person, or null if not found</returns>
	PersonDto? GetPerson(ulong personId);   
	/// <summary>
	/// person is not deleted from db, only hidden
	/// </summary>
	/// <param name="personId"></param>
	void DeletePerson(uint personId);
	/// <summary>
	/// update Person with new data, generating new Id, original person with origin Id remains hidden
	/// </summary>
	/// <param name="personId"></param>
	/// <param name="updatePersonDto"></param>
	/// <returns>updated Person with new Id</returns>
	PersonDto? UpdatePerson(ulong id, PersonDto updatePerson ); 
	/// <summary>
	/// get statistics for all Persons
	/// </summary>
	/// <returns>PersonId - its name - its revenue - its turnover - its profit</returns>
	List<StatisticsPersonDto> GetStatisticsPerson();

}

