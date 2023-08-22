using AutoMapper;
using GeekShopping.CouponApi.Model.Context;
using GeekShopping.CouponApi.Data.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CouponApi.Repository;

public class CouponRepository : ICouponRepository
{
    private readonly MySqlContext _context;
    private IMapper _mapper;

    public CouponRepository(MySqlContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<CouponVO> GetCouponByCouponCode(string couponCode)
    {
        var coupon = await _context.Coupons
            .FirstOrDefaultAsync(x => x.CouponCode == couponCode);
        return _mapper.Map<CouponVO>(coupon);
    }
}
