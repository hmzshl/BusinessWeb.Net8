using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessWeb.Models.Odoo
{
	public class F_FAMILLE
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int ParentId { get; set; }
		public List<int> ChildIds { get; set; }
		public int Sequence { get; set; }
		public bool WebsitePublished { get; set; }
		public byte[] image_1024 { get; set; } // binary
		public byte[] image_128 { get; set; } // binary
		public byte[] image_1920 { get; set; } // binary
		public byte[] image_256 { get; set; } // binary
		public byte[] image_512 { get; set; } // binary
	}
}
