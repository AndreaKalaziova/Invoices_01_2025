using AutoMapper;
using Invoices.Api.Interfaces;
using Invoices.Api.Models;
using Invoices.Data.Interfaces;
using Invoices.Data.Models;

namespace Invoices.Api.Managers
{
	/// <summary>
	/// working with IdentificationNumber as criterium
	/// </summary>
	public class IdentificationManager : IIdentificationManager
	{
		private readonly IInvoiceRepository invoiceRepository;	//repository to manage db operations for Invoice entities
		private readonly IPersonRepository personRepository;	// repository to manage db operations for person entities
		private readonly IMapper mapper;						// automapper for mapping between entity and DTO objects


		public IdentificationManager(
			IInvoiceRepository invoiceRepository,
			IPersonRepository personRepository,
			IMapper mapper)
		{
			this.invoiceRepository = invoiceRepository;	// DI for Invoice repository
			this.personRepository = personRepository;	// DI for Person repository
			this.mapper = mapper;						// DI for Automapper
		}
		/// <summary>
		/// get all invoices in db
		/// </summary>
		/// <returns>List of invoices</returns>
		public IList<InvoiceDto> GetAllInvoices() 
		{
			//get all invoices
			IList<Invoice> invoices = invoiceRepository.GetAll();  
			// map teh Invoice entities to InvoiceDto objects and return the list of Invoices
			return mapper.Map<IList<InvoiceDto>>(invoices);
		}

		/// <summary>
		/// get all invoices based on provided IdentificationNumber of person
		/// </summary>
		/// <param name="identificationNumber"></param>
		/// <param name="isSeller">terary expression isSeller? true = seller : false = buyerr</param>
		/// <returns>list of invoices for selected Identification Number</returns>
		public IList<Invoice> GetAllInvoicesByIdentificationNumber(string identificationNumber, bool isSeller)
		{ 
			//per isSeller filter out invoices by IdenticicationNumber of the Person as seller or buyer
			IList<Invoice> invoices = invoiceRepository.GetAllInvoicesByIdentificationNumber(identificationNumber, isSeller);
			return mapper.Map<IList<Invoice>>(invoices);
		}

		/// <summary>
		/// get all invoices by IdentificationNumber, where person is seller
		/// </summary>
		/// <param name="indentificationNumber"></param>
		/// <returns>list of invoices as seller</returns>
		public IList<Invoice> GetInvoicesBySellerIdentificationNumber(string indentificationNumber)
		{
			//IList<Invoice> invoices = invoiceRepository.GetInvoicesBySellerIdentificationNumber(IdentificationNumber);  
			//return mapper.Map<IList<Invoice>>(invoices);
			//shorter code:
			return invoiceRepository.GetInvoicesBySellerIdentificationNumber(indentificationNumber);
		}

		/// <summary>
		/// get all invoices by IdentificationNumber, where person is buyer
		/// </summary>
		/// <param name="indentificationNumber"></param>
		/// <returns>lits of invoices as buyer</returns>
		public IList<Invoice> GetInvoicesByBuyerIdentificationNumber(string indentificationNumber)
		{
			IList<Invoice> invoices = invoiceRepository.GetInvoicesByBuyerIdentificationNumber(indentificationNumber);  
			return mapper.Map<IList<Invoice>>(invoices);
		}	
	}
}
