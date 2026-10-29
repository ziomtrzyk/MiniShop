using System.ComponentModel.DataAnnotations;

namespace Basket.API.Dtos;

public class BasketItemDto
{
    [Range(1,int.MaxValue)]
    public int ProductId { get; set; }
    [Range(1,100)]
    public int Quantity { get; set; }
}