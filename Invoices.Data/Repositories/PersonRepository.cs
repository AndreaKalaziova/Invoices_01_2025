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

using Invoices.Data.Interfaces;
using Invoices.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Invoices.Data.Repositories;
/// <summary>
/// repository class for managing Person(company) entities in the db
/// Inherits basic CRUD operations from the vase repository
/// </summary>
public class PersonRepository : BaseRepository<Person>, IPersonRepository
{
    public PersonRepository(InvoicesDbContext invoicesDbContext) : base(invoicesDbContext)
    {
    }

    /// <summary>
    /// getting all the persons that are active (hidden = was deleted / hidden = false --> person is active)
    /// </summary>
    /// <param name="hidden" is set to false ></param>
    /// <returns>List of persons per hidden criteria true/false</returns>
    public IList<Person> GetAllByHidden(bool hidden)
    {
        return dbSet
            .Where(p => p.Hidden == hidden) //filters person by hidden true/false (default)
            .Include(p => p.InvoicesPerPerson)  // includes invoices by person 
            .ToList();  //founded entities included in a List
    }

    /// <summary>
    /// search person by Person Id in db
    /// </summary>
    /// <param name="id"></param>
    /// <returns>requested person by Id</returns>
    public Person? FindById(ulong id)
    {
        return dbSet.Find(id);
    }
}