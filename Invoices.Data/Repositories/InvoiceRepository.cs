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
/// repository class for managing Invoice entities in db; inherits basic CRID operations from BaseRepository
/// </summary>
public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository
{
    /// <summary>
    /// constructor that inicializes the repository with the db context
    /// </summary>
    /// <param name="invoicesDbContext"> The db contaxt used to access the invoice</param>
    public InvoiceRepository(InvoicesDbContext invoicesDbContext) : base(invoicesDbContext)
    {
    }
	/// <summary>
	/// return all invoice if no filter applied, or filtered by filters 
	/// </summary>
	/// <param name="buyerId">filters out invoices for this buyer (nullable)</param>
	/// <param name="sellerId">filters out invoices from this seller (nullable)</param>
	/// <param name="product">filters out invoices wit this product (nullable)</param>
	/// <param name="minPrice">filtres out invoices with this Price or higher (nullable)</param>
	/// <param name="maxPrice">filters out invoices with this Price or lower (nullable)</param>
	/// <param name="limit">limit for returned invoices (nullable)</param>
	/// <returns>List of (filtered) invoices</returns>
	public IList<Invoice> GetAll(

        //invoices for this buyer (nullable)
        ulong? buyerId = null,
		//invoices from this seller (nullable)
		ulong? sellerId = null,
		//invoices wit this product (nullable)
		string? product = null,
		//invoices with this Price or higher (nullable)
		int? minPrice = null,
		//invoices with this Price or lower (nullable)
		int? maxPrice = null,
		//limit of count of returned invoices (nullable)
		int? limit = null)
    {
        IQueryable<Invoice> query = dbSet.AsQueryable();
		// apply buyer filter if specified
		if (buyerId is not null)
            query = query.Where(i =>  i.BuyerId == buyerId);
		// apply seller filter if specified
		if (sellerId is not null)
            query = query.Where(i => i.SellerId == sellerId);
		// apply product filter if specified
		if (!string.IsNullOrEmpty(product))
			query = query.Where(i => i.Product.Contains(product));
		// apply minimal Price filter if specified
		if (minPrice is not null)
            query = query.Where(i => i.Price >=  minPrice.Value);
		// apply maximal Price filter if specified
		if (maxPrice is not null)
            query = query.Where(i => i.Price <= maxPrice.Value);
		// apply limit filter if specified
		if (limit is not null && limit > 0)
            query = query.Take(limit.Value);

        Console.WriteLine();
        return query.ToList();
    }
    /// <summary>
    /// finds invoice in db by its invoiceId
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Object Invoice if found, otherwise null</returns>
    public Invoice? FindById(ulong id)
    {
        //query to db for the invoice with Id, includes related entities Seller/Buyer in the result
        return dbSet
            .Include(i => i.Seller)     // include Seller entity
            .Include(i => i.Buyer)      // include Buyer entity
            .FirstOrDefault(i => i.InvoiceId == id); // find the first match or return null if not found
	}
	/// <summary>
	/// get all invoices by person's ICO
	/// </summary>
	/// <param name="identificationNumber"> person's ICO</param>
	/// <param name="isSeller">terary expression isSeller? true = seller : false = buyer</param>
	/// <returns>List of invoices by ICO </returns>
	public IList<Invoice> GetAllInvoicesByIdentificationNumber(string identificationNumber, bool isSeller)
    {
        //query to db for invoices by provided ICO (IdentificationNumber)
        return dbSet
            .Include(i => i.Seller)
            .Include(i => i.Buyer)
            .Where(i => isSeller 
            ? i.Seller.IdentificationNumber == identificationNumber //filter by seller's ICO
            : i.Buyer.IdentificationNumber == identificationNumber) //filter by buyer's ICO
			.ToList(); // return result as List
    }
	/// <summary>
	/// get all issued invoices by person's ICO (as seller)  
	/// </summary>
	/// <param name="identificationNumber">person ICO</param>
	/// <returns>List of invoices issued by the specified person</returns>
	public IList<Invoice> GetInvoicesBySellerIdentificationNumber(string identificationNumber)
    {
        return GetAllInvoicesByIdentificationNumber(identificationNumber, true);
    }
    /// <summary>
    /// get all invoices recieved by person's ICO (as buyer)
    /// </summary>
    /// <param name="indentificationNumber">person's ICO</param>
    /// <returns>List of invoices recieved by specified person</returns>
    public IList<Invoice> GetInvoicesByBuyerIdentificationNumber(string indentificationNumber)
    {
        return GetAllInvoicesByIdentificationNumber(indentificationNumber, false);
    }

    /// <summary>
    /// calculates the total revenue / prices of the current year
    /// </summary>
    /// <returns>total revenue for this year</returns>
    public double GetCurrentYearSum()
    {
        return dbSet
            .Where(i => i.InvoiceId != null && i.Issued.Year == DateTime.Now.Year)
            .Sum(i => i.Price); // (i => i.Price ?? 0) v pripade, ze je nula nahradi null hodnotu 0    
    }
    /// <summary>
    /// calculates the total revenue/prices of all time
    /// </summary>
    /// <returns>total revenue of all the invoices</returns>
    public double GetAllTimeSum()
    {
        return dbSet
            .Where(i => i.InvoiceId != null)
            .Sum(i => i.Price);
    }
    /// <summary>
    /// counts number of invoices in db
    /// </summary>
    /// <returns></returns>
    public double GetInvoiceCount()
    {
        return dbSet.Count(i => i.InvoiceId != null);
    }
}