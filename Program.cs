using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using SudaScan.Services;
using System.Diagnostics;
using WIA;

var builder = WebApplication.CreateBuilder(args);

int port = 59486;
builder.WebHost.UseUrls($"http://localhost:{port}");

builder.Services.AddCors();

var app = builder.Build();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseDefaultFiles();
app.UseStaticFiles();


// =============================
// 🔍 Status Endpoint
// =============================
app.MapGet("/status", () =>
{
    try
    {
        var deviceManager = new DeviceManager();
        bool hasScanner = false;

        for (int i = 1; i <= deviceManager.DeviceInfos.Count; i++)
        {
            if (deviceManager.DeviceInfos[i].Type == WiaDeviceType.ScannerDeviceType)
            {
                hasScanner = true;
                break;
            }
        }

        if (!hasScanner)
            return Results.Json(new { status = "no_scanner" });

        return Results.Json(new { status = "ready" });
    }
    catch (Exception ex)
    {
        return Results.Json(new { status = "service_error", message = ex.Message });
    }
});


// =============================
// 📄 Scan Endpoint
// =============================
app.MapPost("/scan", (string format) =>
{
    try
    {
        if (!ScannerService.IsReady())
            return Results.BadRequest(new { status = "no_scanner", message = "No scanner connected" });

        var img = ScannerService.ScanImage();

        if (format?.ToLower() == "pdf")
            return Results.File(
                PdfHelper.ImageToPdf(img),
                "application/pdf",
                "scan.pdf"
            );

        return Results.File(img, "image/png");
    }
    catch (Exception ex)
    {
        return Results.Problem(
            detail: ex.Message,
            title: "Scan Failed",
            statusCode: 500
        );
    }
});


// =============================
// 🌐 Auto Open Browser
// =============================
Task.Run(() =>
{
    try
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = $"http://localhost:{port}/index.html",
            UseShellExecute = true
        });
    }
    catch { }
});

Console.WriteLine($"SudaScan Agent running on http://localhost:{port}");

app.Run();