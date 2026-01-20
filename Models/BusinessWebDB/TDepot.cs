using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessWeb.Models.BusinessWebDB
{
    [Table("T_Depot", Schema = "dbo")]
    public partial class TDepot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [MaxLength(100)]
        public string UserName { get; set; }

        public int Societe { get; set; }

        public int DE_No { get; set; }

        public bool Visible { get; set; }
    }
}