using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessWeb.Models.BusinessWebDB
{
    [Table("T_Societe", Schema = "dbo")]
    public partial class TSociete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public string Intitule { get; set; }

        public string Ville { get; set; }

        public string Adresse { get; set; }

        public decimal Superficie { get; set; }

        public string IdF { get; set; }

        public string ICE { get; set; }

        public string RC { get; set; }

        public string CNSS { get; set; }

        public string IdTVA { get; set; }

        public string Patente { get; set; }

        public decimal Capital { get; set; }

        public int FormeJuridique { get; set; }

        public string Telephone { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public string Web { get; set; }

        public string Abrege { get; set; }

        public DateTime? DateDebut { get; set; }

        public DateTime? DateFin { get; set; }

        public bool GestionCommercial { get; set; }

        public bool Rendement { get; set; }

        public bool RH { get; set; }

        public bool SousTraitance { get; set; }

        public bool Caisse { get; set; }

        public bool Banque { get; set; }

        public bool Recolte { get; set; }

        public bool RecolteSenegal { get; set; }

        public DateTime? DateCreation { get; set; }

        public int Region { get; set; }

        public bool Comptabilite { get; set; }

        public bool Tracabilite { get; set; }

        public string Serveur { get; set; }

        [Column("Base")]
        public string Base1 { get; set; }

        public string Passe { get; set; }

        public bool HistoriqueConnexion { get; set; }

        public int VersionSage { get; set; }
    }
}