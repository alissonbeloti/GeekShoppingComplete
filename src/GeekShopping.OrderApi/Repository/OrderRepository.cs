using GeekShopping.OrderApi.Model;
using Microsoft.EntityFrameworkCore;
using GeekShopping.OrderApi.Model.Context;

namespace GeekShopping.OrderApi.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly DbContextOptions<MySqlContext> _context;

    public OrderRepository(DbContextOptions<MySqlContext> context)
    {
        _context = context;
    }

    public async Task<bool> AddOrder(OrderHeader header)
    {
        if (header == null) return false;
        await using var _db = new MySqlContext(_context);
        _db.Headers.Add(header);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task UpdateOrderPaymentStatus(long orderHeaderId, bool paid)
    {
        await using var _db = new MySqlContext(_context);
        var header = await _db.Headers.FirstOrDefaultAsync(o => o.Id == orderHeaderId);
        if (header != null)
        {
            header.PaymentStatus = paid;
            _db.Headers.Update(header);
            await _db.SaveChangesAsync();
        }
    }
            
}
