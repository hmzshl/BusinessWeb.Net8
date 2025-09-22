using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessWeb.Models.BusinessWebDB
{
    [Table("T_Collaborateur", Schema = "dbo")]
    public partial class TCollaborateur
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public string UserName { get; set; }

        public int Societe { get; set; }

        public int CO_No { get; set; }
    }
}