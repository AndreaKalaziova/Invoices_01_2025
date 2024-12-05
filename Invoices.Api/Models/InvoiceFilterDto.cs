namespace Invoices.Api.Models
{
	public class InvoiceFilterDto
	{
		//buyerId - vybere faktury s danym buyerem
		public ulong? BuyerId {  get; set; }
		//selledId - vybere faktury s danym sellerem
		public ulong? SellerId { get; set; }
		//product - ziska faktury, ktere obsahuji tento produkt
		public string? Product { get; set; }
		//minprice - vybere faktury, s castkou vyssi nebo rovnou 
		public int? MinPrice { get; set; }
		//maxPrice - faktury, s castkou nizsi nebo rovnoou
		public int? MaxPrrice { get; set; }
		//limit - limit vyctu 
		public int? Limit { get; set; }
	}
}
