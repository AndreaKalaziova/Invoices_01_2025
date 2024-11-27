using Invoices.Data.Models;
using System.Text.Json.Serialization;

namespace Invoices.Api.Models
{
	public class StatisticsPersonDto
	{
		[JsonPropertyName("personId")]
		public ulong PersonId { get; set; }

		public string Name { get; set; } = "";

		public decimal Revenue { get; set; }

		// seller - id & name +
		// revenue - z vypoctu od invoice decimal price
		//prehodit vse do person protoze api/persons/statistics
	}
}
