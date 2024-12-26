using Invoices.Data.Models;

namespace Invoices.Api.Interfaces
{
	public interface IIdentificationManager
	{
		/// <summary>
		/// get all invoices based on provided IdentificationNumber of person
		/// </summary>
		/// <param name="identificationNumber"></param>
		/// <param name="isSeller">terary expression isSeller? true = seller : false = buyerr</param>
		/// <returns>list of invoices for selected Identification Number</returns>
		IList<Invoice> GetAllInvoicesByIdentificationNumber(string identificationNumber, bool isSeller);
		/// <summary>
		/// get all invoices by IdentificationNumber, where person is seller
		/// </summary>
		/// <param name="indentificationNumber"></param>
		/// <returns>list of invoices as seller</returns>
		IList<Invoice> GetInvoicesBySellerIdentificationNumber(string indentificationNumber);
		/// <summary>
		/// get all invoices by IdentificationNumber, where person is buyer
		/// </summary>
		/// <param name="indentificationNumber"></param>
		/// <returns>lits of invoices as buyer</returns>
		IList<Invoice> GetInvoicesByBuyerIdentificationNumber(string indentificationNumber);

	}
}
