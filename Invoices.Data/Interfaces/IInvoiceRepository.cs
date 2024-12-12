using Invoices.Data.Interfaces;
using Invoices.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Data.Interfaces
{
    public interface IInvoiceRepository : IBaseRepository<Invoice>
    {
        IList<Invoice> GetAll(

			ulong? buyerId = null,
			ulong? sellerId = null,
			string? product = null,
			int? minPrice = null,
			int? maxPrice = null,
			int? limit = null);

        Invoice? FindById(ulong id);

        IList<Invoice> GetAllInvoicesByIdentificationNumber(string identificationNumber, bool isSeller);
		IList<Invoice> GetInvoicesBySellerIdentificationNumber(string indentificationNumber);
		IList<Invoice> GetInvoicesByBuyerIdentificationNumber(string indentificationNumber);
        
        double GetCurrentYearSum();
        double GetAllTimeSum();
		double GetInvoiceCount();
	}
}