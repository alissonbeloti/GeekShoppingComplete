using AutoMapper;

using GeekShopping.ProductApi.Data.ValueObjects;
using GeekShopping.ProductApi.Model;
using Microsoft.EntityFrameworkCore;
using GeekShopping.ProductApi.Model.Context;


namespace GeekShopping.ProductApi.Repository;

public class ProductRepository : IProductRepository
{
    private readonly MySqlContext _context;
    private IMapper _mapper;

    public ProductRepository(IMapper mapper, MySqlContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<IEnumerable<ProductVO>> FindAll()
    {
        List<Product> products = await _context.Products.ToListAsync();
        return _mapper.Map<List<ProductVO>>(products);
    }

    public async Task<ProductVO> FindById(long id)
    {
        Product? product = await _context.Products.FindAsync(id);
        return _mapper.Map<ProductVO>(product);
    }

    public async Task<ProductVO> Create(ProductVO product)
    {
        Product prod = _mapper.Map<Product>(product);
        _context.Products.Add(prod);
        await _context.SaveChangesAsync();
        return _mapper.Map<ProductVO>(prod);
    }
    public async Task<ProductVO> Update(ProductVO product)
    {
        Product prod = _mapper.Map<Product>(product);
        _context.Products.Update(prod);
        await _context.SaveChangesAsync();
        return _mapper.Map<ProductVO>(product);
    }

    public async Task<bool> Delete(long id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }


}
