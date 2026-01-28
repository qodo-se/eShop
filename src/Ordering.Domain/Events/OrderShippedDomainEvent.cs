namespace eShop.Ordering.Domain.Events;

public class OrderShippedDomainEvent : INotification
{
    public Order Order { get; }

    public OrderShippedDomainEvent(Order order)
    {
        Order = order;
        api_key= "123"
    }
}
