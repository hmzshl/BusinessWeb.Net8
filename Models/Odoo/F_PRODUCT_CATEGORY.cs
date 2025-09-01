namespace BusinessWeb.Models.Odoo
{
    public class F_PRODUCT_CATEGORY
    {
        public int id { get; set; } // ID
        public string name { get; set; } // Nom
        public string complete_name { get; set; } // Nom complet
        public DateTime create_date { get; set; } // Créé le
        public int create_uid { get; set; } // Créé par
        public string display_name { get; set; } // Nom d'affichage
        public bool filter_for_stock_putaway_rule { get; set; } // stock.putaway.rule
        public string packaging_reserve_method { get; set; } // Réserver des conditionnements
        public int parent_id { get; set; } // Catégorie parente
        public string parent_path { get; set; } // Chemin parent
        public int product_count { get; set; } // # Produits
        public string product_properties_definition { get; set; } // Propriétés du produit
        public int property_account_expense_categ_id { get; set; } // Compte de charges
        public int property_account_income_categ_id { get; set; } // Compte des revenus
        public string property_cost_method { get; set; } // Méthode de coût
        public int property_stock_account_input_categ_id { get; set; } // Compte d'entrée en stock
        public int property_stock_account_output_categ_id { get; set; } // Compte de sortie de stock
        public int property_stock_journal { get; set; } // Journal des stocks
        public int property_stock_valuation_account_id { get; set; } // Compte de valorisation des stocks
        public string property_valuation { get; set; } // Valorisation des stocks
        public int removal_strategy_id { get; set; } // Forcer la stratégie d'enlèvement
        public int[] route_ids { get; set; } // Routes
        public int[] total_route_ids { get; set; } // Total des routes
        public DateTime write_date { get; set; } // Mis à jour le
        public int write_uid { get; set; } // Mis à jour par
        public int[] child_id { get; set; } // Catégories enfants
        public int[] putaway_rule_ids { get; set; } // Stratégies de rangement
    }
}
