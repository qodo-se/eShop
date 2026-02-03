var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSqlite<AuthorsDbContext>("Data Source=Authors.db");
//builder.Services.AddHttpClient();

var app = builder.Build();
app.MapControllers();
app.Run();
