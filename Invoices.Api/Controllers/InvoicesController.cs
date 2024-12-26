using Invoices.Api.Interfaces;
using Invoices.Api.Models;
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

	/// <summary>
	/// add new invoices to db, input with sellerId/buyerId -> return their details
	/// </summary>
	/// <param name="invoiceDto">invoice data to be added</param>
	/// <returns>nwly created invoice with its details as invoiceDto</returns>
	[HttpPost]          
	public IActionResult AddInvoice([FromBody] InvoiceDto invoice)
	{
		//check if PersonDto inserted as per reqirements
		if (!ModelState.IsValid)
			return BadRequest(ModelState);
		try
		{
			// Attempt to add the invoice
			InvoiceDto? createdInvoice = invoiceManager.AddInvoice(invoice);
			return StatusCode(StatusCodes.Status201Created, createdInvoice);
		}
		catch (InvalidOperationException ex)
		{
			// Return a conflict response if InvoiceNumber already exists
			return Conflict(new { message = ex.Message });
		}
	}
	/// <summary>
	/// get invoice with its details per Id
	/// </summary>
	/// <param name="invoiceId">id of the invoice</param>
	/// <returns>the requested invoice</returns>
	//api/invoice/1
	[HttpGet("{invoiceId}")]       
	public IActionResult GetInvoice(ulong invoiceId)
	{
		if (invoiceId <= 0)
			return BadRequest("InvoiceId musí být větší než 0");

		InvoiceDto? invoice = invoiceManager.GetInvoice(invoiceId);

		//check if invoice exists, if not return Not Found
		if (invoice is null)
			return NotFound();
		//if invoice found return it 
		return Ok(invoice);
	}

	/// <summary>
	/// get all invoices filtered if filters used, or get all invoices without filtering
	/// </summary>
	/// <param name="invoiceFilterDto">filters used, if any</param>
	/// <returns>list of invoices, filter or witout filters</returns>
	[HttpGet]	
	public IEnumerable<InvoiceDto> GetInvoices([FromQuery] InvoiceFilterDto invoiceFilter)
	{
		//returns list of invoices as per filters used
		return invoiceManager.GetAllInvoices(invoiceFilter);
	}

	/// <summary>
	/// updates the requested invoice and returns updated version
	/// </summary>
	/// <param name="invoiceId">id of th einvoice that needs to be updated</param>
	/// <param name="invoiceDto">the updated details of the selected invoice</param>
	/// <returns>updated invoice as InvoiceDto or null of not found</returns>
	[HttpPut("{invoiceId}")]
	public IActionResult UpdateInvoice(ulong invoiceId, [FromBody] InvoiceDto invoiceDto)
	{
		//check input requiremnts of DTO
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		//check if invoiceId is valid positiv enumber
		if (invoiceId <= 0)
			return BadRequest("InvoiceId musí být větší než 0");

		try
		{
			InvoiceDto? updateInvoice = invoiceManager.UpdateInvoice(invoiceId, invoiceDto);
			if (updateInvoice is null)
				return NotFound();
			return StatusCode(StatusCodes.Status200OK, updateInvoice);
		}
		catch (InvalidOperationException ex)
		{
			return BadRequest(ex.Message); // 400 bad request with error message
		}
		catch (Exception ex)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); // handle unexpected errors
		}
	}
	/// <summary>
	/// delete the invoice per its Id form db
	/// </summary>
	/// <param name="invoiceId"></param>
	[HttpDelete("{invoiceId}")]  
    public IActionResult DeleteInvoice(uint invoiceId)
    {
		//check if invoiceId is valid positiv enumber
		if (invoiceId <= 0)
			return BadRequest("InvoiceId musí být větší než 0");

		try
		{
			invoiceManager.DeleteInvoice(invoiceId);
			return NoContent();
		}
		catch (Exception ex)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); // handle unexpected errors
		}
    }
	/// <summary>
	/// get statistics for invoices, creates new object for invoices statistics per Dto
	/// </summary>
	/// <returns>Currect Year total Sum, All time Sum, count of all invoices</returns>
	[HttpGet("statistics")]		// api/invoices/statistics
	public IActionResult GetStatisticsInvoice()
	{
		StatisticsInvoiceDto? statistics = invoiceManager.GetStatisticsInvoice();
		return Ok(statistics);
	}
}