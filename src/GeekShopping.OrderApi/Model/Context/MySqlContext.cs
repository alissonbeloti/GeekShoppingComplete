using Microsoft.EntityFrameworkCore;

namespace GeekShopping.OrderApi.Model.Context;

public class MySqlContext : DbContext
{
    public MySqlContext(DbContextOptions<MySqlContext> options)
        : base(options)
    { }

    public DbSet<OrderHeader> Headers { get; set; }

    public DbSet<OrderDetail> Details { get; set; }
}
