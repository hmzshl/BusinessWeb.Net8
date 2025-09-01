using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessWeb.Models.Odoo
{
	public class F_STOCKMOVE
	{
		public int Id { get; set; } // Stock move ID
		public int ProductId { get; set; } // Product ID
		public decimal Quantity { get; set; } // Moved quantity
		public string SourceLocation { get; set; } // Source location of the stock
		public string DestinationLocation { get; set; } // Destination location of the stock
		public string Status { get; set; } // Status of the stock move (e.g., done, draft)
	}
}
