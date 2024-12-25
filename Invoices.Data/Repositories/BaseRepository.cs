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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Invoices.Data.Repositories;

/// <summary>
/// BaseRepository with base CRUD operations that person and invoice will inherit
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly InvoicesDbContext invoicesDbContext;
    protected readonly DbSet<TEntity> dbSet;


    public BaseRepository(InvoicesDbContext invoicesDbContext)
    {
        this.invoicesDbContext = invoicesDbContext;
        dbSet = invoicesDbContext.Set<TEntity>();
    }

	/// <summary>
	/// find and get anTEntity by its unique Id
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public TEntity? FindById(ulong id)
    {
        return dbSet.Find(id);
    }
	/// <summary>
	/// check if TEntity exists 
	/// </summary>
	/// <param name="id">id to be checked</param>
	/// <returns>true = id exist, false = id does not exists</returns>
	public bool ExistsWithId(ulong id)
    {
		// Attempts to find the entity with the given ID in the database.
		TEntity? entity = dbSet.Find(id);
		// If the entity is found, detach it from the context to prevent tracking.
		if (entity is not null)
            invoicesDbContext.Entry(entity).State = EntityState.Detached;
        return entity is not null;
    }
	/// <summary>
	/// get all entities of type TEntity
	/// </summary>
	/// <returns>List of all TEntities</returns>
	public IList<TEntity> GetAll()
    {
        return dbSet.ToList();
    }
	/// <summary>
	/// insert a new TEntity into db
	/// </summary>
	/// <param name="entity"></param>
	/// <returns>the inserted TEntity</returns>
	public TEntity Insert(TEntity entity)
    {
        EntityEntry<TEntity> entityEntry = dbSet.Add(entity);
        invoicesDbContext.SaveChanges();      // Saves the changes to the database.
		return entityEntry.Entity;
    }
	/// <summary>
	/// update existing TEntity in db
	/// </summary>
	/// <param name="entity"></param>
	/// <returns>updated TEntity</returns>
	public TEntity Update(TEntity entity)
    {
        EntityEntry<TEntity> entityEntry = dbSet.Update(entity);
        invoicesDbContext.SaveChanges();      // Saves the changes to the database.
		return entityEntry.Entity;
    }
	/// <summary>
	/// delete TEntity from data source (person stays only hidden, invoice is deleted from db)
	/// </summary>
	/// <param name="id"></param>
	public void Delete(ulong id)
    {
		// Finds the entity by its ID.
		TEntity? entity = dbSet.Find(id);
		// If the entity doesn't exist, exit the method.
		if (entity is null)
            return;

        try
        {
			// Removes the entity from the database set.
			dbSet.Remove(entity);
            invoicesDbContext.SaveChanges();
        }
        catch
        {
			// If an error occurs, reset the entity's state to 'Unchanged' to cancel the deletion process.
			invoicesDbContext.Entry(entity).State = EntityState.Unchanged;
			// Re-throw the exception to notify the caller of the failure.
			throw;
        }
    }
}