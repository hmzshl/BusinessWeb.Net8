using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;


namespace BusinessWeb.Odoo
{
	public class Orders
	{
		private readonly HttpClient _httpClient;
		private readonly string _baseUrl;

		public Orders()
		{
			_httpClient = new HttpClient();
			_baseUrl = "https://noveldata.ma";
		}
		public async Task<(string sessionId, int partnerId)> GetSetCookieValue(string _username, string _password)
		{
			var payload = new
			{
				jsonrpc = "2.0",
				method = "call",
				@params = new
				{
					db = "OdooDB", // Replace with your actual database name
					login = _username,
					password = _password
				}
			};

			var url = $"{_baseUrl}/web/session/authenticate";
			Console.WriteLine($"Authenticating at URL: {url}");

			var response = await _httpClient.PostAsJsonAsync(url, payload);

			if (!response.IsSuccessStatusCode)
			{
				Console.WriteLine($"Error: {response.StatusCode}");
				Console.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
				throw new Exception($"Authentication failed with status code: {response.StatusCode}");
			}

			// Retrieve the Set-Cookie header
			if (response.Headers.Contains("Set-Cookie"))
			{
				// Look for the session_id cookie
				var setCookieHeader = response.Headers.GetValues("Set-Cookie").FirstOrDefault();

				if (setCookieHeader != null)
				{
					var sessionId = setCookieHeader.Split(';')
												   .FirstOrDefault(cookie => cookie.StartsWith("session_id="))?
												   .Substring("session_id=".Length);

					if (sessionId != null)
					{
						Console.WriteLine($"session_id: {sessionId}");

						// Now retrieve the partner_id from the response body
						var responseBody = await response.Content.ReadFromJsonAsync<JsonDocument>();
						var partnerId = responseBody?.RootElement
							.GetProperty("result")
							.GetProperty("partner_id")
							.GetInt32();

						if (partnerId.HasValue)
						{
							Console.WriteLine($"partner_id: {partnerId.Value}");
							return (sessionId, partnerId.Value);
						}
						else
						{
							throw new Exception("partner_id not found in the response.");
						}
					}
				}
			}

			throw new Exception("Set-Cookie header or session_id not found in the response.");
		}




		// Method to create an eCommerce order
		public async Task<int> CreateEcommerceOrder(int partnerId, List<(int productId, float quantity)> orderLines, string sessionId)
		{
			var orderPayload = new
			{
				jsonrpc = "2.0",
				method = "call",
				@params = new
				{
					model = "sale.order",
					method = "create",
					args = new object[]
					{
				new
				{
					partner_id = partnerId,
					partner_invoice_id = partnerId,
					partner_shipping_id = partnerId,
					team_id = 1,
					website_id = 1,
					order_line = orderLines.ConvertAll(line =>
						new object[] { 0, 0, new { product_id = line.productId, product_uom_qty = line.quantity } })
				}
					},
					kwargs = new { }
				}
			};

			// Add session_id to the request header
			_httpClient.DefaultRequestHeaders.Add("Cookie", $"session_id={sessionId}");

			var orderResponse = await _httpClient.PostAsJsonAsync($"{_baseUrl}/web/dataset/call_kw/sale.order/create", orderPayload);
			orderResponse.EnsureSuccessStatusCode();

			// Read the JSON response
			var orderJsonResponse = await orderResponse.Content.ReadFromJsonAsync<JsonDocument>();

			// Check if the result property exists in the JSON response
			if (orderJsonResponse?.RootElement.TryGetProperty("result", out var resultProperty) == true)
			{
				return resultProperty.GetInt32();
			}
			else
			{
				return 0;
			}
		}
		public async Task<bool> ConfirmOrder(int orderId, string sessionId)
		{
			var payload = new
			{
				jsonrpc = "2.0",
				method = "call",
				@params = new
				{
					model = "sale.order",
					method = "action_confirm",
					args = new object[] { new int[] { orderId } }, // Order ID as an array
					kwargs = new { }
				}
			};

			_httpClient.DefaultRequestHeaders.Add("Cookie", $"session_id={sessionId}");

			var url = $"{_baseUrl}/web/dataset/call_kw/sale.order/action_confirm";
			var response = await _httpClient.PostAsJsonAsync(url, payload);

			if (!response.IsSuccessStatusCode)
			{
				Console.WriteLine($"HTTP Error: {response.StatusCode}");
				Console.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
				throw new Exception($"Failed to confirm order with ID: {orderId}");
			}

			var responseContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine($"Response JSON: {responseContent}");

			try
			{
				var jsonResponse = JsonDocument.Parse(responseContent);
				if (jsonResponse.RootElement.TryGetProperty("result", out var result))
				{
					return result.GetBoolean(); // Assuming the response has a "result" key with a boolean value
				}
				else if (jsonResponse.RootElement.TryGetProperty("error", out var error))
				{
					Console.WriteLine($"Error: {error}");
					throw new Exception($"Odoo Error: {error}");
				}
			}
			catch (JsonException ex)
			{
				Console.WriteLine($"JSON Parsing Error: {ex.Message}");
			}

			throw new Exception($"Unexpected response format: {responseContent}");
		}





	}
}




