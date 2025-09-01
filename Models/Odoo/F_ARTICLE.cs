using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessWeb.Models.Odoo
{
	public class F_ARTICLE
	{
		public List<int> accessory_product_ids { get; set; } // many2many
		public List<int> account_tag_ids { get; set; } // many2many
		public bool active { get; set; } // boolean
		public int? activity_calendar_event_id { get; set; } // many2one
		public DateTime? activity_date_deadline { get; set; } // date
		public string activity_exception_decoration { get; set; } // selection
		public string activity_exception_icon { get; set; } // char
		public List<Activity> activity_ids { get; set; } // one2many
		public string activity_state { get; set; } // selection
		public string activity_summary { get; set; } // char
		public string activity_type_icon { get; set; } // char
		public int? activity_type_id { get; set; } // many2one
		public int? activity_user_id { get; set; } // many2one
		public bool allow_out_of_stock_order { get; set; } // boolean
		public List<int> alternative_product_ids { get; set; } // many2many
		public List<AttributeLine> attribute_line_ids { get; set; } // one2many
		public float? available_threshold { get; set; } // float
		public string barcode { get; set; } // char
		public float? base_unit_count { get; set; } // float
		public int? base_unit_id { get; set; } // many2one
		public string base_unit_name { get; set; } // char
		public float? base_unit_price { get; set; } // monetary
		public bool can_image_1024_be_zoomed { get; set; } // boolean
		public bool can_publish { get; set; } // boolean
		public int? categ_id { get; set; } // many2one
		public int color { get; set; } // integer
		public int? company_id { get; set; } // many2one
		public float? compare_list_price { get; set; } // monetary
		public int? cost_currency_id { get; set; } // many2one
		public string cost_method { get; set; } // selection
		public int? country_of_origin { get; set; } // many2one
		public DateTime create_date { get; set; } // datetime
		public int? create_uid { get; set; } // many2one
		public int? currency_id { get; set; } // many2one
		public string default_code { get; set; } // char
		public string description { get; set; } // html
		public string description_ecommerce { get; set; } // html
		public string description_picking { get; set; } // text
		public string description_pickingin { get; set; } // text
		public string description_pickingout { get; set; } // text
		public string description_purchase { get; set; } // text
		public string description_sale { get; set; } // text
		public string detailed_type { get; set; } // selection
		public string display_name { get; set; } // char
		public string expense_policy { get; set; } // selection
		public string fiscal_country_codes { get; set; } // char
		public bool has_available_route_ids { get; set; } // boolean
		public bool has_configurable_attributes { get; set; } // boolean
		public bool has_message { get; set; } // boolean
		public string hs_code { get; set; } // char
		public int id { get; set; } // integer
		public byte[] image_1024 { get; set; } // binary
		public byte[] image_128 { get; set; } // binary
		public byte[] image_1920 { get; set; } // binary
		public byte[] image_256 { get; set; } // binary
		public byte[] image_512 { get; set; } // binary
		public float? incoming_qty { get; set; } // float
		public string invoice_policy { get; set; } // selection
		public bool is_product_variant { get; set; } // boolean
		public bool is_published { get; set; } // boolean
		public bool is_seo_optimized { get; set; } // boolean
		public float? list_price { get; set; } // float
		public int? location_id { get; set; } // many2one
		public int? message_attachment_count { get; set; } // integer
		public List<int> message_follower_ids { get; set; } // one2many
		public bool message_has_error { get; set; } // boolean
		public int message_has_error_counter { get; set; } // integer
		public bool message_has_sms_error { get; set; } // boolean
		public List<Message> message_ids { get; set; } // one2many
		public bool message_is_follower { get; set; } // boolean
		public bool message_needaction { get; set; } // boolean
		public int message_needaction_counter { get; set; } // integer
		public List<int> message_partner_ids { get; set; } // many2many
		public DateTime? my_activity_date_deadline { get; set; } // date
		public string name { get; set; } // char
		public int nbr_moves_in { get; set; } // integer
		public int nbr_moves_out { get; set; } // integer
		public int nbr_reordering_rules { get; set; } // integer
		public List<int> optional_product_ids { get; set; } // many2many
		public string out_of_stock_message { get; set; } // html
		public float? outgoing_qty { get; set; } // float
		public List<Packaging> packaging_ids { get; set; } // one2many
		public int pricelist_item_count { get; set; } // integer
		public string priority { get; set; } // selection
		public int product_document_count { get; set; } // integer
		public List<int> product_document_ids { get; set; } // one2many
		public List<Property> product_properties { get; set; } // properties
		public List<int> product_tag_ids { get; set; } // many2many
		public List<int> product_template_image_ids { get; set; } // one2many
		public string product_tooltip { get; set; } // char
		public int product_variant_count { get; set; } // integer
		public int? product_variant_id { get; set; } // many2one
		public List<int> product_variant_ids { get; set; } // one2many
		public int? property_account_expense_id { get; set; } // many2one
		public int? property_account_income_id { get; set; } // many2one
		public int? property_stock_inventory { get; set; } // many2one
		public int? property_stock_production { get; set; } // many2one
		public List<int> public_categ_ids { get; set; } // many2many
		public bool purchase_ok { get; set; } // boolean
		public float? qty_available { get; set; } // float
		public float? rating_avg { get; set; } // float
		public string rating_avg_text { get; set; } // selection
		public int rating_count { get; set; } // integer
		public List<Rating> rating_ids { get; set; } // one2many
		public string rating_last_feedback { get; set; } // text
		public byte[] rating_last_image { get; set; } // binary
		public string rating_last_text { get; set; } // selection
		public float? rating_last_value { get; set; } // float
		public float? rating_percentage_satisfaction { get; set; } // float
		public float? reordering_max_qty { get; set; } // float
		public float? reordering_min_qty { get; set; } // float
		public int? responsible_id { get; set; } // many2one
		public List<int> route_from_categ_ids { get; set; } // many2many
		public List<int> route_ids { get; set; } // many2many
		public int sale_delay { get; set; } // integer
		public string sale_line_warn { get; set; } // selection
		public string sale_line_warn_msg { get; set; } // text
		public bool sale_ok { get; set; } // boolean
		public float? sales_count { get; set; } // float
		public List<Seller> seller_ids { get; set; } // one2many
		public string seo_name { get; set; } // char
		public int sequence { get; set; } // integer
		public string service_type { get; set; } // selection
		public bool show_availability { get; set; } // boolean
		public float standard_price { get; set; } // monetary
		public string tax_ids { get; set; } // many2many
		public bool to_weight { get; set; } // boolean
		public string type { get; set; } // selection
		public float? unit_price { get; set; } // monetary
		public int? uom_id { get; set; } // many2one
		public float? uom_po_id { get; set; } // many2one
		public string website_description { get; set; } // text
		public string website_message { get; set; } // html
		public string website_name { get; set; } // char
		public string website_url { get; set; } // char
		public string weight { get; set; } // float
		public int? write_uid { get; set; } // many2one
		public DateTime write_date { get; set; } // datetime
		public bool is_favorite { get; set; } // boolean
		public Decimal Qte { get; set; } // boolean
		public int itm_id { get; set; } // boolean
	}

	public class Activity { /* Define properties as per your requirement */ }
	public class AttributeLine { /* Define properties as per your requirement */ }
	public class Message { /* Define properties as per your requirement */ }
	public class Packaging { /* Define properties as per your requirement */ }
	public class Property { /* Define properties as per your requirement */ }
	public class Rating { /* Define properties as per your requirement */ }
	public class Seller { /* Define properties as per your requirement */ }
}

