using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessWeb.Models.Odoo
{
	public class PartnerAddress
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Street { get; set; }
		public string City { get; set; }
		public string Zip { get; set; }
		public string Country { get; set; }
		public string AddressType { get; set; } // "delivery" or "invoice"
	}
}
