using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StoreKit.Infrastructure.Services.LocalStorage;

namespace StoreKit.Application.Services.LocalStorage
{
    public class ImageResizerMiddleware
    {
        private const string WidthQueryParameter = "width";
#pragma warning disable 618
        private readonly IHostingEnvironment _hostingEnvironment;
#pragma warning restore 618
        private readonly IContentTypeProvider _contentTypeProvider;
        private readonly RequestDelegate _next;

        public ImageResizerMiddleware(
            [NotNull] RequestDelegate next,
#pragma warning disable 618
            [NotNull] IHostingEnvironment hostingEnvironment)
#pragma warning restore 618
        {
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _contentTypeProvider = new FileExtensionContentTypeProvider();
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(
            HttpContext context,
            IImageService imageService,
            IOptions<StaticFileSettings> staticFileSettings)
        {
            var requestPath = context.Request.Path;
            var filePath = requestPath.Value;

            if (!TryMatchPath(requestPath, out var path, staticFileSettings.Value))
            {

            }
            else if (!_contentTypeProvider.TryGetContentType(path, out _))
            {
            }
            else if (!File.Exists(PathToFullImage()))
            {
            }
            else if (!context.Request.Query.TryGetValue(WidthQueryParameter, out var stringWidth))
            {
            }
            else if (!double.TryParse(stringWidth.ToString().Replace(",", "."), NumberStyles.Number,
#pragma warning disable SA1117
                CultureInfo.InvariantCulture, out var width))
#pragma warning restore SA1117
            {
            }
            else if (File.Exists(PhysicallPathToResizedImage((int)width)) && !IsOpen(PhysicallPathToResizedImage((int)width)))
            {
                context.Request.Path = VirtualPathToResizedImage((int)width);
            }
            else
            {
                var widthAsInt = (int)width;
                var destPath = PhysicallPathToResizedImage(widthAsInt);

                Directory.CreateDirectory(Directory.GetParent(destPath).FullName);
                using (var stream = File.OpenRead(PathToFullImage()))
                using (var memoryStream = new MemoryStream())
                {
                    await imageService.Resize(stream, widthAsInt, memoryStream);
                    using (var destStream = File.Open(destPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                    {
                        memoryStream.Position = 0;
                        await memoryStream.CopyToAsync(destStream);
                    }
                }

                context.Request.Path = VirtualPathToResizedImage(widthAsInt);
            }

            string PathToFullImage()
            {
                var innerPath = path.Value.StartsWith("/") ? path.Value.Remove(0, 1) : path.Value;
                var pathToFullImage = Path.IsPathRooted(staticFileSettings.Value.PhysicallyImagesPath)
                    ? Path.Combine(staticFileSettings.Value.PhysicallyImagesPath, innerPath)
                    : Path.Combine(
                        _hostingEnvironment.ContentRootPath,
                        staticFileSettings.Value.PhysicallyImagesPath,
                        innerPath);
                return pathToFullImage;
            }

            string PhysicallPathToResizedImage(int width)
            {
                var innerPath = path.Value.StartsWith("/") ? path.Value.Remove(0, 1) : path.Value;
                var pathToResizedImage = Path.IsPathRooted(staticFileSettings.Value.PhysicallyImagesPath)
                    ? Path.Combine(staticFileSettings.Value.PhysicallyImagesPath, $"{width}", innerPath)
                    : Path.Combine(
                        _hostingEnvironment.ContentRootPath,
                        staticFileSettings.Value.PhysicallyImagesPath,
                        $"{width}",
                        innerPath);
                return pathToResizedImage;
            }

            string VirtualPathToResizedImage(int width)
            {
                return $"/{staticFileSettings.Value.VirtualImagesPath}/{width}{path}";
            }

            await _next(context);
        }

        private bool IsOpen(string path)
        {
            const int errorSharingViolation = 32;
            const int errorLockViolation = 33;
            FileStream stream = null;
            try
            {
                stream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException ex)
            {
                var errorCode = Marshal.GetHRForException(ex) & ((1 << 16) - 1);
                if (errorCode == errorSharingViolation || errorCode == errorLockViolation)
                {
                    return true;
                }
            }
            finally
            {
                stream?.Close();
            }

            return false;
        }

        private bool TryMatchPath(PathString requestPath, out PathString subpath, StaticFileSettings staticFileSettings)
        {
            return requestPath.StartsWithSegments(
                new PathString("/" + staticFileSettings.VirtualImagesPath),
                out subpath);
        }
    }
}