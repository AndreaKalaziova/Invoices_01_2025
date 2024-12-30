using Invoices.Api.Models;
using Invoices.Data.Interfaces;
using Invoices.Data.Models;
using AutoMapper;
using Invoices.Api.Interfaces;

namespace Invoices.Api.Managers;

    public class InvoiceManager : IInvoiceManager
    {
        private readonly IInvoiceRepository invoiceRepository;	// repository to manage db operations for Invoice entities
        private readonly IPersonRepository personRepository;	// repository to manage db operations for Person entities
        private readonly IMapper mapper;						// automapper for mapping between entity and Dto objects

        public InvoiceManager(
            IInvoiceRepository invoiceRepository, 
            IPersonRepository personRepository,
            IMapper mapper)
        {
            this.invoiceRepository = invoiceRepository;		// DI for Invoice repository
            this.personRepository = personRepository;		// DI for Person repository
            this.mapper = mapper;							// DI for AutoMapper
        }
	/// <summary>
	/// add new invoices to db
	/// </summary>
	/// <param name="invoiceDto">invoice data to be added</param>
	/// <returns>nwly created invoice with its details as invoiceDto</returns>
	public InvoiceDto? AddInvoice(InvoiceDto invoiceDto)
	{
		// Check if an invoice with the same InvoiceNumber already exists
		if (invoiceRepository.ExistsWithInvoiceNumber(invoiceDto.InvoiceNumber))
			throw new InvalidOperationException($"Čislo faktury {invoiceDto.InvoiceNumber} je již použito.");
		
		//find the seller / buyer entities by its Id
		var seller = personRepository.FindById(invoiceDto.Seller.PersonId);
		var buyer = personRepository.FindById(invoiceDto.Buyer.PersonId);

		// If either Seller or Buyer is not found, return null
		if (seller is null)
			throw new InvalidOperationException($"Seller with ID {invoiceDto.Seller.PersonId} not found.");
		if (buyer is null)
			throw new InvalidOperationException($"Buyer with ID {invoiceDto.Buyer.PersonId} not found.");

		//map the provided dto to a new invoice entity
		Invoice invoice = mapper.Map<Invoice>(invoiceDto);

		// assign the Seller and Buyer entities to the Invoice
		invoice.Seller = seller;
		invoice.Buyer = buyer;

		//set the new invoiceId to default
		invoice.InvoiceId = default;
		//insert new invoice to the repository
		Invoice? addedInvoice = invoiceRepository.Insert(invoice);

		//f the insert operation fails, return null
		if (addedInvoice is null)
			return null;
		
		//retrieve the newly added invoice from the repository
		addedInvoice = invoiceRepository.FindById(addedInvoice.InvoiceId);
		//map the newly added invoice entity to InvoiceDto and return it
		return mapper.Map<InvoiceDto>(addedInvoice);
	}
	/// <summary>
	/// get all invoices filtered if filters used, or get all invoices without filtering
	/// </summary>
	/// <param name="invoiceFilterDto">filters used, if any</param>
	/// <returns>list of invoices, filter or witout filters</returns>
	public IList<InvoiceDto> GetAllInvoices(InvoiceFilterDto? invoiceFilterDto)
	{
		//get all invoices with used fillters, or if there i sno filter used, get all invoices
		IList<Invoice> invoices =
				invoiceRepository.GetAll(
					invoiceFilterDto.BuyerId,
					invoiceFilterDto.SellerId,
					invoiceFilterDto.Product,
					invoiceFilterDto.MinPrice,
					invoiceFilterDto.MaxPrice,
					invoiceFilterDto.Limit);

		//returns list of invoices as per filters used
		return mapper.Map<IList<InvoiceDto>>(invoices);
	}
	/// <summary>
	/// get invoice with its details per Id
	/// </summary>
	/// <param name="invoiceId">id of the invoice</param>
	/// <returns>the requested invoice</returns>
	public InvoiceDto? GetInvoice(ulong invoiceId)      
	{
		//find the requested invoice by its Id
		Invoice? invoice = invoiceRepository.FindById(invoiceId);
		//if the invoice does not exist, return null 
		if (invoice is null)
			return null;
		//return the requested invoice as invoiceDto
		return mapper.Map<InvoiceDto>(invoice);
	}
	/// <summary>
	/// updates the requested invoice and returns updated version
	/// </summary>
	/// <param name="invoiceId">id of th einvoice that needs to be updated</param>
	/// <param name="invoiceDto">the updated details of the selected invoice</param>
	/// <returns>updated invoice as InvoiceDto or null of not found</returns>
	public InvoiceDto? UpdateInvoice(ulong invoiceId, InvoiceDto invoiceDto)
	{
		//find the requested invoice by its Id
		Invoice? invoice = invoiceRepository.FindById(invoiceId);
		//if the invoice does not exist, return null 
		if (invoice is null)
			return null;

		//trigers seller/buyer entity based on their Id
		var seller = personRepository.FindById(invoiceDto.Seller.PersonId);
		var buyer = personRepository.FindById(invoiceDto.Buyer.PersonId);

		//if seller or buyer are not found return null
		if (seller is null || buyer is null)
			return null;

		//check if the invoice number has changed - if did not, omits checking if it already exists
		if (invoice.InvoiceNumber != invoiceDto.InvoiceNumber)
		{
			// if inv.Number was edited then check if an invoice with the same InvoiceNumber already exists
			if (invoiceRepository.ExistsWithInvoiceNumber(invoiceDto.InvoiceNumber))
				throw new InvalidOperationException($"Čislo faktury {invoiceDto.InvoiceNumber} je již použito.");
		}
		//map new date from Dto to the existing invoice entity
		mapper.Map<InvoiceDto, Invoice>(invoiceDto, invoice);
		invoice.InvoiceId = invoiceId;			// ensure the correct invoice id is maintained
		invoice.Seller = seller;				// assign seller/buyer entities to the invoice
		invoice.Buyer = buyer;

		//update the invoice in repository
		Invoice updatedInvoice = invoiceRepository.Update(invoice);
		// if the repository update fails, return null
		if (updatedInvoice is null)
			return null;
		//return the updated invoice as InvoiceDto
		return mapper.Map<InvoiceDto>(updatedInvoice);
	}
	/// <summary>
	/// delete the invoice per its Id form db
	/// </summary>
	/// <param name="invoiceId"></param>
	public InvoiceDto? DeleteInvoice(uint invoiceId)
	{
		//find the requested invoice by its Id
		Invoice invoice = invoiceRepository.FindById(invoiceId);
		InvoiceDto invoiceDto = mapper.Map<InvoiceDto>(invoice);

		//if the invoice does not exist, return null 
		if (!invoiceRepository.ExistsWithId(invoiceId))
			return null;

		//delete the found invoice from db
		invoiceRepository.Delete(invoiceId);

		return invoiceDto;
	}
	/// <summary>
	/// get statistics for invoices, creates new object for invoices statistics per Dto
	/// </summary>
	/// <returns>Currect Year total Sum, All time Sum, count of all invoices</returns>
	public StatisticsInvoiceDto GetStatisticsInvoice()			{
		return new StatisticsInvoiceDto
		{
			CurrentYearSum = invoiceRepository.GetCurrentYearSum(),     //the total revenue / prices of the current year
			AllTimeSum = invoiceRepository.GetAllTimeSum(),             //the total revenue / prices of all time
			InvoiceCount = invoiceRepository.GetInvoiceCount()          //counts number of invoices in db
		}; 
	}
}





