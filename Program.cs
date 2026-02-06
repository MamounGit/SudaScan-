using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using SudaScan.Services;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

int port = 59486;
builder.WebHost.UseUrls($"http://localhost:{port}");

builder.Services.AddCors();

var app = builder.Build();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseDefaultFiles();
app.UseStaticFiles();

// Endpoint لمسح الصورة
app.MapPost("/scan", (string format) =>
{
    try
    {
        var img = ScannerService.ScanImage();

        if (format == "pdf")
            return Results.File(PdfHelper.ImageToPdf(img), "application/pdf", "scan.pdf");

        return Results.File(img, "image/png");
    }
    catch (Exception ex)
    {
        return Results.Text("Error: " + ex.Message);
    }
});

// ✅ إضافة Status Endpoint
app.MapGet("/status", () =>
{
    try
    {
        // اختبار بسيط: محاولة مسح صورة صغيرة أو التحقق من ScannerService
        bool scannerReady = ScannerService.IsReady(); // لو عندك دالة جاهزة
        return Results.Json(new { status = scannerReady ? "ok" : "error" });
    }
    catch
    {
        return Results.Json(new { status = "error" });
    }
});

// فتح المتصفح تلقائيًا على صفحة الحالة
Task.Run(() =>
{
    try
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = $"http://localhost:{port}/Index.html",
            UseShellExecute = true
        });
    }
    catch { }
});

Console.WriteLine($"SudaScan Agent running on http://localhost:{port}");
app.Run();
