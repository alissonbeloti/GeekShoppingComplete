using GeekShopping.Web.Models;
using System.Net.Http.Headers;
using GeekShopping.Web.Extensions;
using GeekShopping.Web.Services.IServices;

namespace GeekShopping.Web.Services;

public class ProductService : IProductService
{
    private readonly HttpClient _client;
    public const string BasePath = "api/v1/product";

    public ProductService(HttpClient client)
    {
        _client = client ?? throw new ArgumentException(nameof(client));
    }

    public async Task<IEnumerable<ProductViewModel>> FindAllProductsAsync(string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(BasePath);
        return await response.ReadContentAsAsync<IEnumerable<ProductViewModel>>();
    }

    public async Task<ProductViewModel> FindProductByIdAsync(long id, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync($"{BasePath}/{id}");
        return await response.ReadContentAsAsync<ProductViewModel>();
    }

    public async Task<ProductViewModel> CreateProductAsync(ProductViewModel model, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.PostAsJsonAsync(BasePath, model);
        if (response.IsSuccessStatusCode)
            return await response.ReadContentAsAsync<ProductViewModel>();
        else throw new Exception("Ocorreu um erro ao chamar a API");
    }
    public async Task<ProductViewModel> UpdateProductAsync(ProductViewModel model, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.PutAsJsonAsync(BasePath, model);
        if (response.IsSuccessStatusCode)
            return await response.ReadContentAsAsync<ProductViewModel>();
        else throw new Exception("Ocorreu um erro ao chamar a API");
    }

    public async Task<bool> DeleteProductAsync(long id, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.DeleteAsync($"{BasePath}/{id}");
        if (!response.IsSuccessStatusCode)
            throw new Exception("Ocorreu um erro ao chamar a API");
        return await response.ReadContentAsAsync<bool>();
    }

}
