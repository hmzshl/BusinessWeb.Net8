using AntDesign.Charts;
using BusinessWeb;
using BusinessWeb.Models.DB;
using BusinessWeb.Models.Odoo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Text;
using System.Xml.Linq;
using F_ARTICLE = BusinessWeb.Models.Odoo.F_ARTICLE;
using F_FAMILLE = BusinessWeb.Models.Odoo.F_FAMILLE;

public class ApiDB
{
	// Single shared HttpClient — avoids socket exhaustion from per-request instantiation.
	private static readonly HttpClient _httpClient = new HttpClient();

	private readonly string _url;
	private readonly string _db;
	private readonly string _username;
	private readonly string _password;
	private readonly ILogger<ApiDB> _logger;
	private int _uid;

	private ApiDB(string url, string db, string username, string password, int uid, ILogger<ApiDB> logger)
	{
		_url      = url;
		_db       = db;
		_username = username;
		_password = password;
		_uid      = uid;
		_logger   = logger;
	}

	/// <param name="configuration">App configuration — reads Odoo:Url and Odoo:Database keys.</param>
	public static async Task<ApiDB> CreateAsync(string username, string password,
		IConfiguration configuration, ILogger<ApiDB> logger)
	{
		var url = configuration["Odoo:Url"] ?? "https://noveldata.ma";
		var db  = configuration["Odoo:Database"] ?? "OdooDB";
		var apiDb = new ApiDB(url, db, username, password, -1, logger);
		var uid   = await apiDb.AuthenticateAsync();
		return new ApiDB(url, db, username, password, uid, logger);
	}

	/// <summary>Backwards-compatible overload — uses hardcoded defaults when no config is available.</summary>
	public static async Task<ApiDB> CreateAsync(string username, string password)
	{
		var logger = LoggerFactory.Create(b => b.AddConsole()).CreateLogger<ApiDB>();
		var apiDb  = new ApiDB("https://noveldata.ma", "OdooDB", username, password, -1, logger);
		var uid    = await apiDb.AuthenticateAsync();
		return new ApiDB("https://noveldata.ma", "OdooDB", username, password, uid, logger);
	}

	// Authenticate user and get UID
	public async Task<int> AuthenticateAsync()
	{
		var client = new RestClient($"{_url}/jsonrpc");
		var request = new RestRequest { Method = Method.Post };
		request.AddHeader("Content-Type", "application/json");
		request.AddJsonBody(new
		{
			jsonrpc = "2.0",
			method = "call",
			@params = new { service = "common", method = "login", args = new object[] { _db, _username, _password } },
			id = 1
		});

		var response = await client.ExecuteAsync(request);
		_logger?.LogDebug("Odoo authenticate response: {Status}", response.StatusCode);

		if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
		{
			try
			{
				return JObject.Parse(response.Content)["result"].Value<int>();
			}
			catch (JsonReaderException ex)
			{
				_logger?.LogWarning(ex, "Odoo authenticate: JSON parse error");
			}
		}
		else
		{
			_logger?.LogError("Odoo authenticate failed: {Status} {Content}", response.StatusCode, response.Content);
		}

		return -1;
	}

	private byte[] TryDecodeBase64(string base64String)
	{
		if (string.IsNullOrWhiteSpace(base64String))
		{
			return null; // No image data
		}

		try
		{
			return Convert.FromBase64String(base64String);
		}
		catch (FormatException)
		{
			// Handle invalid base64 data
			return null;
		}
	}

	// Generic method to execute a search_read
	public async Task<JArray> ExecuteSearchReadAsync(string model, string[] fields, object[] domain)
	{
		var request = new HttpRequestMessage(HttpMethod.Post, $"{_url}/jsonrpc")
		{
			Content = new StringContent(
				JsonConvert.SerializeObject(new
				{
					jsonrpc = "2.0",
					method = "call",
					@params = new
					{
						service = "object",
						method = "execute_kw",
						args = new object[] { _db, _uid, _password, model, "search_read", domain, new { fields } }
					}
				}),
				Encoding.UTF8, "application/json")
		};

		try
		{
			var response = await _httpClient.SendAsync(request);
			response.EnsureSuccessStatusCode();
			var content = await response.Content.ReadAsStringAsync();
			if (!string.IsNullOrEmpty(content))
				return JObject.Parse(content)["result"]?.Value<JArray>() ?? new JArray();
		}
		catch (Exception ex)
		{
			_logger?.LogError(ex, "Odoo search_read failed for model {Model}", model);
		}

		return new JArray();
	}

