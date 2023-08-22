using GeekShopping.CouponApi.Data.ValueObjects;
using GeekShopping.CouponApi.Repository;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CouponApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private ICouponRepository _couponRepository;

        public CouponController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository ?? throw new ArgumentNullException(nameof(couponRepository));
        }

        [HttpGet("{code}")]
        [Authorize]
        public async Task<ActionResult<CouponVO>> FindById(string code)
        {
            var coupon = await _couponRepository.GetCouponByCouponCode(code);
            if (coupon == null) { return NotFound(); }
            return Ok(coupon);
        }
    }
}