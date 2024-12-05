using Invoices.Api.Interfaces;
using Invoices.Api.Managers;
using Invoices.Api.Models;
using Invoices.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Invoices.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoicesController : ControllerBase
{
    private readonly IInvoiceManager invoiceManager;

    public InvoicesController(IInvoiceManager invoiceManager)
    {
        this.invoiceManager = invoiceManager;
    }

	[HttpPost]          // pridani nove faktury s id buyer/seller -> vrati ale jiz detailny vypis / OK
	public IActionResult AddInvoice([FromBody] InvoiceDto invoice)
	{
		InvoiceDto? createdInvoice = invoiceManager.AddInvoice(invoice);
		return StatusCode(StatusCodes.Status201Created, createdInvoice);
	}

	//api/invoice/1
	[HttpGet("{invoiceId}")]         //vypis detailni faktury dle id - OK
	public IActionResult GetInvoice(ulong invoiceId)
	{
		InvoiceDto? invoice = invoiceManager.GetInvoice(invoiceId);

		if (invoice is null)
		{
			return NotFound();
		}

		return Ok(invoice);
	}

	[HttpGet]	// vypsani faktur dle filtru
	public IEnumerable<InvoiceDto> GetInvoices([FromQuery] InvoiceFilterDto invoiceFilter)
	{
		return invoiceManager.GetAllInvoices(invoiceFilter);
	}
	
 //   [HttpGet]   
 //   public IEnumerable<InvoiceDto> GetInvoices()    // vypis vsech faktur, detailne -OK
	//{
 //       return invoiceManager.GetAllInvoices();
 //   }	

	[HttpPut("{invoiceId}")]
	public IActionResult UpdateInvoice(ulong invoiceId, [FromBody] InvoiceDto invoiceDto) // OK
	{
		InvoiceDto? updateInvoice = invoiceManager.UpdateInvoice(invoiceId, invoiceDto); 
		if (updateInvoice is null)
		{
			return NotFound();
		}

		return Ok(updateInvoice);
	}
	
    [HttpDelete("{invoiceId}")] // mazani faktury - OK
    public IActionResult DeleteInvoice(uint invoiceId)
    {
        invoiceManager.DeleteInvoice(invoiceId);
        return NoContent();
    }
	[HttpGet("statistics")]		// api/invoices/statistics
	public IActionResult GetStatisticsInvoice()
	{
		StatisticsInvoiceDto? statistics = invoiceManager.GetStatisticsInvoice();
		return Ok(statistics);
	}
}