using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using StoreKit.Infrastructure.Services.LocalStorage;
using Microsoft.AspNetCore.Hosting;

namespace StoreKit.Application.Services.LocalStorage
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly StaticFileSettings _staticFileSettings;

        public ImageService(IOptions<StaticFileSettings> staticFileSettings, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _staticFileSettings = staticFileSettings.Value;
        }

        public async Task<string> Save(Stream stream, string fileName)
        {
            var customFileName = $"{fileName}";

            var path = Path.Combine(_staticFileSettings.PhysicallyImagesPath, customFileName);
            Directory.GetParent(path).Create();

            using (var fileStream = File.OpenWrite(path))
            {
                await stream.CopyToAsync(fileStream);
            }

            return _staticFileSettings.VirtualImagesPath + Uri.EscapeDataString(customFileName);
        }

        public void Delete(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(path));

            var fullImagePhysicalPath = path
                .Replace(_staticFileSettings.VirtualImagesPath, _staticFileSettings.PhysicallyImagesPath);

            var fileName = Path.GetFileNameWithoutExtension(fullImagePhysicalPath);
            var parentDiretory = Path.Combine(_hostingEnvironment.ContentRootPath, _staticFileSettings.PhysicallyImagesPath);
            foreach (var file in Directory.EnumerateFiles(parentDiretory, "*" + fileName + "*", SearchOption.AllDirectories))
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }

        public async Task Resize(Stream source, int size, Stream dest)
        {
            await Task.Run(() =>
            {
                using (var bitmap = new Bitmap(source))
                {
                    if (bitmap.Size.Width <= size)
                    {
                        bitmap.Save(dest, bitmap.RawFormat);
                        return;
                    }

                    int width = size;
                    int height = Convert.ToInt32(bitmap.Height * ((double)size / bitmap.Width));

                    using (var resized = new Bitmap(width, height))
                    using (var graphics = Graphics.FromImage(resized))
                    {
                        graphics.CompositingQuality = CompositingQuality.HighSpeed;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.CompositingMode = CompositingMode.SourceCopy;
                        bitmap.Rotate();
                        graphics.DrawImage(bitmap, 0, 0, width, height);

                        resized.Save(dest, bitmap.RawFormat);
                    }
                }
            });
        }

        public async Task Crop([NotNull] Stream source, [NotNull] Stream dest, [NotNull] CropParametersDto parameters)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (dest == null) throw new ArgumentNullException(nameof(dest));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            await Task.Run(() =>
            {
                using (var sourceImage = new Bitmap(source))
                using (var newImage = new Bitmap(parameters.Width, parameters.Height))
                using (var graphics = Graphics.FromImage(newImage))
                {
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    sourceImage.Rotate();

                    ValidateParametersAndThrow();

                    graphics.DrawImage(
                        sourceImage,
                        new Rectangle(0, 0, parameters.Width, parameters.Height),
                        new Rectangle(parameters.X, parameters.Y, parameters.Width, parameters.Height),
                        GraphicsUnit.Pixel);

                    newImage.Save(dest, sourceImage.RawFormat);

                    void ValidateParametersAndThrow()
                    {
                        if (parameters.Width > sourceImage.Size.Width - parameters.X)
                        {
                            throw new ArgumentException("Некорректные данные ширины", nameof(parameters.Width));
                        }

                        if (parameters.Height > sourceImage.Size.Height - parameters.Y)
                        {
                            throw new ArgumentException("Некорректные данные высоты", nameof(parameters.Height));
                        }
                    }
                }
            });
        }

        public async Task Rotate([NotNull] Stream source, [NotNull] Stream dest)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (dest == null) throw new ArgumentNullException(nameof(dest));

            await Task.Run(() =>
            {
                using (var bitmap = new Bitmap(source))
                {
                    bitmap.Rotate();
                    bitmap.Save(dest, bitmap.RawFormat);
                }
            });
        }

        public async Task<(int Width, int Height)> GetSize(Stream stream)
        {
            return await Task.Run(() =>
            {
                using (var bitmap = new Bitmap(stream))
                {
                    bitmap.Rotate();
                    return (bitmap.Size.Width, bitmap.Size.Height);
                }
            });
        }

        public void Delete(string path, StaticFileSettings staticFileSettings)
        {
            throw new NotImplementedException();
        }
    }
}