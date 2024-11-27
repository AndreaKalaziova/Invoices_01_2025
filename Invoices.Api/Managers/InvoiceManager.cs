using Invoices.Api.Models;
using Invoices.Data.Interfaces;
using Invoices.Data.Models;
using System.Linq;
using AutoMapper;
using Invoices.Api.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Identity.Client;

namespace Invoices.Api.Managers;

    public class InvoiceManager : IInvoiceManager
    {
        private readonly IInvoiceRepository invoiceRepository;
        private readonly IPersonRepository personRepository;
        private readonly IMapper mapper;


        public InvoiceManager(
            IInvoiceRepository invoiceRepository, 
            IPersonRepository personRepository,
            IMapper mapper)
        {
            this.invoiceRepository = invoiceRepository;
            this.personRepository = personRepository;
            this.mapper = mapper;
        }

	public InvoiceDto? AddInvoice(InvoiceDto invoiceDto) // OK
	{
		var seller = personRepository.FindById(invoiceDto.Seller.PersonId);
		var buyer = personRepository.FindById(invoiceDto.Buyer.PersonId);

		if (seller is null || buyer is null)
			return null;

		Invoice invoice = mapper.Map<Invoice>(invoiceDto);

		invoice.Seller = seller;
		invoice.Buyer = buyer;

		invoice.InvoiceId = default;
		Invoice? addedInvoice = invoiceRepository.Insert(invoice);
		
		if (addedInvoice is null)
		{
			return null;
		}
		
		addedInvoice = invoiceRepository.FindById(addedInvoice.InvoiceId);
		
		return mapper.Map<InvoiceDto>(addedInvoice);
	}


	public InvoiceDto? GetInvoice(ulong invoiceId)       // pro vypis jedne faktury dle Id - OK
	{
        Invoice? invoice = invoiceRepository.FindById(invoiceId);
		
		if (invoice is null)
		{
			return null;
		}

		return mapper.Map<InvoiceDto>(invoice);
	}
	
    public IList<InvoiceDto> GetAllInvoices() // vypis vsech faktur v db OK
    {
        IList<Invoice> invoices = invoiceRepository.GetAllInvoices(); 
		return mapper.Map<IList<InvoiceDto>>(invoices);
    }

	public InvoiceDto? UpdateInvoice(ulong invoiceId, InvoiceDto invoiceDto) // OK
	{
		Invoice? invoice = invoiceRepository.FindById(invoiceId);

		if (invoice is null)
			return null;

		var seller = personRepository.FindById(invoiceDto.Seller.PersonId);
		var buyer = personRepository.FindById(invoiceDto.Buyer.PersonId);

		if (seller is null || buyer is null)
			return null;

		mapper.Map<InvoiceDto, Invoice>(invoiceDto, invoice);
		invoice.InvoiceId = invoiceId;
		invoice.Seller = seller;
		invoice.Buyer = buyer;

		Invoice updatedInvoice = invoiceRepository.Update(invoice);
		if (updatedInvoice is null)
		{
			return null;
		}

		return mapper.Map<InvoiceDto>(updatedInvoice);
	}

	public InvoiceDto? DeleteInvoice(uint invoiceId)	// OK
	{
		if (!invoiceRepository.ExistsWithId(invoiceId))
			return null;

		Invoice invoice = invoiceRepository.FindById(invoiceId)!;
		InvoiceDto invoiceDto = mapper.Map<InvoiceDto>(invoice);

		invoiceRepository.Delete(invoiceId);

		return invoiceDto;
	}

	public StatisticsInvoiceDto GetStatisticsInvoice()		// vytvoreni noveho objektu pro Sttaistiky Invoice pro vypis dle Dto
	{
		//double currentYearSum = invoiceRepository.GetCurrentYearSum();
		//double allTimeSum = invoiceRepository.GetAllTimeSum();		
		//double invoiceCount = invoiceRepository.GetInvoiceCount();

		return new StatisticsInvoiceDto
		{
			CurrentYearSum = invoiceRepository.GetCurrentYearSum(),     //soucet cen za letosni rok currentYearSum
			AllTimeSum = invoiceRepository.GetAllTimeSum(),             //soucet cen za vsechny roky AllTimeSum
			InvoiceCount = invoiceRepository.GetInvoiceCount()          //pocet faktur v databazi 
		};
	}

}





