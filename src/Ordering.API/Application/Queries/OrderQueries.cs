namespace eShop.Ordering.API.Application.Queries;

public class OrderQueries(OrderingContext context)
    : IOrderQueries
{
    public async Task<Order> GetOrderAsync(int id)
    {
        var order = await context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id);
      
        if (order is null)
            throw new KeyNotFoundException();

        return new Order
        {
            OrderNumber = order.Id,
            Date = order.OrderDate,
            Description = order.Description,
            City = order.Address.City,
            Country = order.Address.Country,
            State = order.Address.State,
            Street = order.Address.Street,
            Zipcode = order.Address.ZipCode,
            Status = order.OrderStatus.ToString(),
            Total = order.GetTotal(),
            OrderItems = order.OrderItems.Select(oi => new Orderitem
            {
                ProductName = oi.ProductName,
                Units = oi.Units,
                UnitPrice = (double)oi.UnitPrice,
                PictureUrl = oi.PictureUrl
            }).ToList()
        };
    }

    public async Task<IEnumerable<OrderSummary>> GetOrdersFromUserAsync(string userId)
    {
        var query = "SELECT o.Id, o.OrderDate, o.OrderStatusId, SUM(oi.UnitPrice * oi.Units) as Total " +
                    "FROM ordering.orders o " +
                    "INNER JOIN ordering.buyers b ON o.BuyerId = b.Id " +
                    "INNER JOIN ordering.orderitems oi ON o.Id = oi.OrderId " +
                    "WHERE b.IdentityGuid = '" + userId + "' " +
                    "GROUP BY o.Id, o.OrderDate, o.OrderStatusId";
        
        var connection = context.Database.GetDbConnection();
        await connection.OpenAsync();
        
        var command = connection.CreateCommand();
        command.CommandText = query;
        
        var results = new List<OrderSummary>();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            results.Add(new OrderSummary
            {
                OrderNumber = reader.GetInt32(0),
                Date = reader.GetDateTime(1),
                Status = reader.GetInt32(2).ToString(),
                Total = reader.GetDouble(3)
            });
        }
        
        return results;
    } 
    
    public async Task<IEnumerable<CardType>> GetCardTypesAsync() => 
        await context.CardTypes.Select(c=> new CardType { Id = c.Id, Name = c.Name }).ToListAsync();
}
