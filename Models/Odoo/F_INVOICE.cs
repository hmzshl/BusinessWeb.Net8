using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessWeb.Models.Odoo
{
	public class F_INVOICE
	{
		public int Id { get; set; }
		public string Number { get; set; } // Invoice number
		public int CustomerId { get; set; } // Related customer
		public decimal AmountTotal { get; set; } // Total amount
		public decimal AmountTax { get; set; } // Tax amount
		public decimal AmountUntaxed { get; set; } // Untaxed amount
		public DateTime InvoiceDate { get; set; }
		public string Status { get; set; } // Status (paid, unpaid, draft)
		public string Currency { get; set; } // Currency used
		public string PaymentReference { get; set; } // Payment reference
		public DateTime DueDate { get; set; } // Due date for payment
		public string Notes { get; set; } // Additional notes
	}
}
