using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessWeb.Models.Odoo
{
	public class F_EMPLOYEE
	{
		public int Id { get; set; } // Employee ID
		public string Name { get; set; } // Employee name
		public string JobTitle { get; set; } // Job title
		public string Department { get; set; } // Department name
		public string WorkEmail { get; set; } // Work email
		public string WorkPhone { get; set; } // Work phone
		public int ManagerId { get; set; } // Manager's ID
		public DateTime HireDate { get; set; } // Date of hire
		public string Image { get; set; }
	}
}
