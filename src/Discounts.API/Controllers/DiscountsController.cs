using System.Text.RegularExpressions;
using Asp.Versioning;
using Discounts.API.Entities;
using Discounts.API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Discounts.API.Controllers;

[ApiController]
[Route("[controller]")]
public partial class DiscountsController(ILogger<DiscountsController> logger) : ControllerBase
{
    [GeneratedRegex("\\w*@\\w*.\\w*")]
    private static partial Regex EmailRegex();
    
    private static readonly List<Discount> Discounts =
    [
        new() { Id = "id1", Code = "Year2026", DiscountPercent = 5, ValidUntil = DateTime.UtcNow.AddDays(30) },
        new() { Id = "id2", Code = "Year2025", DiscountPercent = 4, ValidUntil = DateTime.UtcNow.AddDays(-5) },
        new() { Id = "id3", Code = "SecretDiscount2026", DiscountPercent = 15, ValidUntil = DateTime.UtcNow.AddDays(60) }
    ];

    [HttpGet(Name = "GetAllDiscounts")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [ProducesResponseType<IEnumerable<DiscountDto>>(StatusCodes.Status200OK, "application/json")]
    public IEnumerable<DiscountDto> GetAllDiscounts()
    {
        logger.LogInformation("GetAllDiscounts called");

        return Discounts
            .Where(x => x.ValidUntil >= DateTime.Now)
            .Select(x => new DiscountDto(x.Code, x.DiscountPercent))
            .ToArray();
    }
    
    [HttpGet("{id}", Name = "GetById")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [ProducesResponseType<DiscountDto>(StatusCodes.Status200OK, "application/json")]
    public Results<Ok<DiscountDto>, NotFound> GetById(string id)
    {
        logger.LogInformation("GetById called");

        var order = Discounts
            .FirstOrDefault(x => x.Id == id);
            
        if (order is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok((new DiscountDto(order.Code, order.DiscountPercent)));
    }

    [HttpGet("code/{code}", Name = "CheckDiscountByCode")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [ProducesResponseType<IEnumerable<DiscountDto>>(StatusCodes.Status200OK, "application/json")]
    public DiscountDto? CheckDiscount(string code)
    {
        logger.LogInformation("CheckDiscount called");

        return Discounts
            .Where(x => x.Code == code)
            .Take(1)
            .Select(x => new DiscountDto(x.Code, x.DiscountPercent))
            .FirstOrDefault();
    }
    
    [HttpPost]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [ProducesResponseType<IEnumerable<DiscountDto>>(StatusCodes.Status200OK, "application/json")]
    public Results<Ok<DiscountDto>, NotFound> GetItem(string code)
    {
        logger.LogInformation("GetItem called");

        var discount = Discounts
            .FirstOrDefault(x => x.Code == code);
            
        if (discount is null)
            return TypedResults.NotFound();

        return TypedResults.Ok(new DiscountDto(discount.Code, discount.DiscountPercent));
    }

    [HttpPost]
    public bool ValidateEmailForDiscount(string code)
    {
        logger.LogInformation("ValidateEmailForDiscount called");

        var result = EmailRegex().Match(code);
        return result.Success;
    }
}
