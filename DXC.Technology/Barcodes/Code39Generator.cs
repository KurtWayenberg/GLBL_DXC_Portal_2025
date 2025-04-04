using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace DXC.Technology.Barcodes
{
    /// <summary>
    /// Code 39 Generator - code downloaded from open source forge .net and adapted
    /// </summary>
    public class Code39Generator : IDisposable
    {
        #region Static Fields

        /// <summary>
        /// Maximum file size allowed (2 MB).
        /// </summary>
        private const int MaxFileSize = 2097152; // 2 MB

        #endregion

        #region Instance Fields

        /// <summary>
        /// Font used for the footer.
        /// </summary>
        private readonly Font footerFont = new Font("Courier", 8);

        /// <summary>
        /// Alphabet used for Code 39 encoding.
        /// </summary>
        private readonly string alphabet39 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%*";

        /// <summary>
        /// Encoded representations of Code 39 characters.
        /// </summary>
        private readonly string[] coded39Char =
        {
            /* 0 */ "000110100",
            /* 1 */ "100100001",
            /* 2 */ "001100001",
            /* 3 */ "101100000",
            /* 4 */ "000110001",
            /* 5 */ "100110000",
            /* 6 */ "001110000",
            /* 7 */ "000100101",
            /* 8 */ "100100100",
            /* 9 */ "001100100",
            /* A */ "100001001",
            /* B */ "001001001",
            /* C */ "101001000",
            /* D */ "000011001",
            /* E */ "100011000",
            /* F */ "001011000",
            /* G */ "000001101",
            /* H */ "100001100",
            /* I */ "001001100",
            /* J */ "000011100",
            /* K */ "100000011",
            /* L */ "001000011",
            /* M */ "101000010",
            /* N */ "000010011",
            /* O */ "100010010",
            /* P */ "001010010",
            /* Q */ "000000111",
            /* R */ "100000110",
            /* S */ "001000110",
            /* T */ "000010110",
            /* U */ "110000001",
            /* V */ "011000001",
            /* W */ "111000000",
            /* X */ "010010001",
            /* Y */ "110010000",
            /* Z */ "011010000",
            /* - */ "010000101",
            /* . */ "110000100",
            /*' '*/ "011000100",
            /* $ */ "010101000",
            /* / */ "010100010",
            /* + */ "010001010",
            /* % */ "000101010",
            /* * */ "010010100"
        };

        /// <summary>
        /// Indicates whether the object has been disposed.
        /// </summary>
        private bool disposed;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Code39Generator"/> class with specified width and height.
        /// </summary>
        /// <param name="width">The width of the barcode.</param>
        /// <param name="height">The height of the barcode.</param>
        public Code39Generator(int width, int height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Code39Generator"/> class with specified width, height, alignment, and weight.
        /// </summary>
        /// <param name="width">The width of the barcode.</param>
        /// <param name="height">The height of the barcode.</param>
        /// <param name="alignType">The vertical alignment of the barcode.</param>
        /// <param name="barcodeWeight">The weight of the barcode lines.</param>
        public Code39Generator(int width, int height, AlignType alignType, BarcodeWeight barcodeWeight)
        {
            VerticalAlign = alignType;
            Weight = barcodeWeight;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Code39Generator"/> class with specified width, height, alignment, weight, and margin.
        /// </summary>
        /// <param name="width">The width of the barcode.</param>
        /// <param name="height">The height of the barcode.</param>
        /// <param name="alignType">The vertical alignment of the barcode.</param>
        /// <param name="barcodeWeight">The weight of the barcode lines.</param>
        /// <param name="margin">The margin around the barcode.</param>
        public Code39Generator(int width, int height, AlignType alignType, BarcodeWeight barcodeWeight, int margin)
        {
            VerticalAlign = alignType;
            Weight = barcodeWeight;
            Width = width;
            Height = height;
            LeftMargin = margin;
            TopMargin = margin;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the vertical alignment of the barcode.
        /// </summary>
        public AlignType VerticalAlign { get; set; } = AlignType.Center;

        /// <summary>
        /// Gets or sets the height of the barcode.
        /// </summary>
        public int BarcodeHeight { get; set; } = 50;

        /// <summary>
        /// Gets or sets the left margin of the barcode.
        /// </summary>
        public int LeftMargin { get; set; } = 10;

        /// <summary>
        /// Gets or sets the top margin of the barcode.
        /// </summary>
        public int TopMargin { get; set; } = 10;

        /// <summary>
        /// Gets or sets a value indicating whether the footer should be displayed.
        /// </summary>
        public bool ShowFooter { get; set; } = true;

        /// <summary>
        /// Gets or sets the weight of the barcode lines.
        /// </summary>
        public BarcodeWeight Weight { get; set; } = BarcodeWeight.Small;

        /// <summary>
        /// Gets the font used for the footer.
        /// </summary>
        public Font FooterFont => footerFont;

        /// <summary>
        /// Gets or sets the image format of the barcode.
        /// </summary>
        public System.Drawing.Imaging.ImageFormat BarcodeImageFormat { get; set; } = System.Drawing.Imaging.ImageFormat.Bmp;

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets or sets the width of the barcode.
        /// </summary>
        private int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the barcode.
        /// </summary>
        private int Height { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Generates a barcode and returns it as a MemoryStream.
        /// </summary>
        /// <param name="code">The code to encode in the barcode.</param>
        /// <returns>A MemoryStream containing the barcode image.</returns>
        public System.IO.MemoryStream GetBarcode(string code)
        {
            if (string.IsNullOrEmpty(code))
                throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Code");

            string upperCode = code.ToUpper(DXC.Technology.Utilities.CultureInfoProvider.Default);
            const string intercharacterGap = "0";
            string str = '*' + upperCode + '*';
            int strLength = str.Length;

            foreach (char c in upperCode)
            {
                if (alphabet39.IndexOf(c) == -1 || c == '*')
                {
                    throw new DXC.Technology.Exceptions.NamedExceptions.TechnicalException("Invalid BarCode Text");
                }
            }

            var encodedStringBuilder = new System.Text.StringBuilder();

            for (int i = 0; i < strLength; i++)
            {
                if (i > 0)
                    encodedStringBuilder.Append(intercharacterGap);

                encodedStringBuilder.Append(coded39Char[alphabet39.IndexOf(str[i])]);
            }

            string encodedString = encodedStringBuilder.ToString();
            int encodedStringLength = encodedString.Length;
            int widthOfBarcodeString = 0;
            const double wideToNarrowRatio = 3;

            if (VerticalAlign != AlignType.Left)
            {
                foreach (char c in encodedString)
                {
                    widthOfBarcodeString += c == '1' ? (int)(wideToNarrowRatio * (int)Weight) : (int)Weight;
                }
            }

            int x = 0;
            int wid = 0;
            int yTop = 0;

            Bitmap bmp = new Bitmap(Width, Height);
            Graphics graphics = Graphics.FromImage(bmp);
            graphics.FillRectangle(Brushes.White, 0, 0, Width, Height);

            SizeF footerSize = graphics.MeasureString(upperCode, FooterFont);

            int footerX = 0;

            if (VerticalAlign == AlignType.Left)
            {
                x = LeftMargin;
                footerX = LeftMargin;
            }
            else if (VerticalAlign == AlignType.Center)
            {
                x = (Width - widthOfBarcodeString) / 2;
                footerX = (Width - (int)footerSize.Width) / 2;
            }
            else
            {
                x = Width - widthOfBarcodeString - LeftMargin;
                footerX = Width - (int)footerSize.Width - LeftMargin;
            }

            yTop = TopMargin;

            for (int i = 0; i < encodedStringLength; i++)
            {
                wid = encodedString[i] == '1' ? (int)(wideToNarrowRatio * (int)Weight) : (int)Weight;

                graphics.FillRectangle(i % 2 == 0 ? Brushes.Black : Brushes.White, x, yTop, wid, BarcodeHeight);

                x += wid;
            }

            yTop += BarcodeHeight;

            if (ShowFooter)
                graphics.DrawString(upperCode, FooterFont, Brushes.Black, footerX, yTop);

            var memoryStream = new System.IO.MemoryStream();
            bmp.Save(memoryStream, BarcodeImageFormat);
            return memoryStream;
        }

        /// <summary>
        /// Generates a barcode and returns it as a byte array.
        /// </summary>
        /// <param name="code">The code to encode in the barcode.</param>
        /// <returns>A byte array containing the barcode image.</returns>
        public byte[] GetBarcodeAsByteArray(string code)
        {
            System.IO.MemoryStream memoryStream = GetBarcode(code);
            byte[] fileContents = new byte[MaxFileSize];
            memoryStream.Position = 0;
            int result = memoryStream.Read(fileContents, 0, MaxFileSize);
            byte[] fileContentsStripped = new byte[result];
            Array.Copy(fileContents, fileContentsStripped, result);
            return fileContentsStripped;
        }

        /// <summary>
        /// Generates a barcode and returns it as an Image object.
        /// </summary>
        /// <param name="code">The code to encode in the barcode.</param>
        /// <returns>An Image object containing the barcode image.</returns>
        public Image GetBarcodeAsImage(string code)
        {
            return Image.FromStream(GetBarcode(code));
        }

        /// <summary>
        /// Augments a DataTable with barcode images.
        /// </summary>
        /// <param name="table">The DataTable to augment.</param>
        /// <param name="codeColumn">The name of the column containing the codes.</param>
        /// <param name="imageColumn">The name of the column to store the barcode images.</param>
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

        #region IDisposable Members

        /// <summary>
        /// Disposes the object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">Indicates whether managed resources should be disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    footerFont.Dispose();
                }
            }
            disposed = true;
        }

        /// <summary>
        /// Finalizer for the object.
        /// </summary>
        ~Code39Generator()
        {
            Dispose(false);
        }

        #endregion
    }
}