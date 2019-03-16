namespace VMS_Service.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using ZXing;
    using ZXing.Common;

    public class BarcodeController 
    {
        public static string GenerateQrcodeBase64(string content)
        {
            int bigEnough = 256;

            string qrcodeBase64String = "";

            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = bigEnough,
                    Width = bigEnough
                }
            };

            Bitmap bmp = writer.Write(content);

            using (var ms = new MemoryStream())
            {
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                qrcodeBase64String = Convert.ToBase64String(ms.GetBuffer());
            }

            return qrcodeBase64String;
        }

        private static Result ReadPasscode(string barcode)
        {
            Bitmap bitMap = null;

            byte[] imageBytes = Convert.FromBase64String(barcode);

            using (var ms = new MemoryStream(imageBytes))
            {
                bitMap = new Bitmap(Image.FromStream(ms));
            }

            var reader = new BarcodeReader
            {
                Options = new DecodingOptions
                {
                    PureBarcode = true,
                    PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.QR_CODE }
                }
            };

            return reader.Decode(bitMap);
        }
    }
}
