using BusinessWeb.Models.DB;

namespace BusinessWeb.Models.Perso
{
    public class V_CompteT
    {
        public string CT_Num { get; set; }
        public string CT_Intitule { get; set; }
        public short? CT_Type { get; set; }
        public string CG_NumPrinc { get; set; }
        public string CT_Qualite { get; set; }
        public string CT_Contact { get; set; }
        public string CT_Adresse { get; set; }
        public string CT_Complement { get; set; }
        public string CT_CodePostal { get; set; }
        public string CT_Ville { get; set; }
        public string CT_CodeRegion { get; set; }
        public string CT_Pays { get; set; }
        public string CT_Raccourci { get; set; }
        public short? N_Devise { get; set; }
        public string CT_Identifiant { get; set; }
        public string CT_Siret { get; set; }
        public string CT_Statistique01 { get; set; }
        public string CT_Statistique02 { get; set; }
        public string CT_Statistique03 { get; set; }
        public string CT_Statistique04 { get; set; }
        public string CT_Statistique05 { get; set; }
        public string CT_Statistique06 { get; set; }
        public string CT_Statistique07 { get; set; }
        public string CT_Statistique08 { get; set; }
        public string CT_Statistique09 { get; set; }
        public string CT_Statistique10 { get; set; }
        public string CT_Commentaire { get; set; }
        public decimal? CT_Encours { get; set; }
        public decimal? CT_Assurance { get; set; }
        public int? CO_No { get; set; }
        public short? CT_Sommeil { get; set; }
        public short? CT_ControlEnc { get; set; }
        public string CT_Telephone { get; set; }
        public string CT_Telecopie { get; set; }
        public string CT_EMail { get; set; }
        public string CT_Site { get; set; }
    }
}
