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

		// vypis vsech vystavenych faktur, dle seller ICO
		[HttpGet("{identificationNumber}/sales")]
		public IEnumerable<InvoiceDto> GetInvoicesBySellerIdentificationNumber(string identificationNumber)
		{
			return identificationManager
				.GetInvoicesBySellerIdentificationNumber(identificationNumber)
				.Select(invoice => mapper.Map<InvoiceDto>(invoice));
		}

		// vypis vsech prijatych faktur, dle buyer ICO
		[HttpGet("{identificationNumber}/purchases")]
		public IEnumerable<InvoiceDto> GetInvoicesByBuyerIdentificationNumber(string identificationNumber)
		{
			return identificationManager
				.GetInvoicesByBuyerIdentificationNumber(identificationNumber)
				.Select(invoice => mapper.Map<InvoiceDto>(invoice));
		}
	}
}
