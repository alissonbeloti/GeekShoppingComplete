using Microsoft.EntityFrameworkCore;
using GeekShopping.CartApi.Data.ValueObjects;
using System.Net.Http.Headers;
using System.Net;
using System.Text.Json;

namespace GeekShopping.CartApi.Repository;

public class CouponRepository : ICouponRepository
{
    private readonly HttpClient _client;
    public const string BasePath = "api/v1/coupon";

    public CouponRepository(HttpClient client)
    {
        _client = client;
    }

    public async Task<CouponVO> GetCouponByCouponCode(string couponCode, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync($"{BasePath}/{couponCode}");
        if (response.StatusCode != HttpStatusCode.OK)
        {
            return new CouponVO();
        }
        var content = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<CouponVO>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        })?? new CouponVO();
    }
}
