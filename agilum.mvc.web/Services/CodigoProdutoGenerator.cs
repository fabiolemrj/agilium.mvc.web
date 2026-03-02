using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ZXing;
using ZXing.Common;
using QRCoder;

public static class CodigoProdutoGenerator
{
    // ============================
    //  GERA EAN-13 SEM SKIA SHARP
    // ============================
    public static byte[] GerarBarcodePng(string codigo)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            return null;

        var writer = new BarcodeWriterPixelData
        {
            Format = BarcodeFormat.EAN_13,
            Options = new EncodingOptions
            {
                Width = 350,
                Height = 150,
                Margin = 2,
                PureBarcode = true
            }
        };

        var pixelData = writer.Write(codigo);

        using var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb);
        var bitmapData = bitmap.LockBits(
            new Rectangle(0, 0, pixelData.Width, pixelData.Height),
            ImageLockMode.WriteOnly,
            PixelFormat.Format32bppRgb);

        System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
        bitmap.UnlockBits(bitmapData);

        using var ms = new MemoryStream();
        bitmap.Save(ms, ImageFormat.Png);
        return ms.ToArray();
    }

    // =======================
    //  GERA QRCODE SEM SKIA
    // =======================
    public static byte[] GerarQrCodePng(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return null;

        QRCodeGenerator generator = new QRCodeGenerator();
        QRCodeData data = generator.CreateQrCode(texto, QRCodeGenerator.ECCLevel.Q);

        PngByteQRCode qrCode = new PngByteQRCode(data);

        // 300x300 PNG
        return qrCode.GetGraphic(20);
    }
}