var builder = WebApplication.CreateBuilder(args);

// CORS abierto para pruebas (ajústalo en producción)
builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

// SignalR con configuración para mensajes grandes (audio chunks)
builder.Services.AddSignalR(options =>
{
    options.MaximumReceiveMessageSize = 2 * 1024 * 1024; // 2 MB
    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
});

var app = builder.Build();

app.UseCors();

app.MapGet("/", () => "AudioHub OK");
app.MapHub<AudioHub>("/audioHub");

// Railway usa variable PORT → escucha en ese puerto
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(port))
{
    app.Urls.Add($"http://0.0.0.0:{port}");
}

app.Run();
