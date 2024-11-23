using System.Drawing;
using ZXing;
using ZXing.Common;

namespace PresentationLayer.Helpers {

    /// <summary>
    /// Used to generate barcodes.
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public class BarcodeGenerator {

        /// <summary>
        /// Generate bitmap containing a PDF417 barcode.
        /// </summary>
        /// <param name="content">Payload of barcode</param>
        /// <param name="imageSize">Size of bitmap</param>
        /// <returns></returns>
        public Bitmap GeneratePdf417(string content, Size imageSize) {

            BarcodeWriter barcodeWriter = new BarcodeWriter();

            barcodeWriter.Format = BarcodeFormat.PDF_417;

            var options = new EncodingOptions() {
                Width = imageSize.Width,
                Height = imageSize.Height,
            };

            options.Hints[EncodeHintType.CHARACTER_SET] = "UTF-8";

            barcodeWriter.Options = options;

            Bitmap bitmap = barcodeWriter.Write(content);
            return bitmap;
        }

    }
}
