using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Data.Models
{
    public class Invoice
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong InvoiceId {  get; set; }
        [Required]
        public ulong InvoiceNumber { get; set; }


        [Required]
        [ForeignKey(nameof(Seller))]
        public ulong? SellerId { get; set; }
        public virtual Person? Seller { get; set; }

		[Required]
        [ForeignKey(nameof(Buyer))]
        public ulong? BuyerId { get; set; }
		public virtual Person? Buyer { get; set; }

		[Required]
        public DateTime Issued { get; set; }        // DateOnly nepouzivat
        [Required]
        public DateTime DueDate { get; set; }
        [Required]
        public string Product { get; set; } = "";
        [Required]
        public double Price { get; set; }
        [Required]
        public int Vat { get; set; }
        public string Note { get; set; } = "";
    }
}
