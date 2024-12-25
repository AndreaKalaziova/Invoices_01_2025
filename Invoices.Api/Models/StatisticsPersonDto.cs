using System.Text.Json.Serialization;

namespace Invoices.Api.Models
{
	/// <summary>
	/// DTO for Person's statistics, returns Person's Id - name - Revenue - PreviousYearTurnover - Profit
	/// </summary>
	public class StatisticsPersonDto
	{
		/// <summary>
		/// Person's unique id with JSON property name
		/// </summary>
		[JsonPropertyName("personId")]
		public ulong PersonId { get; set; }
		[JsonPropertyName("personName")]
		public string Name { get; set; } = "";
		/// <summary>
		/// revenue/income from all incoming invoices (person.InvoicesAsSeller.Sum(invoice => invoice.Price))
		/// </summary>
		public double Revenue { get; set; }
		/// <summary>
		/// total income in last year (.Sum(invoice => invoice.Price) + .Sum(invoice => invoice.Price))
		/// </summary>
		public double PreviousYearTurnover { get; set; }
		/// <summary>
		/// gross profit (person.InvoicesAsSeller.Sum(invoice => invoice.Price) - person.InvoicesAsBuyer.Sum(invoice => invoice.Price))
		/// </summary>
		public double Profit { get; set; }
	}
}
