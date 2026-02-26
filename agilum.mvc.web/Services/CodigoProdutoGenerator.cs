using SkiaSharp;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.SkiaSharp;
using ZXing.SkiaSharp.Rendering;

public static class CodigoProdutoGenerator
{
    // ---------------------------
    //  GERA CÓDIGO DE BARRAS EAN-13
    // ---------------------------
    public static byte[] GerarBarcodePng(string codigo)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            return null;

        var writer = new BarcodeWriter<SKBitmap>
        {
            Format = BarcodeFormat.EAN_13,
            Options = new EncodingOptions
            {
                Width = 350,
                Height = 150,
                Margin = 2
            },
            Renderer = new SKBitmapRenderer()
        };

        var bitmap = writer.Write(codigo);

        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);

        return data.ToArray();
    }

    // ---------------------------
    //  GERA QR CODE
    // ---------------------------
    public static byte[] GerarQrCodePng(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return null;

        var writer = new BarcodeWriter<SKBitmap>
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Width = 300,
                Height = 300,
                Margin = 1,
                CharacterSet = "UTF-8"
            },
            Renderer = new SKBitmapRenderer()
        };

        var bitmap = writer.Write(texto);

        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);

        return data.ToArray();
    }
}