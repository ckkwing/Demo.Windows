using ImageMagick;
using Microsoft.Win32;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;

namespace Demo.Windows.WPF
{
    /// <summary>
    /// Interaction logic for ImageTestWindow.xaml
    /// </summary>
    public partial class ImageTestWindow : Window
    {
        public string InputImagePath { get; set; }
        public string OutputImagePath { get; set; } = @"E:\Temp\output.jpg";
        public string OutputConvertedImagePath { get; set; } = @"E:\Temp\output_converted.{0}";
        public ImageTestWindow()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
            MagickNET.Initialize();
        }

        private void btnSelectPhoto_Click(object sender, RoutedEventArgs e)
        {
            // 创建OpenFileDialog实例
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // 设置过滤器，例如仅选择文本文件
            //openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            // 设置标题
            openFileDialog.Title = "选择文件";

            // 显示对话框
            bool? result = openFileDialog.ShowDialog();

            // 检查用户是否点击了"确定"按钮
            if (result == true)
            {
                // 获取选择的文件路径
                InputImagePath = openFileDialog.FileName;
                // 处理文件路径，例如显示文件内容或其他操作
            }
        }

        #region 有损压缩
        public void CompressImage(string inputFile, string outputFile, long quality)
        {
            using (Image image = Image.FromFile(inputFile))
            {
                // 设置压缩参数
                EncoderParameters encoderParameters = new EncoderParameters(1);
                EncoderParameter encoderParameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                encoderParameters.Param[0] = encoderParameter;

                // 获取图像编码器信息
                ImageCodecInfo jpegEncoder = GetEncoder(ImageFormat.Jpeg);

                // 保存压缩后的图像
                image.Save(outputFile, jpegEncoder, encoderParameters);
            }
        }

        public Image CompressImage(Image image, long quality, long maxFileSize)
        {
            Image compressedImage = image;
            MemoryStream memoryStream = new MemoryStream();

            // 保存图片到内存流中
            compressedImage.Save(memoryStream, GetEncoder(compressedImage.RawFormat), null);

            // 如果图片大小超过maxFileSize，则继续减小质量
            while (memoryStream.Length > maxFileSize && quality > 0)
            {
                memoryStream.SetLength(0); // 清空内存流
                EncoderParameters encoderParameters = new EncoderParameters(1);
                EncoderParameter encoderParameter = new EncoderParameter(Encoder.Quality, quality);
                encoderParameters.Param[0] = encoderParameter;
                compressedImage.Save(memoryStream, GetEncoder(compressedImage.RawFormat), encoderParameters);
                quality -= 10; // 减少质量
            }

            // 将内存流转换为字节数组
            byte[] imageBytes = memoryStream.ToArray();

            // 重新加载图片
            compressedImage = Image.FromStream(new MemoryStream(imageBytes));

            memoryStream.Dispose();
            return compressedImage;
        }

        public void CompressImage(string sourcePath, string outputPath, int maxWidth, int maxHeight)
        {
            using (Image sourceImage = Image.FromFile(sourcePath))
            {
                double aspectRatio = (double)sourceImage.Width / sourceImage.Height;
                int newWidth = maxWidth;
                int newHeight = (int)(maxWidth / aspectRatio);

                if (newHeight > maxHeight)
                {
                    newHeight = maxHeight;
                    newWidth = (int)(maxHeight * aspectRatio);
                }

                using (Bitmap compressedImage = new Bitmap(newWidth, newHeight))
                {
                    using (Graphics graphics = Graphics.FromImage(compressedImage))
                    {
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics.DrawImage(sourceImage, 0, 0, newWidth, newHeight);
                    }

                    compressedImage.Save(outputPath, ImageFormat.Jpeg);
                }
            }
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public void ConvertImage(string inputFile, string outputFile, MagickFormat format)
        {
            using (MagickImage image = new MagickImage(inputFile))
            {
                image.Format = format;
                // Save frame as jpg
                image.Write(outputFile);
            }
        }
        #endregion

        private void btnLossyCompression_Click(object sender, RoutedEventArgs e)
        {
            string sourceImage = InputImagePath;
            if (rdbConvertedSource.IsChecked == true)
            {
                sourceImage = File.Exists(OutputConvertedImagePath) ? OutputConvertedImagePath : InputImagePath;
            }
            CompressImage(sourceImage, OutputImagePath, 50L);
        }

        private void btnImageFormatConverterToJpeg_Click(object sender, RoutedEventArgs e)
        {
            OutputConvertedImagePath = string.Format(OutputConvertedImagePath, MagickFormat.Jpeg.ToString().ToLower());
            ConvertImage(InputImagePath, OutputConvertedImagePath, MagickFormat.Jpeg);
        }

        private void btnImageFormatConverterToHeic_Click(object sender, RoutedEventArgs e)
        {
            OutputConvertedImagePath = string.Format(OutputConvertedImagePath, MagickFormat.Heic.ToString().ToLower());
            ConvertImage(InputImagePath, OutputConvertedImagePath, MagickFormat.Heic);
        }
    }
}
