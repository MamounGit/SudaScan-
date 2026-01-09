using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using SudaScan.Services;

var builder = WebApplication.CreateBuilder(args);

// 🔹 إيجاد منفذ فارغ تلقائيًا لتجنب مشاكل AddressInUse
TcpListener listener = new TcpListener(IPAddress.Loopback, 0);
listener.Start();
int freePort = ((IPEndPoint)listener.LocalEndpoint).Port;
listener.Stop();

// استخدم المنفذ الفارغ
builder.WebHost.UseUrls($"http://localhost:{freePort}");

builder.Services.AddCors();
var app = builder.Build();

// 🔹 تفعيل CORS
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

// 🔹 صفحات ثابتة
app.UseDefaultFiles();
app.UseStaticFiles();

// 🔹 API لمسح الملفات
app.MapPost("/scan", (string format) =>
{
    var img = ScannerService.ScanImage();

    if (format == "pdf")
    {
        var pdf = PdfHelper.ImageToPdf(img);
        return Results.File(pdf, "application/pdf", "scan.pdf");
    }

    return Results.File(img, "image/png", "scan.png");
});

// 🔹 تشغيل المتصفح تلقائيًا بعد نصف ثانية
var url = $"http://localhost:{freePort}";
Task.Run(async () =>
{
    await Task.Delay(500);
    Process.Start(new ProcessStartInfo
    {
        FileName = url,
        UseShellExecute = true
    });
});

Console.WriteLine($"SudaScan running on {url}...");
app.Run();
