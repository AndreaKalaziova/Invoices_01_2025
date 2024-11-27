using Invoices.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Invoices.Api.Interfaces
{
	public interface IIdentificationManager
	{
		IList<Invoice> GetAllInvoicesByIdentificationNumber(string identificationNumber, bool isSeller);
		IList<Invoice> GetInvoicesBySellerIdentificationNumber(string indentificationNumber);
		IList<Invoice> GetInvoicesByBuyerIdentificationNumber(string indentificationNumber);

	}
}
