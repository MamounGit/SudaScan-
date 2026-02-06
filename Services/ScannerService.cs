using WIA;

namespace SudaScan.Services;

public static class ScannerService
{
    public static byte[] ScanImage()
    {
        // تحديد CommonDialog الخاص بـ WIA صراحة
        var dialog = new WIA.CommonDialog();

        var image = dialog.ShowAcquireImage(
            WiaDeviceType.ScannerDeviceType,
            WiaImageIntent.ColorIntent,
            WiaImageBias.MaximizeQuality,
            FormatID.wiaFormatPNG,
            false,
            true,
            false
        );

        return (byte[])image.FileData.get_BinaryData();
    }

    public static bool IsReady()
    {
        try
        {
            var deviceManager = new WIA.DeviceManager();

            for (int i = 1; i <= deviceManager.DeviceInfos.Count; i++)
            {
                var info = deviceManager.DeviceInfos[i];
                if (info.Type == WiaDeviceType.ScannerDeviceType)
                    return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }
}
