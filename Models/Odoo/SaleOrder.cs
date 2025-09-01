using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessWeb.Models.Odoo
{
	public class SaleOrder
	{
		public string Name { get; set; }
		public DateTime DateOrder { get; set; }
		public double AmountTotal { get; set; }
		public string State { get; set; }
		public int PartnerId { get; set; }
		public int PricelistId { get; set; }
		public bool UserId { get; set; }
		public int TeamId { get; set; }
		public string Note { get; set; }
		public int PaymentTermId { get; set; }
		public string InvoiceStatus { get; set; }
		public List<int> PickingIds { get; set; } // List of picking IDs
		public int CurrencyId { get; set; }
		public int CompanyId { get; set; }
		public List<int> OrderLineIds { get; set; } // List of order line IDs
	}

}
