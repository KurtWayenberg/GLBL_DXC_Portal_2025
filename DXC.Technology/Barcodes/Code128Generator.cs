using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace DXC.Technology.Barcodes
{
    public enum AlignType
    {
        Left = 0, Center = 1, Right = 2
    }

    public enum BarcodeWeight
    {
        NotSpecified = 0, Small = 1, Medium = 2, Large = 3
    }

    public enum BarColorEnum
    {
        White = 0,
        Black = 1
    }

    /// <summary>
    /// Code 128b Generator - code downloaded from Rodney Marr (RodMarr@mailcity.com) October 7, 2000 and adapted
    /// Permission was granted for public use and royalty-free distribution.
    /// No mention of source or credits was required.
    /// </summary>
    public class Code128bGenerator : IDisposable
    {
        #region Static Fields

        /// <summary>
        /// Maximum file size allowed (2 MB).
        /// </summary>
        private const int MaxFileSize = 2097152;

        #endregion

        #region Instance Fields

        /// <summary>
        /// Image format for the barcode.
        /// </summary>
        private System.Drawing.Imaging.ImageFormat barcodeImageFormat = System.Drawing.Imaging.ImageFormat.Bmp;

        /// <summary>
        /// Font used for the footer text.
        /// </summary>
        private Font footerFont = new Font("Courier", 8);

        /// <summary>
        /// Indicates whether the object has been disposed.
        /// </summary>
        private bool disposed;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Code128bGenerator"/> class with default dimensions.
        /// </summary>
        public Code128bGenerator()
        {
            Width = 500;
            Height = 80;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Code128bGenerator"/> class with specified dimensions.
        /// </summary>
        /// <param name="width">Width of the barcode.</param>
        /// <param name="height">Height of the barcode.</param>
        public Code128bGenerator(int width, int height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Code128bGenerator"/> class with specified dimensions, alignment, and weight.
        /// </summary>
        /// <param name="width">Width of the barcode.</param>
        /// <param name="height">Height of the barcode.</param>
        /// <param name="verticalAlign">Vertical alignment of the barcode.</param>
        /// <param name="weight">Weight of the barcode.</param>
        public Code128bGenerator(int width, int height, AlignType verticalAlign, BarcodeWeight weight)
        {
            VerticalAlign = verticalAlign;
            Weight = weight;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Code128bGenerator"/> class with specified dimensions, alignment, weight, and margins.
        /// </summary>
        /// <param name="width">Width of the barcode.</param>
        /// <param name="height">Height of the barcode.</param>
        /// <param name="verticalAlign">Vertical alignment of the barcode.</param>
        /// <param name="weight">Weight of the barcode.</param>
        /// <param name="margin">Margin for the barcode.</param>
        public Code128bGenerator(int width, int height, AlignType verticalAlign, BarcodeWeight weight, int margin)
        {
            VerticalAlign = verticalAlign;
            Weight = weight;
            Width = width;
            Height = height;
            LeftMargin = margin;
            TopMargin = margin;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Width of the barcode.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Height of the barcode.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Vertical alignment of the barcode.
        /// </summary>
        public AlignType VerticalAlign { get; set; } = AlignType.Center;

        /// <summary>
        /// Left margin of the barcode.
        /// </summary>
        public int LeftMargin { get; set; } = 10;

        /// <summary>
        /// Top margin of the barcode.
        /// </summary>
        public int TopMargin { get; set; } = 10;

        /// <summary>
        /// Height of the barcode area.
        /// </summary>
        public int BarcodeHeight { get; set; } = 50;

        /// <summary>
        /// Indicates whether the footer should be displayed.
        /// </summary>
        public bool ShowFooter { get; set; } = true;

        /// <summary>
        /// Weight of the barcode.
        /// </summary>
        public BarcodeWeight Weight { get; set; } = BarcodeWeight.Small;

        /// <summary>
        /// Font used for the footer text.
        /// </summary>
        public Font FooterFont
        {
            get => footerFont;
            set => footerFont = value;
        }

        /// <summary>
        /// Image format for the barcode.
        /// </summary>
        public System.Drawing.Imaging.ImageFormat BarcodeImageFormat
        {
            get => barcodeImageFormat;
            set => barcodeImageFormat = value;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Generates a barcode as a memory stream.
        /// </summary>
        /// <param name="code">The code to encode in the barcode.</param>
        /// <returns>A memory stream containing the barcode image.</returns>
        public System.IO.MemoryStream GetBarcode(string code)
        {
            if (string.IsNullOrEmpty(code))
                throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Code");

            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            Bitmap bmp = new Bitmap(Width, Height);
            Graphics graphics = Graphics.FromImage(bmp);
            graphics.FillRectangle(Brushes.White, 0, 0, Width, Height);

            bmp.Save(memoryStream, barcodeImageFormat);
            return memoryStream;
        }

        /// <summary>
        /// Generates a barcode as a byte array.
        /// </summary>
        /// <param name="code">The code to encode in the barcode.</param>
        /// <returns>A byte array containing the barcode image.</returns>
        public byte[] GetBarcodeAsByteArray(string code)
        {
            using var memoryStream = GetBarcode(code);
            byte[] fileContents = new byte[MaxFileSize];
            memoryStream.Position = 0;
            int result = memoryStream.Read(fileContents, 0, MaxFileSize);
            byte[] fileContentsStripped = new byte[result];
            Array.Copy(fileContents, fileContentsStripped, result);
            return fileContentsStripped;
        }

        /// <summary>
        /// Generates a barcode as an image.
        /// </summary>
        /// <param name="code">The code to encode in the barcode.</param>
        /// <returns>An image containing the barcode.</returns>
        public Image GetBarcodeAsImage(string code)
        {
            using var memoryStream = GetBarcode(code);
            return Image.FromStream(memoryStream);
        }

        /// <summary>
        /// Augments a data table with barcode images.
        /// </summary>
        /// <param name="table">The data table to augment.</param>
        /// <param name="codeColumn">The column containing the codes.</param>
        /// <param name="imageColumn">The column to store the barcode images.</param>
        public void AugmentDataTableWithBarcode(DataTable table, string codeColumn, string imageColumn)
        {
            if (table == null)
                throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Table");

            if (table.Columns[codeColumn] == null)
                throw new DXC.Technology.Exceptions.NamedExceptions.TechnicalException("InvalidCodeColumn");

            if (table.Columns[imageColumn] == null)
                throw new DXC.Technology.Exceptions.NamedExceptions.TechnicalException("InvalidImageColumn");

            foreach (DataRow row in table.Rows)
            {
                if (row[codeColumn] != DBNull.Value)
                {
                    string code = Convert.ToString(row[codeColumn], DXC.Technology.Utilities.StringFormatProvider.Default);
                    row[imageColumn] = GetBarcodeAsByteArray(code);
                }
            }
        }

        #endregion

        #region IDisposable Implementation

        /// <summary>
        /// Disposes the resources used by the object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the resources used by the object.
        /// </summary>
        /// <param name="disposing">Indicates whether managed resources should be disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    footerFont?.Dispose();
                }

                disposed = true;
            }
        }

        /// <summary>
        /// Finalizer for the object.
        /// </summary>
        ~Code128bGenerator()
        {
            Dispose(false);
        }

        /// <summary>
        /// Validates that the object has not been disposed.
        /// </summary>
        public void ValidateNonDisposedness()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        #endregion
    }
}