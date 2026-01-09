using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5050");
builder.Services.AddCors();

var app = builder.Build();

// CORS
app.UseCors(x =>
    x.AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader());

// صفحات ثابتة
app.UseDefaultFiles();
app.UseStaticFiles();

// مسح الملفات
app.MapPost("/scan", (string format) =>
{
    var img = SudaScan.Services.ScannerService.ScanImage();

    if (format == "pdf")
    {
        var pdf = SudaScan.Services.PdfHelper.ImageToPdf(img);
        return Results.File(pdf, "application/pdf", "scan.pdf");
    }

    return Results.File(img, "image/png", "scan.png");
});


var url = "http://localhost:5050";
Task.Run(async () =>
{
    
    await Task.Delay(800);

    Process.Start(new ProcessStartInfo
    {
        FileName = url,
        UseShellExecute = true
    });
});


app.Run();
