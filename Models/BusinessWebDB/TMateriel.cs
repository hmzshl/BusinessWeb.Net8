using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessWeb.Models.BusinessWebDB
{
    [Table("API_T_Materiel", Schema = "dbo")]
    public partial class TMateriel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public string Intitule { get; set; }

        public string Type { get; set; }

        public string Marque { get; set; }

        public string Matricule { get; set; }

        public string Chassis { get; set; }

        public string Modele { get; set; }

        [Required]
        public int Conducteur { get; set; }

        public DateTime? DateImmatriculation { get; set; }

        public string NumWW { get; set; }

        public string PuissanceFiscal { get; set; }

        public string Fournisseur { get; set; }

        public DateTime? DateAchat { get; set; }

        [Required]
        public decimal MontantAchat { get; set; }

        [Required]
        public int TypeMateriel { get; set; }

    }
}