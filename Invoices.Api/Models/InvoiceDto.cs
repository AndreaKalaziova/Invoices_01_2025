using Invoices.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Invoices.Api.Models
{
    public class InvoiceDto
    {
        public ulong InvoiceNumber { get; set; }

        [JsonPropertyName("seller")]
        public PersonDto? Seller { get; set; } 

        [JsonPropertyName("buyer")]
		public PersonDto? Buyer { get; set; } 

		public DateTime Issued { get; set; }
        public DateTime DueDate { get; set; }
        public string Product { get; set; } = "";
        public decimal Price { get; set; }
        public int Vat { get; set; }
        public string Note { get; set; } = "";
        [JsonPropertyName("_id")]
        public ulong InvoiceId { get; set; }

    }
}

