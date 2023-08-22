﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GeekShopping.ProductApi.Data.ValueObjects;

public class ProductVO
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? CategoryName { get; set; }
    public string? ImageUrl { get; set; }

}