	public async Task<List<F_ARTICLE>> GetArticlesAsync(object[] domain)
	{
		string[] fields = { "id", "name", "barcode", "description", "active", "image_256", "is_published", "is_favorite","image_512" };

		// Call the asynchronous version of ExecuteSearchRead
		JArray articlesJson = await ExecuteSearchReadAsync("product.product", fields, domain);

		List<F_ARTICLE> articles = new List<F_ARTICLE>();

		foreach (var articleJson in articlesJson)
		{
			F_ARTICLE article = new F_ARTICLE
			{
				id = articleJson.Value<int>("id"),
				name = articleJson.Value<string>("name"),
				barcode = articleJson.Value<string>("barcode"),
				description = articleJson.Value<string>("description"),
				active = articleJson.Value<bool>("active"),
				is_published = articleJson.Value<bool>("is_published"),
				image_256 = TryDecodeBase64(articleJson.Value<string>("image_256")), // Handle image
				image_512 = TryDecodeBase64(articleJson.Value<string>("image_512")), // Handle image
				is_favorite = articleJson.Value<bool>("is_favorite")
			};
			articles.Add(article);
		}

		return articles;
	}

	public async Task<List<F_FAMILLE>> GetFamillesAsync(object[] domain)
	{
		string[] fields = { "id", "name", "image_256", "website_description" };
		JArray categoriesJson = await ExecuteSearchReadAsync("product.public.category", fields, domain);
		var helpers = new Helpers();

		var categories = categoriesJson.Select(j => new F_FAMILLE
		{
			Id              = j.Value<int>("id"),
			Name            = j.Value<string>("name"),
			Description     = helpers.StripHtml(j.Value<string>("website_description")),
			ParentId        = j["parent_id"]?.Type == JTokenType.Array ? j["parent_id"][0].Value<int>() : 0,
			ChildIds        = j["child_id"]?.ToObject<List<int>>() ?? new List<int>(),
			Sequence        = j.Value<int>("sequence"),
			WebsitePublished = j.Value<bool>("website_published"),
			image_256       = TryDecodeBase64(j.Value<string>("image_256"))
		})
		.Where(a => a.image_256 != null)
		.OrderBy(a => a.Name)
		.ToList();

		return categories;
	}
	public async Task<bool> SetArticleAsFavoriteAsync(int articleId, bool isFavorite)
	{
		var client = new RestClient($"{_url}/jsonrpc");
		var request = new RestRequest { Method = Method.Post };
		request.AddHeader("Content-Type", "application/json");
		request.AddJsonBody(new
		{
			jsonrpc = "2.0",
			method = "call",
			@params = new
			{
				service = "object",
				method = "execute_kw",
				args = new object[] { _db, _uid, _password, "product.product", "write",
					new object[] { new int[] { articleId }, new { is_favorite = isFavorite } } }
			}
		});

		var response = await client.ExecuteAsync(request);

		if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
		{
			try { return JObject.Parse(response.Content)["result"]?.Value<bool>() ?? false; }
			catch (Exception ex) { _logger?.LogWarning(ex, "SetArticleAsFavorite: JSON parse error"); }
		}
		else
		{
			_logger?.LogError("SetArticleAsFavorite failed: {Status} {Content}", response.StatusCode, response.Content);
		}

		return false;
	}

