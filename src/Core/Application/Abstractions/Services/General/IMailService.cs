using StoreKit.Shared.DTOs.General.Requests;
using System.Threading.Tasks;

namespace StoreKit.Application.Abstractions.Services.General
{
    public interface IMailService : ITransientService
    {
        Task SendAsync(MailRequest request);
    }
}