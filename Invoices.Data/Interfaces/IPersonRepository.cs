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

using Invoices.Data.Models;

namespace Invoices.Data.Interfaces;

public interface IPersonRepository : IBaseRepository<Person>
{
	/// <summary>
	///  getting all the persons that are active (hidden = was deleted / hidden = false --> person is active)
	/// </summary>
	/// <param name="hidden"></param>
	/// <returns>List of persons per hidden criteria true/false</returns>
	IList<Person> GetAllByHidden(bool hidden);
	/// <summary>
	/// search person by PersonId in db
	/// </summary>
	/// <param name="id"></param>
	/// <returns>requested person by Id</returns>
	Person? FindById(ulong id);
	/// <summary>
	/// Checks if a person with the given IdentificationNumber exists in the repository
	/// </summary>
	/// <param name="identificationNumber">The IdentificationNumber to check.</param>
	/// <returns>True if a person with the IdentificationNumber exists; otherwise, false.</returns>
	bool ExistsWithIdentificationNumber(string identificationNumber);
}