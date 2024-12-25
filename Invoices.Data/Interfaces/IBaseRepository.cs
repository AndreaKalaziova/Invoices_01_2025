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

namespace Invoices.Data.Interfaces;
/// <summary>
/// interface for BaseRepository with base CRUD operations that person and invoice will inherit
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IBaseRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// get all entities of type TEntity
    /// </summary>
    /// <returns>List of all TEntities</returns>
    IList<TEntity> GetAll();

    /// <summary>
    /// find and get anTEntity by its unique Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    TEntity? FindById(ulong id);

    /// <summary>
    /// insert a new TEntity into db
    /// </summary>
    /// <param name="entity"></param>
    /// <returns>the inserted TEntity</returns>
    TEntity Insert(TEntity entity);
    /// <summary>
    /// update existing TEntity in db
    /// </summary>
    /// <param name="entity"></param>
    /// <returns>updated TEntity</returns>
    TEntity Update(TEntity entity);
    /// <summary>
    /// delete TEntity from data source (person stays only hidden, invoice is deleted from db)
    /// </summary>
    /// <param name="id"></param>
    void Delete(ulong id);
    /// <summary>
    /// check if TEntity exists 
    /// </summary>
    /// <param name="id">id to be checked</param>
    /// <returns>true = id exist, false = id does not exists</returns>
    bool ExistsWithId(ulong id);
}