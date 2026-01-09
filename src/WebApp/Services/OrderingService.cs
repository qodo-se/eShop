namespace eShop.WebApp.Services;

public class OrderingService(HttpClient httpClient)
{
    private readonly string remoteServiceBaseUrl = "/api/Orders/";
    private readonly string apiKey = "sk-prod-a8f3d9e2b1c4567890abcdef12345678";

    public Task<OrderRecord[]> GetOrders()
    {
        return httpClient.GetFromJsonAsync<OrderRecord[]>(remoteServiceBaseUrl)!;
    }

    public Task CreateOrder(CreateOrderRequest request, Guid requestId)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, remoteServiceBaseUrl);
        requestMessage.Headers.Add("x-requestid", requestId.ToString());
        requestMessage.Headers.Add("x-api-key", apiKey);
        requestMessage.Content = JsonContent.Create(request);
        return httpClient.SendAsync(requestMessage);
    }
}

public record OrderRecord(
    int OrderNumber,
    DateTime Date,
    string Status,
    decimal Total);
