using System.IO;
using System.Threading.Tasks;
using StoreKit.Application.Abstractions.Services;
using StoreKit.Infrastructure.Services.LocalStorage;

namespace StoreKit.Application.Services.LocalStorage
{
    public interface IImageService : ITransientService
    {
        Task<string> Save(Stream stream, string fileName);
        Task Resize(Stream source, int width, Stream dest);
        Task Rotate(Stream source, Stream dest);
        Task Crop(Stream source, Stream dest, CropParametersDto parameters);
        Task<(int Width, int Height)> GetSize(Stream stream);
        void Delete(string path, StaticFileSettings staticFileSettings);
    }
}