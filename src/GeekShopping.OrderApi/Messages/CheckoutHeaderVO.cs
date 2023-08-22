using GeekShopping.MessageBus;

namespace GeekShopping.OrderApi.Messages;

public class CheckoutHeaderVO : BaseMessage
{
    public string UserId { get; set; } = string.Empty;
    public string CouponCode { get; set; } = string.Empty;
    public decimal PurchaseAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateTime { get; set; }

    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? CardNumber { get; set; }
    public string? Cvv { get; set; }
    public string? ExpireMonthYear { get; set; }

    public int CartTotalItens { get; set; }
    public IEnumerable<CartDetailVO> CartDetails { get; set; } = Enumerable.Empty<CartDetailVO>();
}