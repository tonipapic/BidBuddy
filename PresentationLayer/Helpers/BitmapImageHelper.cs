using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using EntitiesLayer.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Drawing.Imaging;

namespace PresentationLayer.Helpers {

    /// <summary>
    /// Used for different conversion between AuctionImage, BitmapImage, Bitmap.
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public class BitmapImageHelper {

        /// <summary>
        /// Converts a list of BitmapImage to AuctionImage.
        /// </summary>
        /// <param name="bitmapImages"></param>
        /// <param name="auctionId"></param>
        /// <returns></returns>
        public List<AuctionImage> ConvertToAuctionImages(List<BitmapImage> bitmapImages, int auctionId) {
            return bitmapImages.Select((e) => ConvertToAuctionImage(e, auctionId)).ToList();
        }

        /// <summary>
        /// Converts a list of AuctionImage to BitmapImage
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        public List<BitmapImage> ConvertToBitmapImages(List<AuctionImage> images) {
            return images.Select((e) => ConvertToBitmapImage(e)).ToList();
        }

        /// <summary>
        /// Converts BitmapImage to AuctionImage.
        /// </summary>
        /// <param name="bitmapImage"></param>
        /// <param name="auctionId"></param>
        /// <returns></returns>
        public AuctionImage ConvertToAuctionImage(BitmapImage bitmapImage, int auctionId) {

            using(var bitmap = ConvertBitmapImageToBitmap(bitmapImage)) {
                using(var resizedBitmap = ResizeBitmap(bitmap, 200)) {

                    int quality = 50;
                    var bytes = EncodeBitmapJpeg(resizedBitmap, quality);

                    var image = new AuctionImage();
                    image.AuctionId = auctionId;
                    image.ImageData = bytes;

                    return image;
                }
            }

        }

        /// <summary>
        /// Converts AuctionImage to BitmapImage
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public BitmapImage ConvertToBitmapImage(AuctionImage image) {
            return ConvertByteArrayToBitmapImage(image.ImageData);
        }

        /// <summary>
        /// Converts Bitmap to BitmapImage
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public BitmapImage ConvertToBitmapImage(Bitmap bitmap) {
            using(var stream = new MemoryStream()) {

                bitmap.Save(stream, ImageFormat.Bmp);

                stream.Position = 0;
                var image = new BitmapImage();

                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();

                return image;
            }
        }

        private Bitmap ResizeBitmap(Bitmap bitmap, int maxSize) {

            var size = new System.Drawing.Size(0, 0);
            float aspectRation = (float)bitmap.Width / (float)bitmap.Height;

            if (bitmap.Width >= bitmap.Height) {
                size.Width = maxSize;
                size.Height = (int)(size.Width / aspectRation);

            } else {
                size.Height = maxSize;
                size.Width = (int)(size.Height * aspectRation);
            }

            var resized = new Bitmap(bitmap, size);
            return resized;
        }

        // https://learn.microsoft.com/en-us/dotnet/desktop/winforms/advanced/how-to-set-jpeg-compression-level?view=netframeworkdesktop-4.8

        private byte[] EncodeBitmapJpeg(Bitmap bitmap, int quality) {

            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);

            byte[] buffer = null;

            using (var memory = new MemoryStream()) {
                bitmap.Save(memory, jpgEncoder, encoderParams);
                buffer = new byte[memory.Length];

                memory.Position = 0;
                memory.Read(buffer, 0, buffer.Length);
            }

            return buffer;
        }

        private ImageCodecInfo GetEncoder(ImageFormat format) {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs) {
                if (codec.FormatID == format.Guid) {
                    return codec;
                }
            }
            return null;
        }

        // https://stackoverflow.com/questions/6484357/converting-bitmapimage-to-bitmap-and-vice-versa

        private Bitmap ConvertBitmapImageToBitmap(BitmapImage bitmapImage) {
            using (MemoryStream outStream = new MemoryStream()) {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                Bitmap bitmap = new Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        // https://stackoverflow.com/questions/9564174/convert-byte-array-to-image-in-wpf

        private BitmapImage ConvertByteArrayToBitmapImage(byte[] bytes) {
            using(var stream = new MemoryStream(bytes)) {
                stream.Position = 0;
                var image = new BitmapImage();

                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();

                return image;
            }
        }

    }
}
