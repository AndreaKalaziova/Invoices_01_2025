using Invoices.Data.Models;
using System.Text.Json.Serialization;

namespace Invoices.Api.Models
{
	public class StatisticsPersonDto
	{
		[JsonPropertyName("personId")]
		public ulong PersonId { get; set; }
		[JsonPropertyName("personName")]
		public string Name { get; set; } = "";

		public double Revenue { get; set; }
	}
}
