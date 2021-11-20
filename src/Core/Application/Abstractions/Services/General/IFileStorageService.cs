using StoreKit.Domain.Enums;
using StoreKit.Shared.DTOs.General.Requests;
using System.Threading.Tasks;

namespace StoreKit.Application.Abstractions.Services.General
{
    public interface IFileStorageService : ITransientService
    {
        public Task<string> UploadAsync<T>(FileUploadRequest request, FileType supportedFileType)
        where T : class;
    }
}