using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessWeb.Models.Odoo
{
	public class SaleOrderLine
	{
		public (int Id, string Name) ProductId { get; set; }
		public double ProductUomQty { get; set; }
		public double PriceUnit { get; set; }
		public double PriceSubtotal { get; set; }
		public double Discount { get; set; }
		public (int Id, string Name) OrderId { get; set; }
		public string Name { get; set; }
		public (int Id, string Name) ProductUom { get; set; }
		public List<int> TaxIds { get; set; }
		public (int Id, string Name) CurrencyId { get; set; }
	}
}
