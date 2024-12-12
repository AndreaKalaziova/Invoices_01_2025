using Microsoft.AspNetCore.Mvc;

namespace Invoices.Api.Models
{
	public class InvoiceFilterDto
	{// Mapování "buyer" na "BuyerId"
									//buyerId - vybere faktury s danym buyerem
		public ulong? BuyerId {  get; set; }
 // Mapování "seller" na "SellerId"
									 //selledId - vybere faktury s danym sellerem
		public ulong? SellerId { get; set; }
		//product - ziska faktury, ktere obsahuji tento produkt
		public string? Product { get; set; }
		//minprice - vybere faktury, s castkou vyssi nebo rovnou 
		public int? MinPrice { get; set; }
		//maxPrice - faktury, s castkou nizsi nebo rovnoou
		public int? MaxPrice { get; set; }
		//limit - limit vyctu 
		public int? Limit { get; set; }
	}
}
