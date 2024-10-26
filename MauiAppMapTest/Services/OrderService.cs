
using GD.Shared.Common;
using MauiAppMapTest.DTO;
using MauiAppMapTest.Infra;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MauiAppMapTest.Services
{
    public class OrderService(HttpService http, HttpClient client)
    {
        private readonly HttpService http = http;

        internal async Task<Res<List<Order>>> GetOpenOrdersAsync()
        {
            
            var res = await http.Get<List<Order>>("api/Order/waiting-f");
            if (res.IsSuccess)
            {
                res.Data = res.Data.Where(o => o.Status == GDOrderStatuses.Waiting).ToList();
            }
            return res;
        }

        internal async Task<Res<Order>> TakeOrderAsync(Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"api/order/take?id={id}");
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<Order>();
                return new Res<Order>(data);
            }
            else
            {
                var e = await response.Content.ReadFromJsonAsync<List<string>>();
                return new Res<Order>(e);
            }
        }

        internal async Task<Res<List<Order>>> GetSelectedOrder()
        {
            var res = await http.Get<List<Order>>("api/Order/waiting-f");
            if (res.IsSuccess)
            {
                res.Data = res.Data
                    .Where(o => o.Status == GDOrderStatuses.InDelivery)
                    .Where(o => o.CourierId == AuthProvider.UserToken.id)
                    .ToList();
            }

            return res;
        }

        /// <summary>
        /// order id
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        internal async Task<Res<Order>> LetGoOrderAsync(Guid oid)
        {
            var cid = AuthProvider.UserToken.id;
			var request = new HttpRequestMessage(HttpMethod.Post, $"api/order/letgo/{oid}");
			var response = await client.SendAsync(request);

			if (response.IsSuccessStatusCode)
			{
				var data = await response.Content.ReadFromJsonAsync<Order>();
				return new Res<Order>(data);
			}
			else
			{
				var e = await response.Content.ReadFromJsonAsync<List<string>>();
				return new Res<Order>(e);
			}
		}

        internal async Task<Res<Order>> CloseOrderAsync(Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"api/order/close/{id}");
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<Order>();
                return new Res<Order>(data);
            }
            else
            {
                var e = await response.Content.ReadFromJsonAsync<List<string>>();
                return new Res<Order>(e);
            }
        }

		internal async Task<Res<List<CStatResponse>>> GetWorkStat()
		{
			var request = new HttpRequestMessage(HttpMethod.Post, $"api/courier/stat");
			var response = await client.SendAsync(request);

			if (response.IsSuccessStatusCode)
			{
				var data = await response.Content.ReadFromJsonAsync<List<CStatResponse>>();
				return new Res<List<CStatResponse>>(data);
			}
			else
			{
				var e = await response.Content.ReadFromJsonAsync<List<string>>();
				return new Res<List<CStatResponse>>(e);
			}
		}
	}
}