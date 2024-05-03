using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessWeb.Models.BusinessWebDB
{
    [Table("API_T_Personnel", Schema = "dbo")]
    public partial class TPersonnel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public string Nom { get; set; }

        [Required]
        public string Prenom { get; set; }

        public DateTime? DateEmbauche { get; set; }

        public string Activite { get; set; }

        [Required]
        public decimal SalaireNet { get; set; }

        [Required]
        public decimal CNSS { get; set; }

        [Required]
        public decimal AMO { get; set; }

        [Required]
        public decimal IR { get; set; }

        [Required]
        public decimal CMR { get; set; }

        public string Banque { get; set; }

        [Required]
        public int Paiement { get; set; }

        public DateTime? DateNaissance { get; set; }

        public string CIN { get; set; }

        public string Adresse { get; set; }

        [Required]
        public int Departement { get; set; }

        public string Telephone { get; set; }

        public string NumCNSS { get; set; }

        [Required]
        public int Agence { get; set; }

        [Required]
        public int Fonction { get; set; }

        public string Email { get; set; }

    }
}