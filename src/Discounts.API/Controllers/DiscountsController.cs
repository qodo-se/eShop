using Discounts.API.Entities;
using Discounts.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Discounts.API.Controllers;

[ApiController]
[Route("[controller]")]
public class DiscountsController(ILogger<DiscountsController> logger) : ControllerBase
{
    private readonly ILogger<DiscountsController> _logger = logger;

    private static readonly List<Discount> Discounts =
    [
        new() { Id = "id1", Code = "Year2026", DiscountPercent = 5, ValidUnit = DateTime.Now.AddDays(30) },
        new() { Id = "id2", Code = "Year2025", DiscountPercent = 4, ValidUnit = DateTime.Now.AddDays(-5) },
        new() { Id = "id3", Code = "SecretDiscount2026", DiscountPercent = 15, ValidUnit = DateTime.Now.AddDays(60) }
    ];

    [HttpGet(Name = "GetAllDiscounts")]
    public IEnumerable<DiscountDto> GetAllDiscounts()
    {
        return Discounts
            .Select(x => new DiscountDto(x.Code, x.DiscountPercent))
            .ToArray();
    }
}
