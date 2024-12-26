using AutoMapper;
using Invoices.Api.Interfaces;
using Invoices.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Invoices.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class IdentificationController : ControllerBase
	{
		private readonly IIdentificationManager identificationManager;
		private readonly IMapper mapper;

		public IdentificationController (IIdentificationManager identificationManager, IMapper mapper)
		{
			this.identificationManager = identificationManager;
			this.mapper = mapper;
		}
		/// <summary>
		/// get all invoices by IdentificationNumber, where person is seller
		/// </summary>
		/// <param name="identificationNumber"></param>
		/// <returns>list of invoices as seller</returns>
		[HttpGet("{identificationNumber}/sales")]
		public IEnumerable<InvoiceDto> GetInvoicesBySellerIdentificationNumber(string identificationNumber)
		{
			return identificationManager
				.GetInvoicesBySellerIdentificationNumber(identificationNumber)
				.Select(invoice => mapper.Map<InvoiceDto>(invoice));    // map each invoice entity to its corresponding DTO.
		}
		/// <summary>
		/// get all invoices by IdentificationNumber, where person is buyer
		/// </summary>
		/// <param name="identificationNumber"></param>
		/// <returns>lits of invoices as buyer</returns>
		[HttpGet("{identificationNumber}/purchases")]
		public IEnumerable<InvoiceDto> GetInvoicesByBuyerIdentificationNumber(string identificationNumber)
		{
			return identificationManager
				.GetInvoicesByBuyerIdentificationNumber(identificationNumber)
				.Select(invoice => mapper.Map<InvoiceDto>(invoice));    // map each invoice entity to its corresponding DTO.
		}
	}
}
