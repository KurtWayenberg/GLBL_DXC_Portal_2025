using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace DXC.Technology.Gis
{
    /// <summary>
    /// Specifies the calibration mode for image processing.
    /// </summary>
    public enum CalibrationMode
    {
        TopLeft,
        TopMiddle,
        TopRight,
        BottomLeft,
        BottomMiddle,
        BottomRight,
        Centre,
        IgnoreAspectRatio,
        PadTop,
        PadBottom,
        PadLeft,
        PadRight,
        PadAll,
        BestFitBottomLeft
    }

    /// <summary>
    /// Specifies the orientation of an image.
    /// </summary>
    public enum ImageOrientation
    {
        Horizontal,
        Vertical,
        Square
    }

    /// <summary>
    /// Specifies the transformation to apply to an image.
    /// </summary>
    public enum ImageTransformation
    {
        TurnLeft,
        TurnRight,
        UpsideDown
    }

    /// <summary>
    /// Provides helper methods for image processing.
    /// </summary>
    public class ImageHelper
    {
        #region Private Properties

        /// <summary>
        /// Thumbnail size used for generating thumbnail images.
        /// </summary>
        private Size thumbNailSize { get; set; } = new Size(75, 75);

        /// <summary>
        /// Thumbnail name suffix used for generating thumbnail images.
        /// </summary>
        private string thumbNailName { get; set; } = "_thumbnail";

        #endregion

        #region Public Methods

        /// <summary>
        /// Generates thumbnail images for a folder.
        /// </summary>
        /// <param name="physicalPath">The physical path of the folder.</param>
        /// <param name="fileName">The name of the file.</param>
        public void GenerateThumbNailImagesForFolder(string physicalPath, string fileName)
        {
            string thumbName = fileName.Replace(".", thumbNailName + ".");
            if (fileName.IndexOf(".gif") > 0)
            {
                GenerateThumbNail(physicalPath, fileName, thumbName, ImageFormat.Gif);
            }
            if (fileName.IndexOf(".jpg") > 0)
            {
                GenerateThumbNail(physicalPath, fileName, thumbName, ImageFormat.Jpeg);
            }
            if (fileName.IndexOf(".bmp") > 0)
            {
                GenerateThumbNail(physicalPath, fileName, thumbName, ImageFormat.Bmp);
            }
        }

        /// <summary>
        /// Generates a thumbnail image.
        /// </summary>
        /// <param name="physicalPath">The physical path of the folder.</param>
        /// <param name="originalFileName">The name of the original file.</param>
        /// <param name="thumbNailFileName">The name of the thumbnail file.</param>
        /// <param name="format">The image format.</param>
        public void GenerateThumbNail(string physicalPath, string originalFileName, string thumbNailFileName, ImageFormat format)
        {
            try
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(Path.Combine(physicalPath, originalFileName));
                System.Drawing.Image thumbNail = new Bitmap(thumbNailSize.Width, thumbNailSize.Height, img.PixelFormat);
                using Graphics graphic = Graphics.FromImage(thumbNail);
                graphic.CompositingQuality = CompositingQuality.HighQuality;
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                Rectangle rectangle = new Rectangle(0, 0, thumbNailSize.Width, thumbNailSize.Height);

                graphic.DrawImage(img, rectangle);
                thumbNail.Save(Path.Combine(physicalPath, thumbNailFileName), format);
                img.Dispose();
            }
            catch (Exception)
            {
                // Handle exception or log error
            }
        }

        /// <summary>
        /// Resizes a picture to the specified size and format.
        /// </summary>
        /// <param name="imageBuffer">The image buffer.</param>
        /// <param name="format">The image format.</param>
        /// <param name="size">The target size.</param>
        /// <returns>A byte array representing the resized image.</returns>
        public byte[] ResizePicture(byte[] imageBuffer, ImageFormat format, Size size)
        {
            using MemoryStream memoryStream = new MemoryStream(imageBuffer);
            using Image sourceImage = Image.FromStream(memoryStream);

            Size targetSize = size;
            if (size.Width > 0)
            {
                targetSize.Width = size.Width;
                targetSize.Height = sourceImage.Height * targetSize.Width / sourceImage.Width;
            }
            else if (size.Height > 0)
            {
                targetSize.Height = size.Height;
                targetSize.Width = sourceImage.Width * targetSize.Height / sourceImage.Height;
            }

            using Image resizedImage = new Bitmap(targetSize.Width, targetSize.Height, PixelFormat.Format24bppRgb);
            using Graphics graphic = Graphics.FromImage(resizedImage);
            graphic.CompositingQuality = CompositingQuality.HighQuality;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            Rectangle rectangle = new Rectangle(0, 0, targetSize.Width, targetSize.Height);
            graphic.DrawImage(sourceImage, rectangle);
            graphic.Save();
            return ImageToByteArray(resizedImage, format);
        }

        /// <summary>
        /// Converts an image to a byte array.
        /// </summary>
        /// <param name="image">The image to convert.</param>
        /// <param name="format">The image format.</param>
        /// <returns>A byte array representing the image.</returns>
        public byte[] ImageToByteArray(System.Drawing.Image image, ImageFormat format)
        {
            using MemoryStream memoryStream = new MemoryStream();
            image.Save(memoryStream, format);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Converts a byte array to an image.
        /// </summary>
        /// <param name="byteArray">The byte array to convert.</param>
        /// <returns>An image created from the byte array.</returns>
        public Image ByteArrayToImage(byte[] byteArray)
        {
            using MemoryStream memoryStream = new MemoryStream(byteArray);
            return Image.FromStream(memoryStream);
        }

        #endregion
    }
}