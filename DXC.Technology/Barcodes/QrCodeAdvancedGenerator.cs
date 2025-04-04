using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXC.Technology.Barcodes
{
    public static class QrCodeAdvancedGenerator
    {
        public static byte[] QrCodeAsBytes(string text)
        {
            DXC.Technology.Barcodes.QrCodeAdvanced.ErrorCorrection errorCorrection =
                DXC.Technology.Barcodes.QrCodeAdvanced.ErrorCorrection.M;

            if (!string.IsNullOrEmpty(text))
            {
                var qrEncoder = new DXC.Technology.Barcodes.QrCodeAdvanced.QREncoder();
                qrEncoder.Encode(errorCorrection, text);

                Bitmap qrImage = DXC.Technology.Barcodes.QrCodeAdvanced.QRCodeToBitmap.CreateBitmap(qrEncoder, 4, 8);

                //Bitmap qrImageSized = qrImage;  //size == 0 ? qrImage : new Bitmap(qrImage, new Size(size, size));
                Bitmap qrImageSized = ScaleBitmap(qrImage, Convert.ToInt32(System.Math.Truncate(qrImage.Width * 1.6)), qrImage.Height);  //size == 0 ? qrImage : new Bitmap(qrImage, new Size(size, size));
                ImageConverter converter = new ImageConverter();
                return (byte[])converter.ConvertTo(qrImageSized, typeof(byte[]));
            }
            return null;
        }

        public static Bitmap ScaleBitmap(Bitmap bmp, int targetwidth, int targetheight)
        {
            int srcwidth = bmp.Width;
            int srcheight = bmp.Height;
            if ((srcwidth <= 0) || (srcheight <= 0) || (targetwidth <= 0) || (targetheight <= 0))
            {
                return bmp;
            }

            // scale to the target width
            decimal scaleX1 = targetwidth;
            decimal scaleY1 = (srcheight * targetwidth) / srcwidth;

            // scale to the target height
            decimal scaleX2 = (srcwidth * targetheight) / srcheight;
            decimal scaleY2 = targetheight;

            // now figure out which one we should use
            bool fScaleOnWidth = (scaleX2 > targetwidth);

            Bitmap result = new Bitmap(targetwidth, targetheight);

            int offsetLeft = Convert.ToInt32(Math.Floor((targetwidth - srcwidth) / 2.0));
            int offsetTop = Convert.ToInt32(Math.Floor((targetheight - srcheight) / 2.0));

            using (Graphics grD = Graphics.FromImage(result))
            {
                grD.DrawImage(bmp, new RectangleF(offsetLeft, offsetTop, bmp.Width, bmp.Height), new RectangleF(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                grD.Flush();
            }

            return result;
        }

        public static string AdvancedQrCodeAsBase64(string text, string correction, int size)
        {
            DXC.Technology.Barcodes.QrCodeAdvanced.ErrorCorrection errorCorrection =
                (correction == null) || (correction.Equals(DXC.Technology.Enumerations.EnumerationHelper.BlankCode)) ?
                DXC.Technology.Barcodes.QrCodeAdvanced.ErrorCorrection.M :
                DXC.Technology.Enumerations.EnumerationHelper.CodeToEnum<DXC.Technology.Barcodes.QrCodeAdvanced.ErrorCorrection>(correction);

            if (!string.IsNullOrEmpty(text))
            {
                var qrEncoder = new DXC.Technology.Barcodes.QrCodeAdvanced.QREncoder();
                qrEncoder.Encode(errorCorrection, text);

                Bitmap qrImage = DXC.Technology.Barcodes.QrCodeAdvanced.QRCodeToBitmap.CreateBitmap(qrEncoder, 4, 8);
                Bitmap qrImageSized = size == 0 ? qrImage : new Bitmap(qrImage, new Size(size, size));
                ImageConverter converter = new ImageConverter();
                return Convert.ToBase64String((byte[])converter.ConvertTo(qrImageSized, typeof(byte[])));
            }
            return "";
        }


    }
}
