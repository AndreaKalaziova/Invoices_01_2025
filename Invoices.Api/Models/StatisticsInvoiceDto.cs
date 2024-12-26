namespace Invoices.Api.Models
{
	/// <summary>
	/// DTO for Invoices Statistics, returns Current Year Sum, All time sum and all Invoice Count
	/// </summary>
	public class StatisticsInvoiceDto
	{
		/// <summary>
		/// the total revenue / prices of the current year
		/// </summary>
		public double CurrentYearSum { get; set; }
		/// <summary>
		/// the total revenue/prices of all time
		/// </summary>
		public double AllTimeSum { get; set; }
		/// <summary>
		/// count of all invoices in db
		/// </summary>
		public double InvoiceCount {  get; set; }

	}
}
