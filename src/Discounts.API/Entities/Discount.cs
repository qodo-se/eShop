namespace Discounts.API.Entities;

public class Discount
{
    public required string Id { get; set; }
    public required string Code { get; set; }
    public required int DiscountPercent { get; set; }
    public required DateTime ValidUnit { get; set; }
}
