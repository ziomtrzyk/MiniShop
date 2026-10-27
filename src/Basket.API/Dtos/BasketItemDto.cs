using System.ComponentModel.DataAnnotations;

namespace Basket.API.Dtos;

public class BasketItemDto
{
    [Range(1,int.MaxValue)]
    public int ProductId { get; set; }
    [Required, MaxLength(50)]
    public string ProductName { get; set; } = string.Empty;
    [Range(0.1, 100000)]
    public decimal UnitPrice { get; set; }
    [Range(1,100)]
    public int Quantity { get; set; }
}