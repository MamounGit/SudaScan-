using WIA;

namespace SudaScan.Services;

public static class ScannerService
{
    public static byte[] ScanImage()
    {
        var dialog = new WIA.CommonDialog();
        var image = dialog.ShowAcquireImage(
            WiaDeviceType.ScannerDeviceType,
            WiaImageIntent.ColorIntent,
            WiaImageBias.MaximizeQuality,
            FormatID.wiaFormatPNG,
            false, true, false);

        return (byte[])image.FileData.get_BinaryData();
    }
}
