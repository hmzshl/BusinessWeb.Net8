using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessWeb.Models.BusinessWebDB
{
    [Table("T_AuthorizeMobile", Schema = "dbo")]
    public partial class TAuthorizeMobile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public string UserID { get; set; }

        public int Societe { get; set; }

        public string RoleID { get; set; }

        public string Url { get; set; }

        public bool Visible { get; set; }
    }
}