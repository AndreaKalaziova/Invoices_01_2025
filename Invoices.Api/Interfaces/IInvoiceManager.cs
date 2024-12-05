using Invoices.Api.Models;
using Invoices.Data.Models;
using Invoices.Data.Repositories;

namespace Invoices.Api.Interfaces
{
    public interface IInvoiceManager
    {
        InvoiceDto AddInvoice(InvoiceDto invoiceDto);

        InvoiceDto? GetInvoice(ulong invoiceId);

        //IList<InvoiceDto> GetAllInvoices();
 
        IList<InvoiceDto> GetAllInvoices(InvoiceFilterDto? invoiceFilterDto = null);


		InvoiceDto? UpdateInvoice(ulong invoiceId, InvoiceDto invoiceDto);

        InvoiceDto? DeleteInvoice(uint invoiceId);

        StatisticsInvoiceDto GetStatisticsInvoice();

	}
}
