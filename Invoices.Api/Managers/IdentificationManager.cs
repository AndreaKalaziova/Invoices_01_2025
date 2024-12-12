using AutoMapper;
using Invoices.Api.Interfaces;
using Invoices.Api.Models;
using Invoices.Data.Interfaces;
using Invoices.Data.Models;

namespace Invoices.Api.Managers
{
	public class IdentificationManager : IIdentificationManager
	{
		private readonly IInvoiceRepository invoiceRepository;
		private readonly IPersonRepository personRepository;
		private readonly IMapper mapper;


		public IdentificationManager(
			IInvoiceRepository invoiceRepository,
			IPersonRepository personRepository,
			IMapper mapper)
		{
			this.invoiceRepository = invoiceRepository;
			this.personRepository = personRepository;
			this.mapper = mapper;
		}


		public IList<InvoiceDto> GetAllInvoices() // OK
		{
			IList<Invoice> invoices = invoiceRepository.GetAll();  
			return mapper.Map<IList<InvoiceDto>>(invoices);
		}

		// vypis vsech faktur dle ICO
		public IList<Invoice> GetAllInvoicesByIdentificationNumber(string identificationNumber, bool isSeller)
		{ 
			IList<Invoice> invoices = invoiceRepository.GetAllInvoicesByIdentificationNumber(identificationNumber, isSeller);
			return mapper.Map<IList<Invoice>>(invoices);
		}

		// vypis vsech vystavenych faktur, dle seller ICO
		public IList<Invoice> GetInvoicesBySellerIdentificationNumber(string indentificationNumber)
		{
			//IList<Invoice> invoices = invoiceRepository.GetInvoicesBySellerIdentificationNumber(IdentificationNumber);  
			//return mapper.Map<IList<Invoice>>(invoices);
			//nize zkraceny zapis:
			return invoiceRepository.GetInvoicesBySellerIdentificationNumber(indentificationNumber);
		}

		//vypis vsech prijatuch faktur, dle buyer ICO
		public IList<Invoice> GetInvoicesByBuyerIdentificationNumber(string indentificationNumber)
		{
			IList<Invoice> invoices = invoiceRepository.GetInvoicesByBuyerIdentificationNumber(indentificationNumber);  
			return mapper.Map<IList<Invoice>>(invoices);
		}

		
	}
}