	public async Task<int> CreateEcommerceOrderAsync(List<(int productId, float quantity)> orderLines, string websiteName)
	{
		var orderLinesData = orderLines
			.Select(l => new object[] { 0, 0, new { product_id = l.productId, product_uom_qty = l.quantity } })
			.ToList<object>();

		var body = new
		{
			jsonrpc = "2.0",
			method = "call",
			@params = new
			{
				service = "object",
				method = "execute_kw",
				args = new object[] { _db, _uid, _password, "sale.order", "create",
					new { partner_id = 44, order_line = orderLinesData } }
			}
		};

		var client = new RestClient($"{_url}/jsonrpc");
		var request = new RestRequest { Method = Method.Post };
		request.AddHeader("Content-Type", "application/json");
		request.AddJsonBody(body);

		var response = await client.ExecuteAsync(request);

		if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
		{
			try { return JObject.Parse(response.Content)["result"]?.Value<int>() ?? 0; }
			catch (Exception ex) { _logger?.LogWarning(ex, "CreateEcommerceOrder: JSON parse error"); }
		}
		else
		{
			_logger?.LogError("CreateEcommerceOrder failed: {Status} {Content}", response.StatusCode, response.Content);
		}

		return 0;
	}
	public async Task<List<PartnerAddress>> GetPartnerAddressesAsync(object[] domain)
	{
		// Define the fields to retrieve
		string[] fields = { "id", "name", "street", "city", "zip", "country_id", "type" };

		// Execute the asynchronous search_read query
		JArray addressesJson = await ExecuteSearchReadAsync("res.partner", fields, domain);

		// Convert the JSON result to a list of PartnerAddress objects
		List<PartnerAddress> addresses = new List<PartnerAddress>();

		foreach (var addressJson in addressesJson)
		{
			PartnerAddress address = new PartnerAddress
			{
				Id = addressJson.Value<int>("id"),
				Name = addressJson.Value<string>("name"),
				Street = addressJson.Value<string>("street"),
				City = addressJson.Value<string>("city"),
				Zip = addressJson.Value<string>("zip"),
				Country = addressJson["country_id"] != null && addressJson["country_id"].Type == JTokenType.Array
					? addressJson["country_id"][1].Value<string>() // Get country name
					: null,
				AddressType = addressJson.Value<string>("type")
			};
			addresses.Add(address);
		}

		return addresses;
	}
	public async Task<List<SaleOrder>> GetSaleOrdersForPartnerAsync(object[] domain)
	{
		// Define the fields to retrieve for sale orders
		string[] fields =
		{
		"name", "date_order", "amount_total", "state", "partner_id", "pricelist_id",
		"user_id", "team_id", "note", "payment_term_id", "invoice_status", "picking_ids",
		"order_line", "currency_id", "company_id"
	};

		// Execute the asynchronous search_read query
		JArray saleOrdersJson = await ExecuteSearchReadAsync("sale.order", fields, domain);

		// Convert the JSON result to a list of SaleOrder objects
		List<SaleOrder> saleOrders = new List<SaleOrder>();

		foreach (var saleOrderJson in saleOrdersJson)
		{
			SaleOrder saleOrder = new SaleOrder
			{
				// Extract values from the JSON response
				Name = saleOrderJson.Value<string>("name"),
				DateOrder = saleOrderJson.Value<DateTime>("date_order"),
				AmountTotal = saleOrderJson.Value<double>("amount_total"),
				State = saleOrderJson.Value<string>("state"),
				Note = saleOrderJson.Value<string>("note"),
				InvoiceStatus = saleOrderJson.Value<string>("invoice_status")
			};

			// Handle fields that are arrays (like partner_id, pricelist_id, etc.)
			saleOrder.PartnerId = saleOrderJson["partner_id"] != null && saleOrderJson["partner_id"].Type == JTokenType.Array
				? saleOrderJson["partner_id"][0].Value<int>() // Get partner ID
				: 0;

			saleOrder.PricelistId = saleOrderJson["pricelist_id"] != null && saleOrderJson["pricelist_id"].Type == JTokenType.Array
				? saleOrderJson["pricelist_id"][0].Value<int>() // Get pricelist ID
				: 0;

			saleOrder.UserId = saleOrderJson["user_id"] != null && saleOrderJson["user_id"].Type == JTokenType.Boolean
				? saleOrderJson.Value<bool>("user_id") // Handle user_id as boolean (could be null or false)
				: false;

			saleOrder.TeamId = saleOrderJson["team_id"] != null && saleOrderJson["team_id"].Type == JTokenType.Array
				? saleOrderJson["team_id"][0].Value<int>() // Get team ID
				: 0;

			saleOrder.PaymentTermId = saleOrderJson["payment_term_id"] != null && saleOrderJson["payment_term_id"].Type == JTokenType.Array
				? saleOrderJson["payment_term_id"][0].Value<int>() // Get payment term ID
				: 0;

			saleOrder.CurrencyId = saleOrderJson["currency_id"] != null && saleOrderJson["currency_id"].Type == JTokenType.Array
				? saleOrderJson["currency_id"][0].Value<int>() // Get currency ID
				: 0;

			saleOrder.CompanyId = saleOrderJson["company_id"] != null && saleOrderJson["company_id"].Type == JTokenType.Array
				? saleOrderJson["company_id"][0].Value<int>() // Get company ID
				: 0;

			// Handle picking_ids as a list of integers
			saleOrder.PickingIds = saleOrderJson["picking_ids"] != null && saleOrderJson["picking_ids"].Type == JTokenType.Array
				? saleOrderJson["picking_ids"].ToObject<List<int>>() // Convert picking_ids to a list of integers
				: new List<int>();

			// Handle order_line as a list of integers (order line IDs)
			saleOrder.OrderLineIds = saleOrderJson["order_line"] != null && saleOrderJson["order_line"].Type == JTokenType.Array
				? saleOrderJson["order_line"].ToObject<List<int>>() // Convert order_line to a list of integers
				: new List<int>();

			// Add the sale order to the list
			saleOrders.Add(saleOrder);
		}

		return saleOrders;
	}
	public async Task<List<SaleOrderLine>> GetSaleOrderLinesAsync(object[] domain)
	{
		// Define the fields to retrieve for sale order lines
		string[] fields =
		{
		"product_id",
		"product_uom_qty",
		"price_unit",
		"price_subtotal",
		"discount",
		"order_id",
		"name",
		"product_uom",
		"tax_id",
		"currency_id"
	};

		// Execute the asynchronous search_read query
		JArray saleOrderLinesJson = await ExecuteSearchReadAsync("sale.order.line", fields, domain);

		// Convert the JSON result to a list of SaleOrderLine objects
		List<SaleOrderLine> saleOrderLines = new List<SaleOrderLine>();

		foreach (var saleOrderLineJson in saleOrderLinesJson)
		{
			SaleOrderLine saleOrderLine = new SaleOrderLine
			{
				// Extract values from the JSON response
				ProductId = saleOrderLineJson["product_id"] != null && saleOrderLineJson["product_id"].Type == JTokenType.Array
					? (saleOrderLineJson["product_id"][0].Value<int>(), saleOrderLineJson["product_id"][1].Value<string>()) // Get product ID and name
					: (0, string.Empty),

				ProductUomQty = saleOrderLineJson.Value<double>("product_uom_qty"),
				PriceUnit = saleOrderLineJson.Value<double>("price_unit"),
				PriceSubtotal = saleOrderLineJson.Value<double>("price_subtotal"),
				Discount = saleOrderLineJson.Value<double>("discount"),

				OrderId = saleOrderLineJson["order_id"] != null && saleOrderLineJson["order_id"].Type == JTokenType.Array
					? (saleOrderLineJson["order_id"][0].Value<int>(), saleOrderLineJson["order_id"][1].Value<string>()) // Get order ID and name
					: (0, string.Empty),

				Name = saleOrderLineJson.Value<string>("name"),

				ProductUom = saleOrderLineJson["product_uom"] != null && saleOrderLineJson["product_uom"].Type == JTokenType.Array
					? (saleOrderLineJson["product_uom"][0].Value<int>(), saleOrderLineJson["product_uom"][1].Value<string>()) // Get UOM ID and name
					: (0, string.Empty),

				TaxIds = saleOrderLineJson["tax_id"] != null && saleOrderLineJson["tax_id"].Type == JTokenType.Array
					? saleOrderLineJson["tax_id"].ToObject<List<int>>() // Convert tax IDs to a list of integers
					: new List<int>(),

				CurrencyId = saleOrderLineJson["currency_id"] != null && saleOrderLineJson["currency_id"].Type == JTokenType.Array
					? (saleOrderLineJson["currency_id"][0].Value<int>(), saleOrderLineJson["currency_id"][1].Value<string>()) // Get currency ID and name
					: (0, string.Empty)
			};

			// Add the sale order line to the list
			saleOrderLines.Add(saleOrderLine);
		}

		return saleOrderLines;
	}
    public async Task<int> CreatePartnerAsync(API_V_COMPTET customer)
    {
        var client = new RestClient($"{_url}/jsonrpc");
        var request = new RestRequest();
        request.Method = Method.Post;
        request.AddHeader("Content-Type", "application/json");

        // Map your customer model to Odoo partner fields
        var partnerData = new
        {
            name = customer.CT_Intitule,
            street = customer.CT_Adresse,
            street2 = customer.CT_Complement,
            city = customer.CT_Ville,
            zip = customer.CT_CodePostal,
            phone = customer.CT_Telephone,
            email = customer.CT_EMail,
            vat = customer.CT_Siret, // SIRET as VAT number
            comment = customer.CT_Commentaire,
            // Add more fields as needed
            is_company = customer.CT_Type == 1, // Adjust based on your CT_Type values
            customer_rank = 1, // Mark as customer
            supplier_rank = 0,
			@ref = customer.CT_Num // Reference field to store CT_Num
        };

        var body = new
        {
            jsonrpc = "2.0",
            method = "call",
            @params = new
            {
                service = "object",
                method = "execute_kw",
                args = new object[]
                {
                _db,
                _uid,
                _password,
                "res.partner", // Model name
                "create", // Method to create
                new object[] { partnerData } // Data
                }
            }
        };

        request.AddJsonBody(body);
        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
        {
            try { return JObject.Parse(response.Content)["result"]?.Value<int>() ?? 0; }
            catch (Exception ex) { _logger?.LogWarning(ex, "CreatePartner: JSON parse error"); }
        }
        else
        {
            _logger?.LogError("CreatePartner failed: {Status} {Content}", response.StatusCode, response.Content);
        }

        return 0;
    }


