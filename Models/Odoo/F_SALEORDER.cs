using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessWeb.Models.Odoo
{
	public class F_SALEORDER
	{
		public int Id { get; set; }
		public string Name { get; set; } // Sales order reference
		public int CustomerId { get; set; } // Customer ID
		public DateTime OrderDate { get; set; }
		public decimal TotalAmount { get; set; }
		public string Status { get; set; } // Status of the order (e.g., draft, confirmed)
		public string PaymentTerms { get; set; }
		public DateTime ExpectedDeliveryDate { get; set; }
		public string ShippingMethod { get; set; }
		public string Currency { get; set; }
		public string Notes { get; set; } // Additional notes
	}
}
