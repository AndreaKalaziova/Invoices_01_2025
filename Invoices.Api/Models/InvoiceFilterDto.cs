
namespace Invoices.Api.Models
{
	/// <summary>
	/// DTO for invoice filets
	/// </summary>
	public class InvoiceFilterDto
	{
		/// <summary>
		/// for invoices with sellected buyer
		/// map seller and buyerId
		/// </summary>
		public ulong? BuyerId {  get; set; }
		/// <summary>
		/// for invoices with sellected seller
		/// map seller and sellerId
		/// </summary>
		public ulong? SellerId { get; set; }
		/// <summary>
		/// for invoices with specified product
		/// </summary>
		public string? Product { get; set; }
		/// <summary>
		/// for invoices with Price equal or higher then specified
		/// </summary>
		public int? MinPrice { get; set; }
		/// <summary>
		/// for invoices with Price equal or lower then specified
		/// </summary>
		public int? MaxPrice { get; set; }
		/// <summary>
		/// limit od return foundings
		/// </summary>
		public int? Limit { get; set; }
	}
}
