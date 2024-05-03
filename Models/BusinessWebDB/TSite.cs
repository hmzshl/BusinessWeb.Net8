using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessWeb.Models.BusinessWebDB
{
    [Table("API_T_Site", Schema = "dbo")]
    public partial class TSite
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public string Intitule { get; set; }

        public string Ville { get; set; }

        public string Adresse { get; set; }

        [Required]
        public int Responsable { get; set; }

    }
}