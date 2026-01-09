using PdfSharp.Pdf;
using PdfSharp.Drawing;

namespace SudaScan.Services;

public static class PdfHelper
{
    public static byte[] ImageToPdf(byte[] imageBytes)
    {
        using var ms = new MemoryStream();
        using var imgStream = new MemoryStream(imageBytes);

        var doc = new PdfDocument();
        var page = doc.AddPage();

        using var gfx = XGraphics.FromPdfPage(page);
        var img = XImage.FromStream(imgStream);

        gfx.DrawImage(img, 0, 0, page.Width, page.Height);
        doc.Save(ms);

        return ms.ToArray();
    }
}
