using Invoices.Data.Models;

namespace Invoices.Data.Interfaces
{
    public interface IInvoiceRepository : IBaseRepository<Invoice>
    {
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
		IList<Invoice> GetAll(

			ulong? buyerId = null,
			ulong? sellerId = null,
			string? product = null,
			int? minPrice = null,
			int? maxPrice = null,
			int? limit = null);

		/// <summary>
		/// finds invoice in db by its invoiceId
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Object Invoice if found, otherwise null</returns>
		Invoice? FindById(ulong id);

		/// <summary>
		/// get all invoices by person's ICO
		/// </summary>
		/// <param name="identificationNumber"> person's ICO</param>
		/// <param name="isSeller">terary expression isSeller? true = seller : false = buyer</param>
		/// <returns>List of invoices by ICO </returns>
		IList<Invoice> GetAllInvoicesByIdentificationNumber(string identificationNumber, bool isSeller);
		/// <summary>
		/// get all issued invoices by person's ICO (as seller)  
		/// </summary>
		/// <param name="identificationNumber">person ICO</param>
		/// <returns>List of invoices issued by the specified person</returns>
		IList<Invoice> GetInvoicesBySellerIdentificationNumber(string indentificationNumber);
		/// <summary>
		/// get all invoices recieved by person's ICO (as buyer)
		/// </summary>
		/// <param name="indentificationNumber">person's ICO</param>
		/// <returns>List of invoices recieved by specified person</returns>
		IList<Invoice> GetInvoicesByBuyerIdentificationNumber(string indentificationNumber);
		/// <summary>
		/// calculates the total revenue / prices of the current year
		/// </summary>
		/// <returns>total revenue for this year</returns>
		double GetCurrentYearSum();
		/// <summary>
		/// calculates the total revenue/prices of all time
		/// </summary>
		/// <returns>total revenue of all the invoices</returns>
		double GetAllTimeSum();
		/// <summary>
		/// counts number of invoices in db
		/// </summary>
		/// <returns></returns>
		double GetInvoiceCount();
	}
}