    public async Task<int?> FindPartnerByReferenceAsync(string reference)
    {
        var domain = new object[] { new object[] { "ref", "=", reference } };
        string[] fields = { "id" };

        JArray partners = await ExecuteSearchReadAsync("res.partner", fields, domain);

        if (partners.Count > 0)
        {
            return partners[0].Value<int>("id");
        }

        return null;
    }

    public async Task<List<(string CustomerCode, int OdooId, bool Success, string Message)>> CreateCustomersBatchAsync(ICollection<API_V_COMPTET> customers)
    {
        var results = new List<(string, int, bool, string)>();

        foreach (var customer in customers)
        {
            try
            {
                // Check if customer already exists in Odoo
                var existingPartner = await FindPartnerByReferenceAsync(customer.CT_Num);

                if (existingPartner != null)
                {
                    results.Add((customer.CT_Num, existingPartner.Value, false, "Customer already exists"));
                    continue;
                }

                int partnerId = await CreatePartnerAsync(customer);

                if (partnerId > 0)
                {
                    results.Add((customer.CT_Num, partnerId, true, "Customer created successfully"));
                }
                else
                {
                    results.Add((customer.CT_Num, 0, false, "Failed to create customer"));
                }
            }
            catch (Exception ex)
            {
                results.Add((customer.CT_Num, 0, false, $"Error: {ex.Message}"));
            }
        }

        return results;
    }










}

