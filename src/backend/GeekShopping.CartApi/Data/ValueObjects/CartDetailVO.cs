﻿using GeekShopping.CartApi.Model.Base;

using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.CartApi.Data.ValueObjects;
public class CartDetailVO
{
    public long Id { get; set; }
    public long CartHeaderId { get; set; }
    public virtual CartHeaderVO? CartHeader { get; set; }
    public long ProductId { get; set; }
    public virtual ProductVO? Product { get; set; }
    public int Count { get; set; }
}
