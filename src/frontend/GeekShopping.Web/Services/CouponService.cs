using GeekShopping.Web.Extensions;
using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;

using System.Net;
using System.Net.Http.Headers;

namespace GeekShopping.Web.Services;

public class CouponService : ICouponService
{
    private readonly HttpClient _client;
    public const string BasePath = "api/v1/coupon";

    public CouponService(HttpClient client)
    {
        _client = client ?? throw new ArgumentException(nameof(client));
    }

    public async Task<CouponViewModel> GetCoupon(string code, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync($"{BasePath}/{code}");
        if (response.StatusCode != HttpStatusCode.OK)
        {
            return new CouponViewModel();
        }
        return await response.ReadContentAsAsync<CouponViewModel>();
    }
}
