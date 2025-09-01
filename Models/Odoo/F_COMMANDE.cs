using System;
using System.Collections.Generic;

public class F_COMMANDE
{
	public string access_point_address { get; set; }
	public string access_token { get; set; }
	public string access_url { get; set; }
	public string access_warning { get; set; }
	public int activity_calendar_event_id { get; set; }
	public DateTime activity_date_deadline { get; set; }
	public string activity_exception_decoration { get; set; }
	public string activity_exception_icon { get; set; }
	public List<Activity> activity_ids { get; set; }
	public string activity_state { get; set; }
	public string activity_summary { get; set; }
	public string activity_type_icon { get; set; }
	public int activity_type_id { get; set; }
	public int activity_user_id { get; set; }
	public decimal amount_delivery { get; set; }
	public decimal amount_invoiced { get; set; }
	public float amount_paid { get; set; }
	public decimal amount_tax { get; set; }
	public decimal amount_to_invoice { get; set; }
	public decimal amount_total { get; set; }
	public float amount_undiscounted { get; set; }
	public decimal amount_untaxed { get; set; }
	public int analytic_account_id { get; set; }
	public int attendee_count { get; set; }
	public List<int> authorized_transaction_ids { get; set; }
	public int campaign_id { get; set; }
	public int carrier_id { get; set; }
	public int cart_quantity { get; set; }
	public bool cart_recovery_email_sent { get; set; }
	public string client_order_ref { get; set; }
	public DateTime commitment_date { get; set; }
	public int company_id { get; set; }
	public string country_code { get; set; }
	public DateTime create_date { get; set; }
	public int create_uid { get; set; }
	public int currency_id { get; set; }
	public float currency_rate { get; set; }
	public DateTime date_order { get; set; }
	public int delivery_count { get; set; }
	public string delivery_message { get; set; }
	public bool delivery_rating_success { get; set; }
	public bool delivery_set { get; set; }
	public string delivery_status { get; set; }
	public string display_name { get; set; }
	public DateTime effective_date { get; set; }
	public DateTime expected_date { get; set; }
	public int fiscal_position_id { get; set; }
	public bool has_active_pricelist { get; set; }
	public bool has_message { get; set; }
	public int id { get; set; }
	public int incoterm { get; set; }
	public string incoterm_location { get; set; }
	public int invoice_count { get; set; }
	public List<int> invoice_ids { get; set; }
	public string invoice_status { get; set; }
	public bool is_abandoned_cart { get; set; }
	public bool is_all_service { get; set; }
	public bool is_expired { get; set; }
	public int journal_id { get; set; }
	public string json_popover { get; set; }
	public bool locked { get; set; }
	public int medium_id { get; set; }
	public int message_attachment_count { get; set; }
	public List<int> message_follower_ids { get; set; }
	public bool message_has_error { get; set; }
	public int message_has_error_counter { get; set; }
	public bool message_has_sms_error { get; set; }
	public List<int> message_ids { get; set; }
	public bool message_is_follower { get; set; }
	public bool message_needaction { get; set; }
	public int message_needaction_counter { get; set; }
	public List<int> message_partner_ids { get; set; }
	public DateTime my_activity_date_deadline { get; set; }
	public string name { get; set; }
	public string note { get; set; }
	public bool only_services { get; set; }
	public List<OrderLine> order_line { get; set; }
	public string origin { get; set; }
	public string partner_credit_warning { get; set; }
	public int partner_id { get; set; }
	public int partner_invoice_id { get; set; }
	public int partner_shipping_id { get; set; }
	public int payment_term_id { get; set; }
	public int pending_email_template_id { get; set; }
	public List<int> picking_ids { get; set; }
	public string picking_policy { get; set; }
	public float prepayment_percent { get; set; }
	public int pricelist_id { get; set; }
	public int procurement_group_id { get; set; }
	public List<int> rating_ids { get; set; }
	public bool recompute_delivery_price { get; set; }
	public string reference { get; set; }
	public bool require_payment { get; set; }
	public bool require_signature { get; set; }
	public List<int> sale_order_option_ids { get; set; }
	public int sale_order_template_id { get; set; }
	public float shipping_weight { get; set; }
	public string shop_warning { get; set; }
	public bool show_json_popover { get; set; }
	public bool show_update_fpos { get; set; }
	public bool show_update_pricelist { get; set; }
	public byte[] signature { get; set; }
	public string signed_by { get; set; }
	public DateTime signed_on { get; set; }
	public int source_id { get; set; }
	public string state { get; set; }
	public List<int> tag_ids { get; set; }
	public string tax_calculation_rounding_method { get; set; }
	public int tax_country_id { get; set; }
	public byte[] tax_totals { get; set; }
	public int team_id { get; set; }
	public string terms_type { get; set; }
	public List<int> transaction_ids { get; set; }
	public string type_name { get; set; }
	public int user_id { get; set; }
	public DateTime validity_date { get; set; }
	public int warehouse_id { get; set; }
	public int website_id { get; set; }
	public List<int> website_message_ids { get; set; }
	public List<int> website_order_line { get; set; }
	public DateTime write_date { get; set; }
	public int write_uid { get; set; }
}

// Additional classes for collections
public class Activity
{
	// Define properties for Activity if needed
}

public class OrderLine
{
	// Define properties for OrderLine if needed
}
