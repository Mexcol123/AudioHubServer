var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

// + Límite más alto y soporte MessagePack (opcional pero recomendado)
builder.Services.AddSignalR(options =>
{
    options.MaximumReceiveMessageSize = 5 * 1024 * 1024; // 5 MB
    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
})
.AddMessagePackProtocol(); // <-- reduce tamaño con binario

var app = builder.Build();

app.UseCors();

app.MapGet("/", () => "AudioHub OK");
app.MapHub<AudioHub>("/audioHub");

// Respeta puerto de Railway
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(port))
{
    app.Urls.Add($"http://0.0.0.0:{port}");
}

app.Run();
