using GeekShopping.Web.Models;

namespace GeekShopping.Web.Services.IServices;

public interface IProductService
{
    Task<IEnumerable<ProductViewModel>> FindAllProductsAsync(string token);
    Task<ProductViewModel> FindProductByIdAsync(long id, string token);
    Task<ProductViewModel> CreateProductAsync(ProductViewModel model, string token);
    Task<ProductViewModel> UpdateProductAsync(ProductViewModel model, string token);
    Task<bool> DeleteProductAsync(long id, string token);
}
