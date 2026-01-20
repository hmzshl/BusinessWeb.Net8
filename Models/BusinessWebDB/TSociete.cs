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
        [MaxLength(100)]
        public string Intitule { get; set; }

        [MaxLength(100)]
        public string Ville { get; set; }

        [MaxLength(255)]
        public string Adresse { get; set; }

        public decimal Superficie { get; set; }

        [MaxLength(100)]
        public string IdF { get; set; }

        [MaxLength(100)]
        public string ICE { get; set; }

        [MaxLength(100)]
        public string RC { get; set; }

        [MaxLength(100)]
        public string CNSS { get; set; }

        [MaxLength(100)]
        public string IdTVA { get; set; }

        [MaxLength(100)]
        public string Patente { get; set; }

        public decimal Capital { get; set; }

        public int FormeJuridique { get; set; }

        [MaxLength(100)]
        public string Telephone { get; set; }

        [MaxLength(100)]
        public string Fax { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string Web { get; set; }

        [MaxLength(100)]
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

        [MaxLength(100)]
        public string Serveur { get; set; }

        [Column("Base")]
        [MaxLength(100)]
        public string Base1 { get; set; }

        [MaxLength(100)]
        public string Passe { get; set; }

        public bool HistoriqueConnexion { get; set; }

        public int VersionSage { get; set; }

        [MaxLength(50)]
        public string IFU { get; set; }

        [MaxLength(100)]
        public string Responsable { get; set; }

        [MaxLength(50)]
        public string Statut { get; set; }

        public int? Regime { get; set; }

        public int? Periode { get; set; }

        public int? SourceRapprochement { get; set; }

        public int? SourceDetailsFacture { get; set; }

        public string JO_Banques { get; set; }

        public string JO_ANouveaux { get; set; }

        public string JO_Achats { get; set; }

        public string JO_Ventes { get; set; }

        public bool? Rapprochement { get; set; }

        public string JO_Caisses { get; set; }

        public int? SensEncaissement { get; set; }

        public int? SensDecaissement { get; set; }

        public int? NumFA_Achat { get; set; }

        public int? NumFA_Vente { get; set; }

        public string CG_Inclures { get; set; }

        public string CG_Inclures_VE { get; set; }

        public bool? RapprochementSur { get; set; }
    }
}