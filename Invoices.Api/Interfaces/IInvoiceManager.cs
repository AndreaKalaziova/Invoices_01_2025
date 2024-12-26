using Invoices.Api.Models;

namespace Invoices.Api.Interfaces
{
    public interface IInvoiceManager
    {
		/// <summary>
		/// add new invoices to db
		/// </summary>
		/// <param name="invoiceDto">invoice data to be added</param>
		/// <returns>nwly created invoice with its details as invoiceDto</returns>
		InvoiceDto AddInvoice(InvoiceDto invoiceDto);
		/// <summary>
		/// get all invoices filtered if filters used, or get all invoices without filtering
		/// </summary>
		/// <param name="invoiceFilterDto">filters used, if any</param>
		/// <returns>list of invoices, filter or witout filters</returns>
		InvoiceDto? GetInvoice(ulong invoiceId);
		/// <summary>
		/// get invoice with its details per Id
		/// </summary>
		/// <param name="invoiceId">id of the invoice</param>
		/// <returns>the requested invoice</returns>
		IList<InvoiceDto> GetAllInvoices(InvoiceFilterDto? invoiceFilterDto = null);
		/// <summary>
		/// updates the requested invoice and returns updated version
		/// </summary>
		/// <param name="invoiceId">id of th einvoice that needs to be updated</param>
		/// <param name="invoiceDto">the updated details of the selected invoice</param>
		/// <returns>updated invoice as InvoiceDto or null of not found</returns>
		InvoiceDto? UpdateInvoice(ulong invoiceId, InvoiceDto invoiceDto);
		/// <summary>
		/// delete the invoice per its Id form db
		/// </summary>
		/// <param name="invoiceId"></param>
		InvoiceDto? DeleteInvoice(uint invoiceId);
		/// <summary>
		/// get statistics for invoices, creates new object for invoices statistics per Dto
		/// </summary>
		/// <returns>Currect Year total Sum, All time Sum, count of all invoices</returns>
		StatisticsInvoiceDto GetStatisticsInvoice();
	}
}
