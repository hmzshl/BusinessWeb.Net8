namespace BusinessWeb.Models.Odoo
{
    public class F_ATTRIBUTE
    {
        public int id { get; set; } // ID
        public string name { get; set; } // Attribut
        public DateTime create_date { get; set; } // Créé le
        public int create_uid { get; set; } // Créé par
        public string create_variant { get; set; } // Mode de création des variantes
        public string display_name { get; set; } // Nom d'affichage
        public string display_type { get; set; } // Type d'affichage
        public int number_related_products { get; set; } // Nombre de produits associés
        public int[] product_tmpl_ids { get; set; } // Produits associés
        public int sequence { get; set; } // Séquence
        public int[] value_ids { get; set; } // Valeurs
        public string visibility { get; set; } // Visibilité
        public DateTime write_date { get; set; } // Mis à jour le
        public int write_uid { get; set; } // Mis à jour par
    }
}
