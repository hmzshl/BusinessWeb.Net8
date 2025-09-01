using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessWeb.Models.Odoo
{
	public class F_PURCHASEORDER
	{
		public int Id { get; set; } // Purchase order ID
		public int VendorId { get; set; } // Vendor ID
		public DateTime OrderDate { get; set; } // Date of the order
		public decimal TotalAmount { get; set; } // Total amount of the purchase
		public string Status { get; set; } // Status (e.g., draft, confirmed)
		public string Currency { get; set; } // Currency used for the purchase
	}
}
