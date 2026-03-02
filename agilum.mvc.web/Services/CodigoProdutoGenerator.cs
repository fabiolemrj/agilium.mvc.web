using System;
using System.IO;
using ZXing;
using ZXing.Common;
using QRCoder;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats.Png;

public static class CodigoProdutoGenerator
{
    // ============================
    //  EAN-13 via ZXing + ImageSharp
    // ============================
    public static byte[] GerarBarcodePng(string codigo)
    {
        var writer = new BarcodeWriterPixelData
        {
            Format = BarcodeFormat.EAN_13,
            Options = new EncodingOptions
            {
                Width = 350,
                Height = 150,
                Margin = 2
            }
        };

        var pixelData = writer.Write(codigo);

        using var image = Image.LoadPixelData<Rgba32>(
            pixelData.Pixels,
            pixelData.Width,
            pixelData.Height
        );

        using var ms = new MemoryStream();
        image.Save(ms, new PngEncoder());
        return ms.ToArray();
    }

    // ============================
    //  QRCode via QRCoder (ImageSharp-free)
    // ============================
    public static byte[] GerarQrCodePng(string texto)
    {
        QRCodeGenerator gen = new QRCodeGenerator();
        QRCodeData data = gen.CreateQrCode(texto, QRCodeGenerator.ECCLevel.Q);

        PngByteQRCode qr = new PngByteQRCode(data);

        return qr.GetGraphic(20); // PNG pronto
    }
}