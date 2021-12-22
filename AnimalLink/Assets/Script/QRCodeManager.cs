using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;

public class QRCodeManager : MonoBehaviour
{

    private static Color32[] useEncode(string textForEncoding, int width, int height)
    {
        BarcodeWriter writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };

        return writer.Write(textForEncoding);
    }

    public static Texture2D GenQRCode(string str, int w, int h)
    {
        Color32[] color32 = useEncode(str, w, h);
        Texture2D tex = new Texture2D(w, h);
        tex.SetPixels32(color32);
        tex.Apply();

        return tex;
    }
}
